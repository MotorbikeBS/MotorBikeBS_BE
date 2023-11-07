using API.DTO;
using API.DTO.BookingDTO;
using API.DTO.BookingNegotiationDTO;
using API.DTO.UserDTO;
using API.Utility;
using API.Validation;
using AutoMapper;
using Azure;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.Mvc;
using Service.Service;
using Service.UnitOfWork;
using System.Net;
using System.Runtime.CompilerServices;

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
		public async Task<IActionResult> StoreBookingOwner(int negotiationId, BookingCreateDTO dto)
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
				var negotiationInDb = await _unitOfWork.NegotiationService.GetFirst(x => x.NegotiationId == negotiationId);
				if (negotiationInDb == null || negotiationInDb.Status != SD.Request_Accept)
				{
					_response.IsSuccess = false;
					_response.ErrorMessages.Add("Không thể đặt lịch, quá trình thương lượng chưa kết thúc!");
					_response.StatusCode = HttpStatusCode.BadRequest;
					return BadRequest(_response);
				}
				var requestInDb = await _unitOfWork.RequestService.GetFirst(x => x.RequestId == negotiationInDb.RequestId);
				if(requestInDb == null || requestInDb.Status != SD.Request_Pending)
				{
					_response.IsSuccess = false;
					_response.ErrorMessages.Add("Không thể đặt lịch, quá trình này đã kết thúc!");
					_response.StatusCode = HttpStatusCode.BadRequest;
					return BadRequest(_response);
				}
				var motor = await _unitOfWork.MotorBikeService.GetFirst(x => x.MotorId == requestInDb.MotorId);
				if(motor == null || motor.MotorStatusId != SD.Status_Consignment && motor.MotorStatusId != SD.Status_nonConsignment)
				{
					_response.IsSuccess = false;
					_response.ErrorMessages.Add("Xe này đã bán hoặc đã bị xóa!");
					_response.StatusCode = HttpStatusCode.BadRequest;
					return BadRequest(_response);
				}
				if(userId != requestInDb.SenderId)
				{
					_response.IsSuccess = false;
					_response.ErrorMessages.Add("Bạn không có quyền này!");
					_response.StatusCode = HttpStatusCode.BadRequest;
					return BadRequest(_response);
				}
				var duplicateBooking = await _unitOfWork.BookingService.Get(x => x.BaseRequestId == negotiationInDb.RequestId);
				if(duplicateBooking.Count() > 0)
				{
					_response.IsSuccess = false;
					_response.ErrorMessages.Add("Bạn đã đặt lịch cho xe này!");
					_response.StatusCode = HttpStatusCode.BadRequest;
					return BadRequest(_response);
				}
				var newBooking = _mapper.Map<Booking>(dto);
				newBooking.DateCreate = DateTime.Now;
				//newBooking.BaseRequestId = negotiationInDb.BaseRequestId;
				newBooking.NegotiationId = negotiationId;
				newBooking.Status = SD.Request_Pending;

				await _unitOfWork.BookingService.Add(newBooking);
				_response.IsSuccess = true;
				_response.StatusCode = HttpStatusCode.OK;
				_response.Message = ("Đặt lịch thành công!");
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

		[Authorize(Roles ="Store, Owner")]
		[HttpGet]
		public async Task<IActionResult>Get()
		{
			try
			{
				var userId = int.Parse(User.FindFirst("UserId")?.Value);
				var roleId = int.Parse(User.FindFirst("RoleId")?.Value);
				IEnumerable<Request> list;
				if(roleId == SD.Role_Store_Id)
				{
					list = await _unitOfWork.RequestService.Get(x => x.SenderId == userId
					&& x.RequestTypeId == SD.Request_Negotiation_Id
					&& x.Status == SD.Request_Pending,
					//&& x.Negotiations.Any(n => n.Bookings.Any(b => b.BookingDate >= DateTime.Now.Date && b.BookingDate <= b.BookingDate.Value.Date.AddDays(7))),
					includeProperties: new string[] { "Negotiations", "Motor", "Motor.MotorStatus", "Motor.MotorbikeImages", "Negotiations.Bookings", "Receiver" });
				}
				else
				{
					list = await _unitOfWork.RequestService.Get(x => x.ReceiverId == userId
					&& x.RequestTypeId == SD.Request_Negotiation_Id
					&& x.Status == SD.Request_Pending,
					//&& x.Negotiations.Any(n => n.Bookings.Any(b => b.BookingDate >= DateTime.Now.Date &&  b.BookingDate <= b.BookingDate.Value.Date.AddDays(7))),
					includeProperties: new string[] { "Negotiations", "Motor", "Motor.MotorStatus", "Motor.MotorbikeImages", "Negotiations.Bookings", "Sender", "Sender.StoreDesciptions" });
				}
				if(list.Count() > 0)
				{
					var response = _mapper.Map<List<BookingNegoRequestResponseDTO>>(list);
					
					response.ForEach(item => item.Motor.Owner = null);
					_response.IsSuccess = true;
					_response.StatusCode = HttpStatusCode.OK;
					_response.Result = response;
					return Ok(_response);
				}
				_response.IsSuccess = false;
				_response.StatusCode = HttpStatusCode.NotFound;
				_response.ErrorMessages.Add("Không có lịch hẹn nào đang chờ!");
				return NotFound(_response);

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

		[Authorize(Roles = "Store")]
		[HttpPut]
		[Route("CancelBooking")]
		public async Task<IActionResult> CancelBooking(int bookingId)
		{
			try
			{
				var userId = int.Parse(User.FindFirst("UserId")?.Value);
				var booking = await _unitOfWork.BookingService.GetFirst(x => x.BookingId == bookingId);
				if (booking == null)
				{
					_response.IsSuccess = false;
					_response.ErrorMessages.Add("Không tìm thấy lịch hẹn!");
					_response.StatusCode = HttpStatusCode.NotFound;
					return NotFound(_response);
				}
				var request = await _unitOfWork.RequestService.GetFirst(x => x.RequestId == booking.BaseRequestId);
				if (request == null)
				{
					_response.IsSuccess = false;
					_response.ErrorMessages.Add("Không tìm thấy yêu cầu!");
					_response.StatusCode = HttpStatusCode.NotFound;
					return NotFound(_response);
				}
				if (request.SenderId != userId)
				{
					_response.IsSuccess = false;
					_response.ErrorMessages.Add("Bạn không có quyền này!");
					_response.StatusCode = HttpStatusCode.BadRequest;
					return BadRequest(_response);
				}
				var contract = await _unitOfWork.ContractService.GetFirst(x => x.BaseRequestId == request.RequestId);
				if(contract != null && contract.Status == SD.Request_Accept)
				{
					_response.IsSuccess = false;
					_response.ErrorMessages.Add("Đã tạo hợp đồng, không thể hủy lịch hẹn!");
					_response.StatusCode = HttpStatusCode.BadRequest;
					return BadRequest(_response);
				}
				if (booking.Status != SD.Request_Pending)
				{
					_response.IsSuccess = false;
					_response.ErrorMessages.Add("Không thể hủy lịch hẹn!");
					_response.StatusCode = HttpStatusCode.BadRequest;
					return BadRequest(_response);
				}
				booking.Status = SD.Request_Cancel;
				
				request.Status = SD.Request_Cancel;

				await _unitOfWork.RequestService.Update(request);
				await _unitOfWork.BookingService.Update(booking);

				_response.IsSuccess = true;
				_response.StatusCode = HttpStatusCode.OK;
				_response.Message = "Hủy lịch hẹn thành công!";
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