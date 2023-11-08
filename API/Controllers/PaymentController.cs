using API.DTO;
using API.DTO.VnPayDTO;
using AutoMapper;
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

        [HttpPost]
        [Route("CreatePaymentUrl")]
        public IActionResult CreatePaymentUrl(PaymentInformationModel model)
        {
            var url = _vnPayService.CreatePaymentUrl(model, HttpContext);

            return Ok(url);
        }

        //[HttpGet]
        //[Route("PaymentCallBack")]
        //public async IActionResult PaymentCallback()
        //{
        //    try
        //    {
        //        var response = _vnPayService.PaymentExecute(Request.Query);
        //        if (response.Success)
        //        {
                    
        //            _response.StatusCode = HttpStatusCode.OK;
        //            _response.ErrorMessages.Add("Nạp điểm thành công!");
        //            _response.IsSuccess = false;
        //            return NotFound(_response);
        //        }
        //        _response.StatusCode = HttpStatusCode.NotFound;
        //        _response.ErrorMessages.Add("Không tìm thấy cửa hàng nào!");
        //        _response.IsSuccess = false;
        //        return NotFound(_response);
        //    }
        //    catch (Exception ex)
        //    {
        //        _response.IsSuccess = false;
        //        _response.StatusCode = HttpStatusCode.BadRequest;
        //        _response.ErrorMessages = new List<string>()
        //        {
        //            ex.ToString()
        //        };
        //        return BadRequest(_response);
        //    }
        //}
    }
}
