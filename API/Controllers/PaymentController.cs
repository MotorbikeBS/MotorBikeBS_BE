using API.DTO;
using API.DTO.VnPayDTO;
using API.Utility;
using AutoMapper;
using Core.Models;
using Core.VnPayModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.BlobImageService;
using Service.UnitOfWork;
using Service.VnPay.Service;
using System.Net;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private ApiResponse _response;
        private readonly IMapper _mapper;
        private readonly IVnPayService _vnPayService;

        public PaymentController(IUnitOfWork unitOfWork, IMapper mapper, IVnPayService vnPayService)
        {
            _unitOfWork = unitOfWork;
            _response = new ApiResponse();
            _mapper = mapper;
            _vnPayService = vnPayService;
        }

        [Authorize(Roles = "Store")]
        [HttpPost]
        [Route("CreatePaymentUrl")]
        public async Task<IActionResult> CreatePaymentUrl(PaymentCreateModel model)
        {
            try
            {
                if (model.Amount == default(int) || model.Amount < 10000 || model.Amount > 10000000)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages.Add("Số tiền từ 10.000VNĐ đến 10.000.000VNĐ!");
                    _response.IsSuccess = false;
                    return BadRequest(_response);
                }
                var url = _vnPayService.CreatePaymentUrl(model, HttpContext);
                if (url == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages.Add("Đã xảy ra lỗi!");
                    _response.IsSuccess = false;
                    return BadRequest(_response);
                }
                var userId = int.Parse(User.FindFirst("UserId")?.Value);
                Request request = new()
                {
                    SenderId = userId,
                    Time = DateTime.Now,
                    RequestTypeId = SD.Request_Add_Point_Id,
                    Status = SD.Request_Pending,
                };
                await _unitOfWork.RequestService.Add(request);

                Payment payment = new()
                {
                    RequestId = request.RequestId,
                    Content = $"Nạp {model.Amount}VNĐ",
                    DateCreated = DateTime.Now,
                    PaymentType = "Nạp điểm"
                };
                await _unitOfWork.PaymentService.Add(payment);

                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Result = url;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages = new List<string>()
                {
                    ex.ToString()
                };
                return BadRequest(_response);
            }

        }

        [HttpGet]
        [Route("PaymentCallBack")]
        public async Task<IActionResult> PaymentCallback()
        {
            try
            {
                var response = _vnPayService.PaymentExecute(Request.Query);
                var userId = int.Parse(User.FindFirst("UserId")?.Value);
                var request = await _unitOfWork.RequestService.GetLast(x => x.SenderId == userId
                && x.RequestTypeId == SD.Request_Add_Point_Id
                && x.Status == SD.Request_Pending);
                if(request != null)
                {
                    if (response.VnPayResponseCode == "00")
                    {

                        var store = await _unitOfWork.StoreDescriptionService.GetFirst(x => x.UserId == userId);
                        if (store == null)
                        {
                            _response.StatusCode = HttpStatusCode.NotFound;
                            _response.ErrorMessages.Add("Không tìm thấy cửa hàng!");
                            _response.IsSuccess = false;
                            _response.Result = response;
                            return NotFound(_response);
                        }

                        request.Status = SD.Request_Accept;
                        await _unitOfWork.RequestService.Update(request);

                        var payment = await _unitOfWork.PaymentService.GetFirst(x => x.RequestId == request.RequestId);
                        payment.PaymentTime = DateTime.Now;
                        await _unitOfWork.PaymentService.Update(payment);

                        if (store.Point == null)
                            store.Point = response.Amount;
                        else
                            store.Point = store.Point + response.Amount;
                        await _unitOfWork.StoreDescriptionService.Update(store);
                        _response.StatusCode = HttpStatusCode.OK;
                        _response.ErrorMessages.Add("Nạp điểm thành công!");
                        _response.IsSuccess = false;
                        _response.Result = response;
                        return Ok(_response);
                    }
                    else
                    {
                        request.Status = SD.Request_Cancel;
                        await _unitOfWork.RequestService.Update(request);
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.ErrorMessages.Add("Thanh toán không thành công!");
                        _response.IsSuccess = false;
                        return BadRequest(_response);
                    }
                }
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("Không tìm thấy yêu cầu thanh toán!");
                _response.IsSuccess = false;
                return BadRequest(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages = new List<string>()
                {
                    ex.ToString()
                };
                return BadRequest(_response);
            }
        }
    }
}
