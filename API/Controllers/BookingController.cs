using API.DTO;
using API.DTO.BookingDTO;
using API.DTO.UserDTO;
using API.Utility;
using API.Validation;
using AutoMapper;
using Azure;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Service;
using Service.UnitOfWork;
using System.Net;

namespace API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class BookingController : ControllerBase
	{
		private readonly IUnitOfWork _unitOfWork;
		private ApiResponse _response;
		private readonly IMapper _mapper;

		public BookingController(IUnitOfWork unitOfWork, IMapper mapper, IEmailSender emailSender)
		{
			_unitOfWork = unitOfWork;
			_response = new ApiResponse();
			_mapper = mapper;
		}

		[Authorize(Roles = "Store")]
		[HttpPost]
		[Route("StoreBookingOwner")]
		public async Task<IActionResult> StoreBookingOwner(int motorId, BookingCreateDTO dto)
		{
			try
			{
				var rs = InputValidation.BookingTimeValidation(dto.BookingDate.Value);
				if (rs != "")
				{
					_response.IsSuccess = false;
					_response.ErrorMessages.Add(rs);
					_response.StatusCode = HttpStatusCode.BadRequest;
					return BadRequest(_response);
				}

				var userId = int.Parse(User.FindFirst("UserId")?.Value);
				var motor = await _unitOfWork.MotorBikeService.GetFirst(x => x.MotorId == motorId);
				if (motor == null)
				{
					_response.IsSuccess = false;
					_response.ErrorMessages.Add("Không tìm thấy xe máy!");
					_response.StatusCode = HttpStatusCode.NotFound;
					return NotFound(_response);
				}

				var bookingInDb = await _unitOfWork.RequestService.GetFirst(x => x.SenderId == userId);
				if (bookingInDb != null && bookingInDb.RequestTypeId == SD.Request_Booking_Id && bookingInDb.MotorId == motorId)
				{
					_response.IsSuccess = false;
					_response.ErrorMessages.Add("Bạn đã đặt lịch, vui lòng chờ xác nhận!");
					_response.StatusCode = HttpStatusCode.BadRequest;
					return BadRequest(_response);
				}

				if (motor.MotorStatusId != SD.Status_Consignment)
				{
					_response.IsSuccess = false;
					_response.ErrorMessages.Add("Bạn không thể đặt lịch xem xe này!");
					_response.StatusCode = HttpStatusCode.BadRequest;
					return BadRequest(_response);
				}
				Request request = new()
				{
					MotorId = motorId,
					ReceiverId = motor.OwnerId,
					SenderId = userId,
					Time = DateTime.Now,
					RequestTypeId = SD.Request_Booking_Id,
					Status = SD.Request_Booking_Pending
				};
				await _unitOfWork.RequestService.Add(request);

				var bookingCreate = _mapper.Map<Booking>(dto);
				bookingCreate.RequestId = request.RequestId;
				bookingCreate.DateCreate = DateTime.Now;
				bookingCreate.Status = SD.Request_Booking_Pending;

				await _unitOfWork.BookingService.Add(bookingCreate);
				_response.IsSuccess = true;
				_response.Message = "Đặt lịch thành công, vui lòng chờ người bán xác nhận!";
				_response.StatusCode = HttpStatusCode.OK;
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

		[Authorize]
		[HttpGet("{id:int}")]
		public async Task<IActionResult>Get(int id)
		{
			try
			{
				var userId = int.Parse(User.FindFirst("UserId")?.Value);
				Request request = await _unitOfWork.RequestService.GetFirst(x => x.RequestId == id, includeProperties: "Bookings");
				if(request == null)
				{
					_response.IsSuccess = false;
					_response.ErrorMessages.Add("Không tìm thấy yêu cầu nào!");
					_response.StatusCode = HttpStatusCode.NotFound;
					return NotFound(_response);
				}
				if(request.RequestTypeId != SD.Request_Booking_Id)
				{
					_response.IsSuccess = false;
					_response.ErrorMessages.Add("Đây không phải là yêu cầu đặt lịch!");
					_response.StatusCode = HttpStatusCode.BadRequest;
					return BadRequest(_response);
				}
				_response.IsSuccess = true;
				_response.StatusCode = HttpStatusCode.OK;
				_response.Result = request;
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

		[Authorize]
		[HttpGet]
		[Route("GetBookingRequest")]
		public async Task<IActionResult> GetBookingRequest()
		{
			try
			{
				var userId = int.Parse(User.FindFirst("UserId")?.Value);
				IEnumerable<Request> list = await _unitOfWork.RequestService.Get(x => x.ReceiverId == userId, includeProperties: new string[] { "Bookings", "Motor" });
				IEnumerable<Request> requestBooking = list.Where(x => x.RequestTypeId == SD.Request_Booking_Id);
				if (requestBooking == null)
				{
					_response.IsSuccess = false;
					_response.ErrorMessages.Add("Không tìm thấy yêu cầu nào!");
					_response.StatusCode = HttpStatusCode.NotFound;
					return NotFound(_response);
				}

				var response = new List<BookingResponseRequestDTO>();

				foreach (var rs in requestBooking)
				{
					var bookingResponse = _mapper.Map<BookingResponseRequestDTO>(rs);
					var sender = await _unitOfWork.StoreDescriptionService.GetFirst(x => x.UserId == bookingResponse.SenderId);
					bookingResponse.Sender = sender;
					response.Add(bookingResponse);
				}

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
