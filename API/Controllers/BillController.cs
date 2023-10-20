//using API.DTO;
//using API.DTO.BookingDTO;
//using API.DTO.MotorbikeDTO;
//using API.DTO.UserDTO;
//using API.Utility;
//using API.Validation;
//using AutoMapper;
//using Azure;
//using Core.Models;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Service.Service;
//using Service.UnitOfWork;
//using System.Net;
//using System.Runtime.CompilerServices;

//namespace API.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class BillController : ControllerBase
//	{
//		private readonly IUnitOfWork _unitOfWork;
//		private ApiResponse _response;
//		private readonly IMapper _mapper;

//		public BillController(IUnitOfWork unitOfWork, IMapper mapper, IEmailSender emailSender)
//		{
//			_unitOfWork = unitOfWork;
//			_response = new ApiResponse();
//			_mapper = mapper;
//		}

//        [Authorize]
//        [HttpGet]
//        public async Task<IActionResult> GetByReceiverID(int ReceiverID)
//        {
//            try
//            {
//                var listDatabase = await _unitOfWork.BillService.Get(e => e.StoreId == ReceiverID, includeProperties: "Request");
//                if (listDatabase == null || listDatabase.Count() <= 0)
//                {
//                    _response.ErrorMessages.Add("Không tìm thấy hóa đơn nào!");
//                    _response.IsSuccess = false;
//                    _response.StatusCode = HttpStatusCode.BadRequest;
//                    return NotFound(_response);
//                }
//                else
//                {
//                    _response.IsSuccess = true;
//                    _response.StatusCode = HttpStatusCode.OK;
//                    _response.Result = listDatabase;
//                }
//                return Ok(_response);
//            }
//            catch (Exception ex)
//            {
//                _response.IsSuccess = false;
//                _response.StatusCode = HttpStatusCode.BadRequest;
//                _response.ErrorMessages = new List<string>()
//                {
//                    ex.ToString()
//                };
//                return BadRequest(_response);
//            }
//        }

//        [Authorize]
//        [HttpGet]
//        public async Task<IActionResult> Get(int BillID)
//        {
//            try
//            {
//                var listDatabase = await _unitOfWork.BillService.GetFirst(e => e.BillConfirmId == BillID, includeProperties: "Request");
//                if (listDatabase == null)
//                {
//                    _response.ErrorMessages.Add("Không tìm thấy hóa đơn nào!");
//                    _response.IsSuccess = false;
//                    _response.StatusCode = HttpStatusCode.BadRequest;
//                    return NotFound(_response);
//                }
//                else
//                {
//                    _response.IsSuccess = true;
//                    _response.StatusCode = HttpStatusCode.OK;
//                    _response.Result = listDatabase;
//                }
//                return Ok(_response);
//            }
//            catch (Exception ex)
//            {
//                _response.IsSuccess = false;
//                _response.StatusCode = HttpStatusCode.BadRequest;
//                _response.ErrorMessages = new List<string>()
//                {
//                    ex.ToString()
//                };
//                return BadRequest(_response);
//            }
//        }
//    }
//}