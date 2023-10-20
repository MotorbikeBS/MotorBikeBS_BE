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
					_response.ErrorMessages.Add("Không thể đặt lịch!");
					_response.StatusCode = HttpStatusCode.BadRequest;
					return BadRequest(_response);
				}
				var requestInDb = await _unitOfWork.RequestService.GetFirst(x => x.RequestId == negotiationInDb.BaseRequestId);
				if(requestInDb == null || requestInDb.Status != SD.Request_Pending)
				{
					_response.IsSuccess = false;
					_response.ErrorMessages.Add("Không thể đặt lịch, quá trình này đã kết thúc!");
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
				var duplicateBooking = await _unitOfWork.BookingService.Get(x => x.BaseRequestId == negotiationInDb.BaseRequestId);
				if(duplicateBooking.Count() > 0)
				{
					_response.IsSuccess = false;
					_response.ErrorMessages.Add("Bạn đã đặt lịch cho xe này!");
					_response.StatusCode = HttpStatusCode.BadRequest;
					return BadRequest(_response);
				}
				var newBooking = _mapper.Map<Booking>(dto);
				newBooking.DateCreate = DateTime.Now;
				newBooking.BaseRequestId = negotiationInDb.BaseRequestId;
				newBooking.NegotiationId = negotiationId;
				newBooking.Status = SD.Request_Pending;

				await _unitOfWork.BookingService.Add(newBooking);
				_response.IsSuccess = true;
				_response.StatusCode = HttpStatusCode.OK;
				_response.Message = ("Đặt lịch thành công, vui lòng chờ người bán duyệt!");
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
					includeProperties: new string[] { "Negotiations", "Motor", "Negotiations.Bookings", "Receiver" });
				}
				else
				{
					list = await _unitOfWork.RequestService.Get(x => x.ReceiverId == userId
					&& x.RequestTypeId == SD.Request_Negotiation_Id
					&& x.Status == SD.Request_Pending,
					includeProperties: new string[] { "Negotiations", "Motor", "Negotiations.Bookings", "Sender", "Sender.StoreDesciptions" });
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
				_response.Result = list;
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

		//        [Authorize(Roles = "Store")]
		//        [HttpPost]
		//        [Route("StoreBookingOwner")]
		//        public async Task<IActionResult> StoreBookingOwner(int motorId, BookingCreateDTO dto)
		//        {
		//            try
		//            {
		//                var rs = InputValidation.BookingTimeValidation(dto.BookingDate.Value);
		//                if (rs != "")
		//                {
		//                    _response.IsSuccess = false;
		//                    _response.ErrorMessages.Add(rs);
		//                    _response.StatusCode = HttpStatusCode.BadRequest;
		//                    return BadRequest(_response);
		//                }

		//                var userId = int.Parse(User.FindFirst("UserId")?.Value);
		//                var motor = await _unitOfWork.MotorBikeService.GetFirst(x => x.MotorId == motorId);
		//                if (motor == null)
		//                {
		//                    _response.IsSuccess = false;
		//                    _response.ErrorMessages.Add("Không tìm thấy xe máy!");
		//                    _response.StatusCode = HttpStatusCode.NotFound;
		//                    return NotFound(_response);
		//                }

		//                IEnumerable<Request> list = await _unitOfWork.RequestService.Get(x => x.SenderId == userId
		//                && x.RequestTypeId == SD.Request_Booking_Id
		//                && x.MotorId == motorId
		//                && x.Status != SD.Request_Booking_Cancel
		//                && x.Status != SD.Request_Booking_Reject);

		//                if (list.Count() > 0)
		//                {
		//                    _response.IsSuccess = false;
		//                    _response.ErrorMessages.Add("Bạn đã đặt lịch cho xe này, vui lòng chờ xác nhận!");
		//                    _response.StatusCode = HttpStatusCode.BadRequest;
		//                    return BadRequest(_response);
		//                }

		//                if (motor.MotorStatusId == SD.Status_Consignment || motor.MotorStatusId == SD.Status_Livelihood)
		//                {
		//                    Request request = new()
		//                    {
		//                        MotorId = motorId,
		//                        ReceiverId = motor.OwnerId,
		//                        SenderId = userId,
		//                        Time = DateTime.Now,
		//                        RequestTypeId = SD.Request_Booking_Id,
		//                        Status = SD.Request_Booking_Pending
		//                    };
		//                    await _unitOfWork.RequestService.Add(request);

		//                    var bookingCreate = _mapper.Map<Booking>(dto);
		//                    bookingCreate.RequestId = request.RequestId;
		//                    bookingCreate.DateCreate = DateTime.Now;
		//                    bookingCreate.Status = SD.Request_Booking_Pending;

		//                    await _unitOfWork.BookingService.Add(bookingCreate);
		//                    _response.IsSuccess = true;
		//                    _response.Message = "Đặt lịch thành công, vui lòng chờ người bán xác nhận!";
		//                    _response.StatusCode = HttpStatusCode.OK;
		//                    return Ok(_response);
		//                }
		//                else
		//                {
		//                    _response.IsSuccess = false;
		//                    _response.ErrorMessages.Add("Bạn không thể đặt lịch xem xe này!");
		//                    _response.StatusCode = HttpStatusCode.BadRequest;
		//                    return BadRequest(_response);
		//                }

		//            }
		//            catch (Exception ex)
		//            {
		//                _response.IsSuccess = false;
		//                _response.StatusCode = HttpStatusCode.BadRequest;
		//                _response.ErrorMessages = new List<string>()
		//                {
		//                    ex.ToString()
		//                };
		//                return BadRequest();
		//            }
		//        }


		//        [Authorize(Roles = "Store, Owner")]
		//        [HttpGet]
		//        [Route("GetBookingRequest")]
		//        public async Task<IActionResult> GetBookingRequest()
		//        {
		//            try
		//            {
		//                var roleId = int.Parse(User.FindFirst("RoleId")?.Value);
		//                var userId = int.Parse(User.FindFirst("UserId")?.Value);

		//                IEnumerable<Request> requestBooking = null;

		//                if (roleId == SD.Role_Owner_Id)
		//                {
		//                    requestBooking = await _unitOfWork.RequestService.Get(x => x.ReceiverId == userId
		//                    && x.RequestTypeId == SD.Request_Booking_Id, includeProperties: new string[] { "Bookings", "Motor", "Motor.MotorStatus", "Motor.Owner" });
		//                }

		//                if (roleId == SD.Role_Store_Id)
		//                {
		//                    requestBooking = await _unitOfWork.RequestService.Get(x => x.SenderId == userId
		//                    && x.RequestTypeId == SD.Request_Booking_Id, includeProperties: new string[] { "Bookings", "Motor", "Motor.MotorStatus", "Motor.Owner" });
		//                }

		//                if (requestBooking == null)
		//                {
		//                    _response.IsSuccess = false;
		//                    _response.ErrorMessages.Add("Không tìm thấy yêu cầu nào!");
		//                    _response.StatusCode = HttpStatusCode.NotFound;
		//                    return NotFound(_response);
		//                }

		//                var response = new List<BookingResponseRequestDTO>();

		//                foreach (var rs in requestBooking)
		//                {
		//                    var bookingResponse = _mapper.Map<BookingResponseRequestDTO>(rs);
		//                    var sender = await _unitOfWork.StoreDescriptionService.GetFirst(x => x.UserId == bookingResponse.SenderId);
		//                    bookingResponse.Sender = sender;
		//                    if (bookingResponse.Bookings != null)
		//                    {
		//                        var filteredBookings = bookingResponse.Bookings
		//                            .Where(b => b.BookingDate.HasValue && b.BookingDate.Value > DateTime.Now)
		//                            .ToList();

		//                        if (filteredBookings.Any())
		//                        {
		//                            bookingResponse.Bookings = filteredBookings;
		//                            response.Add(bookingResponse);
		//                        }
		//                    }
		//                }
		//                if (response == null || response.Count < 1)
		//                {
		//                    _response.IsSuccess = false;
		//                    _response.StatusCode = HttpStatusCode.NotFound;
		//                    _response.ErrorMessages.Add("Hiện không có lịch hẹn nào đang chờ!");
		//                    return NotFound(_response);
		//                }
		//                _response.IsSuccess = true;
		//                _response.StatusCode = HttpStatusCode.OK;
		//                _response.Result = response;
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
		//                return BadRequest();
		//            }
		//        }

		//        [Authorize(Roles = "Owner")]
		//        [HttpPut]
		//        [Route("AcceptBooking")]
		//        public async Task<IActionResult> AcceptBooking(int bookingId)
		//        {
		//            try
		//            {
		//                var booking = await _unitOfWork.BookingService.GetFirst(x => x.BookingId == bookingId);
		//                if (booking == null)
		//                {
		//                    _response.IsSuccess = false;
		//                    _response.ErrorMessages.Add("Không tìm thấy lịch hẹn!");
		//                    _response.StatusCode = HttpStatusCode.NotFound;
		//                    return NotFound(_response);
		//                }
		//                if (booking.Status != SD.Request_Booking_Pending)
		//                {
		//                    _response.IsSuccess = false;
		//                    _response.ErrorMessages.Add("Không thể đồng ý lịch hẹn!");
		//                    _response.StatusCode = HttpStatusCode.BadRequest;
		//                    return BadRequest(_response);
		//                }
		//                booking.Status = SD.Request_Booking_Accept;
		//                var request = await _unitOfWork.RequestService.GetFirst(x => x.RequestId == booking.RequestId);
		//                if (request == null)
		//                {
		//                    _response.IsSuccess = false;
		//                    _response.ErrorMessages.Add("Không tìm thấy yêu cầu!");
		//                    _response.StatusCode = HttpStatusCode.NotFound;
		//                    return NotFound(_response);
		//                }
		//                request.Status = SD.Request_Booking_Accept;

		//                await _unitOfWork.RequestService.Update(request);
		//                await _unitOfWork.BookingService.Update(booking);

		//                _response.IsSuccess = true;
		//                _response.StatusCode = HttpStatusCode.OK;
		//                _response.Message = "Đồng ý lịch hẹn thành công!";
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
		//                return BadRequest();
		//            }
		//        }

		//        [Authorize(Roles = "Owner")]
		//        [HttpPut]
		//        [Route("RejectBooking")]
		//        public async Task<IActionResult> RejectBooking(int bookingId)
		//        {
		//            try
		//            {
		//                var booking = await _unitOfWork.BookingService.GetFirst(x => x.BookingId == bookingId);
		//                if (booking == null)
		//                {
		//                    _response.IsSuccess = false;
		//                    _response.ErrorMessages.Add("Không tìm thấy lịch hẹn!");
		//                    _response.StatusCode = HttpStatusCode.NotFound;
		//                    return NotFound(_response);
		//                }
		//                if (booking.Status != SD.Request_Booking_Pending)
		//                {
		//                    _response.IsSuccess = false;
		//                    _response.ErrorMessages.Add("Không thể từ chối lịch hẹn!");
		//                    _response.StatusCode = HttpStatusCode.BadRequest;
		//                    return BadRequest(_response);
		//                }
		//                booking.Status = SD.Request_Booking_Reject;
		//                var request = await _unitOfWork.RequestService.GetFirst(x => x.RequestId == booking.RequestId);
		//                if (request == null)
		//                {
		//                    _response.IsSuccess = false;
		//                    _response.ErrorMessages.Add("Không tìm thấy yêu cầu!");
		//                    _response.StatusCode = HttpStatusCode.NotFound;
		//                    return NotFound(_response);
		//                }
		//                request.Status = SD.Request_Booking_Reject;

		//                await _unitOfWork.RequestService.Update(request);
		//                await _unitOfWork.BookingService.Update(booking);

		//                _response.IsSuccess = true;
		//                _response.StatusCode = HttpStatusCode.OK;
		//                _response.Message = "Từ chối lịch hẹn thành công!";
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
		//                return BadRequest();
		//            }
		//        }

		//        [Authorize(Roles = "Store")]
		//        [HttpPut]
		//        [Route("CancelBooking")]
		//        public async Task<IActionResult> CancelBooking(int bookingId)
		//        {
		//            try
		//            {
		//                var booking = await _unitOfWork.BookingService.GetFirst(x => x.BookingId == bookingId);
		//                if (booking == null)
		//                {
		//                    _response.IsSuccess = false;
		//                    _response.ErrorMessages.Add("Không tìm thấy lịch hẹn!");
		//                    _response.StatusCode = HttpStatusCode.NotFound;
		//                    return NotFound(_response);
		//                }
		//                if (booking.Status != SD.Request_Booking_Pending)
		//                {
		//                    _response.IsSuccess = false;
		//                    _response.ErrorMessages.Add("Không thể hủy lịch hẹn!");
		//                    _response.StatusCode = HttpStatusCode.BadRequest;
		//                    return BadRequest(_response);
		//                }
		//                booking.Status = SD.Request_Booking_Cancel;
		//                var request = await _unitOfWork.RequestService.GetFirst(x => x.RequestId == booking.RequestId);
		//                if (request == null)
		//                {
		//                    _response.IsSuccess = false;
		//                    _response.ErrorMessages.Add("Không tìm thấy yêu cầu!");
		//                    _response.StatusCode = HttpStatusCode.NotFound;
		//                    return NotFound(_response);
		//                }
		//                request.Status = SD.Request_Booking_Cancel;

		//                await _unitOfWork.RequestService.Update(request);
		//                await _unitOfWork.BookingService.Update(booking);

		//                _response.IsSuccess = true;
		//                _response.StatusCode = HttpStatusCode.OK;
		//                _response.Message = "Hủy lịch hẹn thành công!";
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
		//                return BadRequest();
		//            }
		//        }
	}
}