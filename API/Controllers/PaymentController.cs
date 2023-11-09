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
                if(model.Amount <5000 || model.Amount > 10000000)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages.Add("Số tiền từ 5.000VNĐ đến 10.000.000VNĐ!");
                    _response.IsSuccess = false;
                    return BadRequest(_response);
                }
                var url = _vnPayService.CreatePaymentUrl(model, HttpContext);
                if(url == null)
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
                    Status = SD.Request_Pending
                };
                await _unitOfWork.RequestService.Add(request);
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
        public IActionResult PaymentCallback()
        {
            try
            {
                var response = _vnPayService.PaymentExecute(Request.Query);
                if (response.Success)
                {
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.ErrorMessages.Add("Nạp điểm thành công!");
                    _response.IsSuccess = false;
                    _response.Result = response;
                    return NotFound(_response);
                }
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("Đã xảy ra lỗi!");
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
