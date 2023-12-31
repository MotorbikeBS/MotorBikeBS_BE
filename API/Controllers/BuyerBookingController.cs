﻿using API.DTO;
using API.DTO.BookingDTO;
using API.DTO.BuyerBookingDTO;
using API.Utility;
using API.Validation;
using AutoMapper;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.UnitOfWork;
using System.Collections.Generic;
using System.Net;

namespace API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class BuyerBookingController : ControllerBase
	{
		private readonly IUnitOfWork _unitOfWork;
		private ApiResponse _response;
		private readonly IMapper _mapper;
        public DateTime VnDate = DateTime.Now.ToLocalTime();

        public BuyerBookingController(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_response = new ApiResponse();
			_mapper = mapper;
		}

		[Authorize(Roles = "Customer")]
		[HttpPost]
		[Route("BookingNonConsignment")]
		public async Task<IActionResult> BookingNonConsignment(int motorId, BuyerBookingCreateDTO dto)
		{
			try
			{
				var rs = InputValidation.BookingNonConsignmentTimeValidation(dto.BookingDate.Value);
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
					_response.StatusCode = HttpStatusCode.NotFound;
					_response.ErrorMessages.Add("Không tìm thấy xe máy!");
					return NotFound(_response);
				}

				IEnumerable<Request> list = await _unitOfWork.RequestService.Get(x => x.SenderId == userId
				&& x.RequestTypeId == SD.Request_Booking_Id
				&& x.MotorId == motorId
				&& x.Status == SD.Request_Pending
				&& x.BuyerBookings.Any(y => y.BookingDate > VnDate));

				if (list.Count() > 0)
				{
					_response.IsSuccess = false;
					_response.ErrorMessages.Add("Bạn đã đặt lịch cho xe này, vui lòng chờ xác nhận!");
					_response.StatusCode = HttpStatusCode.BadRequest;
					return BadRequest(_response);
				}

				if (motor.MotorStatusId == SD.Status_Consignment || motor.MotorStatusId == SD.Status_nonConsignment)
				{
					var userIdStore = await _unitOfWork.StoreDescriptionService.GetFirst(x => x.StoreId == motor.StoreId);
					if(userIdStore == null)
					{
						_response.IsSuccess = false;
						_response.ErrorMessages.Add("Không tìm thấy người dùng!");
						_response.StatusCode = HttpStatusCode.BadRequest;
						return BadRequest(_response);
					}
					Request request = new()
					{
						MotorId = motorId,
						ReceiverId = userIdStore.UserId,
						SenderId = userId,
						Time = VnDate,
						RequestTypeId = SD.Request_Booking_Id,
						Status = SD.Request_Pending
					};
					await _unitOfWork.RequestService.Add(request);

					var bookingCreate = _mapper.Map<BuyerBooking>(dto);
					bookingCreate.RequestId = request.RequestId;
					bookingCreate.DateCreate = VnDate;
					bookingCreate.Status = SD.Request_Pending;

					await _unitOfWork.BuyerBookingService.Add(bookingCreate);
					_response.IsSuccess = true;
					_response.StatusCode = HttpStatusCode.OK;
					_response.Message = "Đặt lịch thành công, vui lòng chờ người bán xác nhận!";
					return Ok(_response);
				}
				else
				{
					_response.IsSuccess = false;
					_response.ErrorMessages.Add("Bạn không thể đặt lịch xem xe này!");
					_response.StatusCode = HttpStatusCode.BadRequest;
					return BadRequest(_response);
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


		[Authorize(Roles = "Store, Customer")]
		[HttpGet]
		[Route("GetBookingRequest")]
		public async Task<IActionResult> GetBookingRequest()
		{
			try
			{
				var roleId = int.Parse(User.FindFirst("RoleId")?.Value);
				var userId = int.Parse(User.FindFirst("UserId")?.Value);

				IEnumerable<Request> requestBooking;

				if (roleId == SD.Role_Store_Id)
				{
					requestBooking = await _unitOfWork.RequestService.Get(x => x.ReceiverId == userId
					&& x.RequestTypeId == SD.Request_Booking_Id
					&& x.BuyerBookings.Any(y => y.BookingDate > VnDate)
					&& x.Status != SD.Request_Cancel 
					&& x.Status != SD.Request_Reject,
					includeProperties: new string[] { "BuyerBookings", "Motor", "Motor.MotorStatus", "Motor.MotorbikeImages", "Sender" });
				}
				else
				{
					requestBooking = await _unitOfWork.RequestService.Get(x => x.SenderId == userId
					&& x.RequestTypeId == SD.Request_Booking_Id
					&& x.Status != SD.Request_Cancel
					&& x.BuyerBookings.Any(y => y.BookingDate > VnDate),
					includeProperties: new string[] { "BuyerBookings", "Motor", "Motor.MotorStatus", "Motor.MotorbikeImages", "Receiver", "Receiver.StoreDescriptions" });
				}

				//var response = new List<BookingResponseRequestDTO>();

				//foreach (var rs in requestBooking)
				//{
				//	var bookingResponse = _mapper.Map<BookingResponseRequestDTO>(rs);
				//	var sender = await _unitOfWork.StoreDescriptionService.GetFirst(x => x.UserId == bookingResponse.SenderId);
				//	bookingResponse.Sender = sender;
				//	if (bookingResponse.BuyerBookings != null)
				//	{
				//		var filteredBookings = bookingResponse.BuyerBookings
				//			.Where(b => b.BookingDate.HasValue && b.BookingDate.Value > VnDate)
				//			.ToList();

				//		if (filteredBookings.Any())
				//		{
				//			bookingResponse.BuyerBookings = filteredBookings;
				//			response.Add(bookingResponse);
				//		}
				//	}
				//}
				if (requestBooking.Count() < 1)
				{
					_response.IsSuccess = false;
					_response.StatusCode = HttpStatusCode.NotFound;
					_response.ErrorMessages.Add("Hiện không có lịch hẹn nào đang chờ!");
					return NotFound(_response);
				}

				var response = _mapper.Map<List<BookingResponseRequestDTO>>(requestBooking);
				response.ForEach(item => item.Motor.Owner = null);
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

		[Authorize(Roles = "Store")]
		[HttpPut]
		[Route("AcceptBooking")]
		public async Task<IActionResult> AcceptBooking(int bookingId)
		{
			try
			{
				var booking = await _unitOfWork.BuyerBookingService.GetFirst(x => x.BookingId == bookingId);
				if (booking == null)
				{
					_response.IsSuccess = false;
					_response.ErrorMessages.Add("Không tìm thấy lịch hẹn!");
					_response.StatusCode = HttpStatusCode.NotFound;
					return NotFound(_response);
				}
				if (booking.Status != SD.Request_Pending)
				{
					_response.IsSuccess = false;
					_response.ErrorMessages.Add("Không thể đồng ý lịch hẹn!");
					_response.StatusCode = HttpStatusCode.BadRequest;
					return BadRequest(_response);
				}
				booking.Status = SD.Request_Accept;
				var request = await _unitOfWork.RequestService.GetFirst(x => x.RequestId == booking.RequestId);
				if (request == null)
				{
					_response.IsSuccess = false;
					_response.ErrorMessages.Add("Không tìm thấy yêu cầu!");
					_response.StatusCode = HttpStatusCode.NotFound;
					return NotFound(_response);
				}

				var motor = await _unitOfWork.MotorBikeService.GetFirst(x => x.MotorId == request.MotorId);
				if (motor == null || motor.MotorStatusId != SD.Status_Consignment && motor.MotorStatusId != SD.Status_nonConsignment)
				{
					_response.IsSuccess = false;
					_response.ErrorMessages.Add("Xe này đã bán hoặc đã bị xóa, không thể đồng ý!!");
					_response.StatusCode = HttpStatusCode.BadRequest;
					return BadRequest(_response);
				}

				request.Status = SD.Request_Accept;

				await _unitOfWork.RequestService.Update(request);
				await _unitOfWork.BuyerBookingService.Update(booking);

				Notification newUserNoti = new()
				{
					RequestId = request.RequestId,
					UserId = request.SenderId,
					Title = "Lịch xem xe đã được chấp nhận",
					Content = "Lịch xem xe " + motor.MotorName + " đã được xác nhận, vui lòng đến cửa hàng đúng hẹn.",
					NotificationTypeId = SD.NotificationType_TranferOwnership,
					Time = VnDate,
					IsRead = false
				};
				await _unitOfWork.NotificationService.Add(newUserNoti);

				_response.IsSuccess = true;
				_response.StatusCode = HttpStatusCode.OK;
				_response.Message = "Đồng ý lịch hẹn thành công!";
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

		[Authorize(Roles = "Store")]
		[HttpPut]
		[Route("RejectBooking")]
		public async Task<IActionResult> RejectBooking(int bookingId)
		{
			try
			{
				var booking = await _unitOfWork.BuyerBookingService.GetFirst(x => x.BookingId == bookingId);
				if (booking == null)
				{
					_response.IsSuccess = false;
					_response.ErrorMessages.Add("Không tìm thấy lịch hẹn!");
					_response.StatusCode = HttpStatusCode.NotFound;
					return NotFound(_response);
				}
				if (booking.Status != SD.Request_Pending)
				{
					_response.IsSuccess = false;
					_response.ErrorMessages.Add("Không thể từ chối lịch hẹn!");
					_response.StatusCode = HttpStatusCode.BadRequest;
					return BadRequest(_response);
				}
				booking.Status = SD.Request_Reject;
				var request = await _unitOfWork.RequestService.GetFirst(x => x.RequestId == booking.RequestId);
				if (request == null)
				{
					_response.IsSuccess = false;
					_response.ErrorMessages.Add("Không tìm thấy yêu cầu!");
					_response.StatusCode = HttpStatusCode.NotFound;
					return NotFound(_response);
				}
				request.Status = SD.Request_Reject;

				await _unitOfWork.RequestService.Update(request);
				await _unitOfWork.BuyerBookingService.Update(booking);

                var motor = await _unitOfWork.MotorBikeService.GetFirst(x => x.MotorId == request.MotorId);

                Notification newUserNoti = new()
                {
                    RequestId = request.RequestId,
                    UserId = request.SenderId,
                    Title = "Lịch xem xe đã bị từ chối",
                    Content = "Lịch xem xe " + motor.MotorName + " đã bị từ chối, vui lòng liên hệ lại.",
                    NotificationTypeId = SD.NotificationType_TranferOwnership,
                    Time = VnDate,
                    IsRead = false
                };
                await _unitOfWork.NotificationService.Add(newUserNoti);

                _response.IsSuccess = true;
				_response.StatusCode = HttpStatusCode.OK;
				_response.Message = "Từ chối lịch hẹn thành công!";
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

		[Authorize(Roles = "Customer")]
		[HttpPut]
		[Route("CancelBooking")]
		public async Task<IActionResult> CancelBooking(int bookingId)
		{
			try
			{
				var booking = await _unitOfWork.BuyerBookingService.GetFirst(x => x.BookingId == bookingId);
				if (booking == null)
				{
					_response.IsSuccess = false;
					_response.ErrorMessages.Add("Không tìm thấy lịch hẹn!");
					_response.StatusCode = HttpStatusCode.NotFound;
					return NotFound(_response);
				}
				if (booking.Status != SD.Request_Pending)
				{
					_response.IsSuccess = false;
					_response.ErrorMessages.Add("Không thể hủy lịch hẹn!");
					_response.StatusCode = HttpStatusCode.BadRequest;
					return BadRequest(_response);
				}
				booking.Status = SD.Request_Cancel;
				var request = await _unitOfWork.RequestService.GetFirst(x => x.RequestId == booking.RequestId);
				if (request == null)
				{
					_response.IsSuccess = false;
					_response.ErrorMessages.Add("Không tìm thấy yêu cầu!");
					_response.StatusCode = HttpStatusCode.NotFound;
					return NotFound(_response);
				}
				request.Status = SD.Request_Cancel;

				await _unitOfWork.RequestService.Update(request);
				await _unitOfWork.BuyerBookingService.Update(booking);


                var motor = await _unitOfWork.MotorBikeService.GetFirst(x => x.MotorId == request.MotorId);

                Notification newUserNoti = new()
                {
                    RequestId = request.RequestId,
                    UserId = request.SenderId,
                    Title = "Lịch xem xe đã bị từ chối",
                    Content = "Lịch xem xe " + motor.MotorName + " đã bị từ chối, vui lòng liên hệ lại.",
                    NotificationTypeId = SD.NotificationType_TranferOwnership,
                    Time = VnDate,
                    IsRead = false
                };
                await _unitOfWork.NotificationService.Add(newUserNoti);

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
