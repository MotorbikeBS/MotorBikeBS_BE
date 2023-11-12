using API.DTO;
using API.DTO.ContractDTO;
using API.DTO.PaymentDTO;
using API.DTO.VnPayDTO;
using API.Utility;
using API.Validation;
using AutoMapper;
using Core.Models;
using Core.VnPayModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.BlobImageService;
using Service.UnitOfWork;
using Service.VnPay.Service;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;

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
                var userId = int.Parse(User.FindFirst("UserId")?.Value);
                var rs = InputValidation.PaymentValidate(model.Amount);
                if (!string.IsNullOrEmpty(rs))
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages.Add(rs);
                    _response.IsSuccess = false;
                    return BadRequest(_response);
                }
                var url = _vnPayService.CreatePaymentUrl(model, HttpContext, userId);
                if (url == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages.Add("Đã xảy ra lỗi!");
                    _response.IsSuccess = false;
                    return BadRequest(_response);
                }
                Request request = new()
                {
                    SenderId = userId,
                    Time = DateTime.Now,
                    RequestTypeId = SD.Request_Add_Point_Id,
                    Status = SD.Payment_Unpaid,
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
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> PaymentCallback()
        {
            var paymentFeResponselink = "https://motorbikebs.azurewebsites.net/payment-point/";
            try
            {
                var response = _vnPayService.PaymentExecute(Request.Query);
                var userId = ExtractUserId(response.OrderDescription);
                var request = await _unitOfWork.RequestService.GetLast(x => x.SenderId == userId
                && x.RequestTypeId == SD.Request_Add_Point_Id
                && x.Status == SD.Payment_Unpaid);
                if (request != null)
                {
                    
                    var result = int.Parse(response.VnPayResponseCode);
                    if (result == 00)
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

                        request.Status = SD.Payment_Paid;
                        await _unitOfWork.RequestService.Update(request);

                        var payment = await _unitOfWork.PaymentService.GetFirst(x => x.RequestId == request.RequestId);
                        payment.PaymentTime = DateTime.Now;
                        payment.VnpayOrderId = response.OrderId;
                        await _unitOfWork.PaymentService.Update(payment);

                        if (store.Point == null)
                            store.Point = response.Amount;
                        else
                            store.Point = store.Point + (response.Amount / 100000);
                        await _unitOfWork.StoreDescriptionService.Update(store);

                        return Redirect($"{paymentFeResponselink}" + "successfully");
                    }
                    else
                    {
                        switch (result)
                        {
                            case 07:
                                _response.ErrorMessages.Add("Trừ tiền thành công. Giao dịch bị nghi ngờ (liên quan tới lừa đảo, giao dịch bất thường).!");
                                break;
                            case 09:
                                _response.ErrorMessages.Add("Giao dịch không thành công do: Thẻ/Tài khoản của khách hàng chưa đăng ký dịch vụ InternetBanking tại ngân hàng.!");
                                break;
                            case 10:
                                _response.ErrorMessages.Add("Giao dịch không thành công do: Khách hàng xác thực thông tin thẻ/tài khoản không đúng quá 3 lần!");
                                break;
                            case 11:
                                _response.ErrorMessages.Add("Giao dịch không thành công do: Đã hết hạn chờ thanh toán. Xin quý khách vui lòng thực hiện lại giao dịch.!");
                                break;
                            case 12:
                                _response.ErrorMessages.Add("Giao dịch không thành công do: Thẻ/Tài khoản của khách hàng bị khóa.!");
                                break;
                            case 13:
                                _response.ErrorMessages.Add("Giao dịch không thành công do Quý khách nhập sai mật khẩu xác thực giao dịch (OTP). Xin quý khách vui lòng thực hiện lại giao dịch.!");
                                break;
                            case 24:
                                _response.ErrorMessages.Add("Giao dịch không thành công do: Khách hàng hủy giao dịch!");
                                break;
                            case 51:
                                _response.ErrorMessages.Add("Giao dịch không thành công do: Tài khoản của quý khách không đủ số dư để thực hiện giao dịch.!");
                                break;
                            case 65:
                                _response.ErrorMessages.Add("Giao dịch không thành công do: Tài khoản của Quý khách đã vượt quá hạn mức giao dịch trong ngày.!");
                                break;
                            case 75:
                                _response.ErrorMessages.Add("Ngân hàng thanh toán đang bảo trì.!");
                                break;
                            case 79:
                                _response.ErrorMessages.Add("Giao dịch không thành công do: KH nhập sai mật khẩu thanh toán quá số lần quy định. Xin quý khách vui lòng thực hiện lại giao dịch!");
                                break;
                            case 99:
                                _response.ErrorMessages.Add("Thanh toán không thành công, đã có lỗi xảy ra!");
                                break;
                        }

                        request.Status = SD.Payment_Error;
                        await _unitOfWork.RequestService.Update(request);

                        return Redirect($"{paymentFeResponselink}" + "error-payment");
                    }
                }
                return Redirect($"{paymentFeResponselink}" + "error-payment");
            }
            catch (Exception ex)
            {
                return Redirect($"{paymentFeResponselink}" + "error-payment");
            }
        }

        [Authorize(Roles =("Store"))]
        [HttpGet]
        [Route("GetPaymentHistory")]
        public async Task<IActionResult>GetPaymentHistory()
        {
            try
            {
                var userId = int.Parse(User.FindFirst("UserId")?.Value);
                var payment = await _unitOfWork.RequestService.Get(x => x.RequestTypeId == SD.Request_Add_Point_Id
                && x.SenderId == userId
                && x.Payments.Any(y => y.RequestId != default(int)),
                includeProperties: "Payments");

                if(payment.Count() < 1)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.ErrorMessages.Add("Bạn chưa thực hiện thanh toán!");
                    _response.IsSuccess = false;
                    return NotFound(_response);
                }
                var response = _mapper.Map<List<PaymentRequestResponseDTO>>(payment);
                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = response;
                _response.IsSuccess = false;
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


        static int ExtractUserId(string input)
        {
            string pattern = @"UserId:(\d+)";

            Match match = Regex.Match(input, pattern);

            if (match.Success)
            {
                string userIdStr = match.Groups[1].Value;

                // Parse the extracted string to an integer
                if (int.TryParse(userIdStr, out int userId))
                {
                    return userId;
                }
            }

            return -1;
        }
    }
}
