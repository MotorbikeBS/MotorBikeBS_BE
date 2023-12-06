using API.DTO;
using API.DTO.PaymentDTO;
using API.Utility;
using AutoMapper;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.UnitOfWork;
using System.Net;
using System.Runtime.CompilerServices;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentStatisticController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private ApiResponse _response;

        public PaymentStatisticController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _response = new ApiResponse();
        }

        [HttpGet]
        [Route("GetToalByAdmin")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> GetToalByAdmin(int year, int? storeId)
        {
            try
            {
                if (year == default)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages.Add("Vui lòng nhập năm!");
                    return BadRequest(_response);
                }
                if (year < 2023 || year > DateTime.Now.Year)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages.Add("Chưa có dữ liệu!");
                    return BadRequest(_response);
                }
                
                if (storeId != default)
                {
                    var store = await _unitOfWork.StoreDescriptionService.GetFirst(x => x.StoreId == storeId);
                    if (store == null)
                    {
                        _response.IsSuccess = false;
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.ErrorMessages.Add("Cửa hàng không tồn tại!");
                        return BadRequest(_response);
                    }
                    else
                    {
                        IEnumerable<Request> paymentRequest = await _unitOfWork.RequestService.Get(x => x.RequestTypeId == SD.Request_Add_Point_Id
                        && x.Status == SD.Payment_Paid
                        && x.SenderId == store.UserId
                        && x.Payments.Any(y => y.PaymentTime != null
                        && y.PaymentTime.Value.Year == year),
                        includeProperties: "Payments");

                        decimal sum = 0;

                        Dictionary<int, decimal> monthlySum = new Dictionary<int, decimal>();

                        if (paymentRequest.Any())
                        {
                            for (int i = 1; i <= 12; i++)
                            {
                                var paymentsInMonth = paymentRequest
                                    .Where(x => x.Payments.Any(y => y.PaymentTime.Value.Month == i))
                                    .SelectMany(x => x.Payments.Where(y => y.PaymentTime.Value.Month == i));

                                decimal total = (decimal)paymentsInMonth.Sum(y => y.Amount);
                                monthlySum.Add(i, total);
                                sum += total;
                            }
                        }

                        PaymentStatisticResponseDTO response = new()
                        {
                            Month1 = monthlySum.ContainsKey(1) ? monthlySum[1] : null,
                            Month2 = monthlySum.ContainsKey(2) ? monthlySum[2] : null,
                            Month3 = monthlySum.ContainsKey(3) ? monthlySum[3] : null,
                            Month4 = monthlySum.ContainsKey(4) ? monthlySum[4] : null,
                            Month5 = monthlySum.ContainsKey(5) ? monthlySum[5] : null,
                            Month6 = monthlySum.ContainsKey(6) ? monthlySum[6] : null,
                            Month7 = monthlySum.ContainsKey(7) ? monthlySum[7] : null,
                            Month8 = monthlySum.ContainsKey(8) ? monthlySum[8] : null,
                            Month9 = monthlySum.ContainsKey(9) ? monthlySum[9] : null,
                            Month10 = monthlySum.ContainsKey(10) ? monthlySum[10] : null,
                            Month11 = monthlySum.ContainsKey(11) ? monthlySum[11] : null,
                            Month12 = monthlySum.ContainsKey(12) ? monthlySum[12] : null,
                            Total = sum
                        };

                        _response.IsSuccess = true;
                        _response.StatusCode = HttpStatusCode.OK;
                        _response.Result = response;
                        return Ok(_response);
                    }
                }
                else
                {
                    IEnumerable<Request> paymentRequest = await _unitOfWork.RequestService.Get(x => x.RequestTypeId == SD.Request_Add_Point_Id
                        && x.Status == SD.Payment_Paid
                        && x.Payments.Any(y => y.PaymentTime != null
                        && y.PaymentTime.Value.Year == year),
                        includeProperties: "Payments");

                    decimal sum = 0;

                    Dictionary<int, decimal> monthlySum = new Dictionary<int, decimal>();

                    if (paymentRequest.Any())
                    {
                        for (int i = 1; i <= 12; i++)
                        {
                            var paymentsInMonth = paymentRequest
                                .Where(x => x.Payments.Any(y => y.PaymentTime.Value.Month == i))
                                .SelectMany(x => x.Payments.Where(y => y.PaymentTime.Value.Month == i));

                            decimal total = (decimal)paymentsInMonth.Sum(y => y.Amount);
                            monthlySum.Add(i, total);
                            sum += total;
                        }
                    }

                    PaymentStatisticResponseDTO response = new()
                    {
                        Month1 = monthlySum.ContainsKey(1) ? monthlySum[1] : null,
                        Month2 = monthlySum.ContainsKey(2) ? monthlySum[2] : null,
                        Month3 = monthlySum.ContainsKey(3) ? monthlySum[3] : null,
                        Month4 = monthlySum.ContainsKey(4) ? monthlySum[4] : null,
                        Month5 = monthlySum.ContainsKey(5) ? monthlySum[5] : null,
                        Month6 = monthlySum.ContainsKey(6) ? monthlySum[6] : null,
                        Month7 = monthlySum.ContainsKey(7) ? monthlySum[7] : null,
                        Month8 = monthlySum.ContainsKey(8) ? monthlySum[8] : null,
                        Month9 = monthlySum.ContainsKey(9) ? monthlySum[9] : null,
                        Month10 = monthlySum.ContainsKey(10) ? monthlySum[10] : null,
                        Month11 = monthlySum.ContainsKey(11) ? monthlySum[11] : null,
                        Month12 = monthlySum.ContainsKey(12) ? monthlySum[12] : null,
                        Total = sum
                    };
                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Result = response;
                    return Ok(_response);
                }
                

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages = new List<string>()
                {
                    ex.ToString()
                };
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("GetToalByStore")]
        [Authorize(Roles = "Store")]
        public async Task<IActionResult> GetToalByStore(int year)
        {
            try
            {
                if (year == default)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages.Add("Vui lòng nhập năm!");
                    return BadRequest(_response);
                }
                if (year < 2023 || year > DateTime.Now.Year)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages.Add("Chưa có dữ liệu!");
                    return BadRequest(_response);
                }
                var userId = int.Parse(User.FindFirst("UserId")?.Value);

                    IEnumerable<Request> paymentRequest = await _unitOfWork.RequestService.Get(x => x.RequestTypeId == SD.Request_Add_Point_Id
                        && x.SenderId == userId
                        && x.Status == SD.Payment_Paid
                        && x.Payments.Any(y => y.PaymentTime != null
                        && y.PaymentTime.Value.Year == year),
                        includeProperties: "Payments");

                    decimal sum = 0;

                    Dictionary<int, decimal> monthlySum = new Dictionary<int, decimal>();

                    if (paymentRequest.Any())
                    {
                        for (int i = 1; i <= 12; i++)
                        {
                            var paymentsInMonth = paymentRequest
                                .Where(x => x.Payments.Any(y => y.PaymentTime.Value.Month == i))
                                .SelectMany(x => x.Payments.Where(y => y.PaymentTime.Value.Month == i));

                            decimal total = (decimal)paymentsInMonth.Sum(y => y.Amount);
                            monthlySum.Add(i, total);
                            sum += total;
                        }
                    }

                    PaymentStatisticResponseDTO response = new()
                    {
                        Month1 = monthlySum.ContainsKey(1) ? monthlySum[1] : null,
                        Month2 = monthlySum.ContainsKey(2) ? monthlySum[2] : null,
                        Month3 = monthlySum.ContainsKey(3) ? monthlySum[3] : null,
                        Month4 = monthlySum.ContainsKey(4) ? monthlySum[4] : null,
                        Month5 = monthlySum.ContainsKey(5) ? monthlySum[5] : null,
                        Month6 = monthlySum.ContainsKey(6) ? monthlySum[6] : null,
                        Month7 = monthlySum.ContainsKey(7) ? monthlySum[7] : null,
                        Month8 = monthlySum.ContainsKey(8) ? monthlySum[8] : null,
                        Month9 = monthlySum.ContainsKey(9) ? monthlySum[9] : null,
                        Month10 = monthlySum.ContainsKey(10) ? monthlySum[10] : null,
                        Month11 = monthlySum.ContainsKey(11) ? monthlySum[11] : null,
                        Month12 = monthlySum.ContainsKey(12) ? monthlySum[12] : null,
                        Total = sum
                    };
                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Result = response;
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
                return BadRequest();
            }
        }
    }
}
