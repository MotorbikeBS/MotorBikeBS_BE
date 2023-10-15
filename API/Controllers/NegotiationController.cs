﻿using API.DTO;
using API.DTO.BookingDTO;
using API.DTO.NegotiationDTO;
using API.DTO.UserDTO;
using API.Utility;
using AutoMapper;
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
	public class NegotiationController : ControllerBase
	{
		private readonly IUnitOfWork _unitOfWork;
		private ApiResponse _response;
		private readonly IMapper _mapper;

		public NegotiationController(IUnitOfWork unitOfWork, IMapper mapper, IEmailSender emailSender)
		{
			_unitOfWork = unitOfWork;
			_response = new ApiResponse();
			_mapper = mapper;
		}

		[Authorize(Roles = ("Owner, Store"))]
		[HttpPost]
		[Route("StartNegotitaion")]
		public async Task<IActionResult> StartNegotitaion(int motorId, NegotiationCreateDTO dto)
		{
			try
			{
				if (dto.StorePrice == null)
				{
					_response.IsSuccess = false;
					_response.StatusCode = HttpStatusCode.BadRequest;
					_response.ErrorMessages.Add("Vui lòng nhập giá mong muốn!");
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

				IEnumerable<Request> list = await _unitOfWork.RequestService.Get(x => x.SenderId == userId
				&& x.RequestTypeId == SD.Request_Negotiation_Id
				&& x.MotorId == motorId
				&& x.Status == SD.Request_Negotiation_Pending);

				if (list.Count() > 0)
				{
					_response.IsSuccess = false;
					_response.ErrorMessages.Add("Bạn đang thương lượng giá cả cho xe này!");
					_response.StatusCode = HttpStatusCode.BadRequest;
					return BadRequest(_response);
				}
				if (motor.MotorStatusId == SD.Status_Consignment || motor.MotorStatusId == SD.Status_Livelihood)
				{
					Request request = new()
					{
						MotorId = motorId,
						ReceiverId = motor.OwnerId,
						SenderId = userId,
						Time = DateTime.Now,
						RequestTypeId = SD.Request_Negotiation_Id,
						Status = SD.Request_Booking_Pending
					};
					await _unitOfWork.RequestService.Add(request);

					var negotiationCreate = _mapper.Map<Negotiation>(dto);
					negotiationCreate.RequestId = request.RequestId;
					negotiationCreate.StartTime = DateTime.Now;

					await _unitOfWork.NegotiationService.Add(negotiationCreate);

					_response.IsSuccess = true;
					_response.Message = "Trả giá thành công, vui lòng chờ người bán xác nhận!";
					_response.StatusCode = HttpStatusCode.OK;
					return Ok(_response);
				}
				else
				{
					_response.IsSuccess = false;
					_response.ErrorMessages.Add("Bạn không thể thương lượng xe này!");
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

		[Authorize(Roles = "Store, Owner")]
		[HttpGet]
		[Route("GetNegotiationRequest")]
		public async Task<IActionResult> GetNegotiationRequest()
		{
			try
			{
				var roleId = int.Parse(User.FindFirst("RoleId")?.Value);
				var userId = int.Parse(User.FindFirst("UserId")?.Value);

				IEnumerable<Request> requestNegotiation = null;

				if (roleId == SD.Role_Owner_Id)
				{
					requestNegotiation = await _unitOfWork.RequestService.Get(x => x.ReceiverId == userId
					&& x.RequestTypeId == SD.Request_Negotiation_Id, includeProperties: new string[] { "Negotiations", "Motor", "Motor.MotorStatus", "Sender.StoreDesciptions" });
				}

				if (roleId == SD.Role_Store_Id)
				{
					requestNegotiation = await _unitOfWork.RequestService.Get(x => x.SenderId == userId
					&& x.RequestTypeId == SD.Request_Negotiation_Id, includeProperties: new string[] { "Negotiations", "Motor", "Motor.MotorStatus", "Receiver" });
				}

				if (requestNegotiation == null)
				{
					_response.IsSuccess = false;
					_response.ErrorMessages.Add("Không tìm thấy yêu cầu nào!");
					_response.StatusCode = HttpStatusCode.NotFound;
					return NotFound(_response);
				}

				if (roleId == SD.Role_Store_Id)
				{
					//var response = new List<NegotiationResponseRequestDTO>();
					//foreach (var rs in requestNegotiation)
					//{
					//	var negotiationResponseRequest = _mapper.Map<NegotiationResponseRequestDTO>(rs);
					//	var owner = await _unitOfWork.UserService.GetFirst(x => x.UserId == rs.ReceiverId);
					//	var ownerResponse = _mapper.Map<UserResponseDTO>(owner);
					//	if (owner != null && negotiationResponseRequest != null)
					//	{
					//		negotiationResponseRequest.Owner = ownerResponse;
					//	}
					//	response.Add(negotiationResponseRequest);
					//}
					var response = _mapper.Map<List<NegotiationResponseRequestDTO>>(requestNegotiation);
					response.ForEach(item => item.Motor.Owner = null);
					response.ForEach(item => item.Motor.Requests = null);
					_response.IsSuccess = true;
					_response.StatusCode = HttpStatusCode.OK;
					_response.Result = response;
					return Ok(_response);
				}
				var negotiationResponse = _mapper.Map<List<NegotiationResponseRequestDTO>>(requestNegotiation);
				negotiationResponse.ForEach(item => item.Motor.Requests = null);
				_response.IsSuccess = true;
				_response.StatusCode = HttpStatusCode.OK;
				_response.Result = negotiationResponse;
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

		//[Authorize(Roles ="Owner, Store")]
		//[HttpPut("{id:int}")]
		//[Route("Bid")]
		//public async Task<IActionResult>Bid(int id)
		//{
		//	return Ok();
		//}
	}
}