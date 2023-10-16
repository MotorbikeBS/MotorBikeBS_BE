using API.DTO;
using API.DTO.BookingDTO;
using API.DTO.NegotiationDTO;
using API.DTO.UserDTO;
using API.Utility;
using AutoMapper;
using Azure.Core;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Service;
using Service.UnitOfWork;
using System.Linq;
using System.Net;
using Request = Core.Models.Request;

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

		[Authorize(Roles = ("Store"))]
		[HttpPost]
		[Route("StartNegotitaion")]
		public async Task<IActionResult> StartNegotiation(int motorId, NegotiationCreateDTO dto)
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
				if (dto.StorePrice < 1000000 || dto.StorePrice > 1000000000)
				{
					_response.IsSuccess = false;
					_response.StatusCode = HttpStatusCode.BadRequest;
					_response.ErrorMessages.Add("Vui lòng nhập giá hợp lệ, thấp nhất là 1.000.000VNĐ!");
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
				&& x.Status == SD.Request_Pending);

				if (list.Count() > 0)
				{
					_response.IsSuccess = false;
					_response.ErrorMessages.Add("Bạn đang trong quá trình làm việc cho xe này!");
					_response.StatusCode = HttpStatusCode.BadRequest;
					return BadRequest(_response);
				}
				if (motor.MotorStatusId == SD.Status_Consignment || motor.MotorStatusId == SD.Status_nonConsignment)
				{
					Request request = new()
					{
						MotorId = motorId,
						ReceiverId = motor.OwnerId,
						SenderId = userId,
						Time = DateTime.Now,
						RequestTypeId = SD.Request_Negotiation_Id,
						Status = SD.Request_Pending
					};
					await _unitOfWork.RequestService.Add(request);

					var negotiationCreate = _mapper.Map<Negotiation>(dto);
					negotiationCreate.RequestId = request.RequestId;
					negotiationCreate.StartTime = DateTime.Now;
					negotiationCreate.Status = SD.Request_Pending;
					negotiationCreate.BaseRequestId = request.RequestId;

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
					&& x.RequestTypeId == SD.Request_Negotiation_Id
					&& x.Status == SD.Request_Pending,
					includeProperties: new string[] { "Negotiations", "Motor", "Motor.MotorStatus", "Sender.StoreDesciptions" });
				}

				if (roleId == SD.Role_Store_Id)
				{
					requestNegotiation = await _unitOfWork.RequestService.Get(x => x.SenderId == userId
					&& x.RequestTypeId == SD.Request_Negotiation_Id
					&& x.Status == SD.Request_Pending,
					includeProperties: new string[] { "Negotiations", "Motor", "Motor.MotorStatus", "Receiver" });
				}
				var negotiationResponse = _mapper.Map<List<NegotiationResponseRequestDTO>>(requestNegotiation);

				//negotiationResponse.ForEach(item =>
				//{
				//	item.Negotiations = item.Negotiations.Where(n => n.EndTime == null).ToList();
				//});

				//if (negotiationResponse.Any(item => item.Negotiations == null || item.Negotiations.Count == 0))
				//{
				//	_response.IsSuccess = false;
				//	_response.ErrorMessages.Add("Hiện tại không có xe nào đang thương lượng!");
				//	_response.StatusCode = HttpStatusCode.NotFound;
				//	return NotFound(_response);


				if (negotiationResponse.Count() < 1)
				{
					_response.IsSuccess = false;
					_response.ErrorMessages.Add("Hiện tại không có xe nào đang thương lượng!");
					_response.StatusCode = HttpStatusCode.NotFound;
					return NotFound(_response);
				}

				if (roleId == SD.Role_Store_Id)
				{
					negotiationResponse.ForEach(item => item.Motor.Owner = null);
				}
				negotiationResponse.ForEach(item => item.Motor.Requests = null);
				negotiationResponse.ForEach(item => item.Motor.MotorStatus.Motorbikes = null);


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
				return BadRequest(_response);
			}
		}

		[Authorize(Roles = "Owner, Store")]
		[HttpPut]
		[Route("Bid")]
		public async Task<IActionResult> Bid(int NegotiationId, NegotiationUpdateDTO dto)
		{
			try
			{


				var userId = int.Parse(User.FindFirst("UserId")?.Value);
				var roleId = int.Parse(User.FindFirst("RoleId")?.Value);
				var negotiationInDb = await _unitOfWork.NegotiationService.GetFirst(x => x.NegotiationId == NegotiationId);
				if (negotiationInDb == null)
				{
					_response.IsSuccess = false;
					_response.ErrorMessages.Add("Không tìm thấy yêu cầu này!");
					_response.StatusCode = HttpStatusCode.NotFound;
					return NotFound(_response);
				}
				var baseRequest = await _unitOfWork.RequestService.GetFirst(x => x.RequestId == negotiationInDb.BaseRequestId);
				if(negotiationInDb.Status != SD.Request_Pending || baseRequest.Status != SD.Request_Pending)
				{
					_response.IsSuccess = false;
					_response.ErrorMessages.Add("Không thể thương lượng, quá trình này đã kết thúc!");
					_response.StatusCode = HttpStatusCode.BadRequest;
					return BadRequest(_response);
				}
				
				if (dto.Price == null)
				{
					_response.IsSuccess = false;
					_response.StatusCode = HttpStatusCode.BadRequest;
					_response.ErrorMessages.Add("Vui lòng nhập giá mong muốn!");
					return BadRequest(_response);
				}
				if (dto.Price < 1000000 || dto.Price > 1000000000)
				{
					_response.IsSuccess = false;
					_response.StatusCode = HttpStatusCode.BadRequest;
					_response.ErrorMessages.Add("Vui lòng nhập giá hợp lệ, thấp nhất là 1.000.000VNĐ!");
					return BadRequest(_response);
				}

				var request = await _unitOfWork.RequestService.GetFirst(x => x.RequestId == negotiationInDb.RequestId && x.RequestTypeId == SD.Request_Negotiation_Id);
				if (roleId == SD.Role_Owner_Id)
				{
					if (userId != request.ReceiverId)
					{
						_response.IsSuccess = false;
						_response.ErrorMessages.Add("Bạn không có quyền này!");
						_response.StatusCode = HttpStatusCode.BadRequest;
						return BadRequest(_response);
					}
					negotiationInDb.OwnerPrice = dto.Price;
					negotiationInDb.Description = dto.Description;
				}
				else if (roleId == SD.Role_Store_Id)
				{
					if (userId != request.SenderId)
					{
						_response.IsSuccess = false;
						_response.ErrorMessages.Add("Bạn không có quyền này!");
						_response.StatusCode = HttpStatusCode.BadRequest;
						return BadRequest(_response);
					}
					negotiationInDb.StorePrice = dto.Price;
					negotiationInDb.Description = dto.Description;
				}
				await _unitOfWork.NegotiationService.Update(negotiationInDb);
				_response.IsSuccess = true;
				_response.StatusCode = HttpStatusCode.OK;
				_response.Message = ("Trả giá thành công");
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

		[Authorize(Roles = "Owner, Store")]
		[HttpPut]
		[Route("Accept")]
		public async Task<IActionResult> Accept(int NegotiationId)
		{
			try
			{
				var userId = int.Parse(User.FindFirst("UserId")?.Value);
				var roleId = int.Parse(User.FindFirst("RoleId")?.Value);
				var negotiationInDb = await _unitOfWork.NegotiationService.GetFirst(x => x.NegotiationId == NegotiationId);
				if (negotiationInDb == null)
				{
					_response.IsSuccess = false;
					_response.ErrorMessages.Add("Không tìm thấy yêu cầu này!");
					_response.StatusCode = HttpStatusCode.NotFound;
					return NotFound(_response);
				}
				var baseRequest = await _unitOfWork.RequestService.GetFirst();
				if (negotiationInDb.Status != SD.Request_Pending || baseRequest.Status != SD.Request_Pending)
				{
					_response.IsSuccess = false;
					_response.ErrorMessages.Add("Không thể đồng ý yêu cầu, quá trình này đã kết thúc!");
					_response.StatusCode = HttpStatusCode.BadRequest;
					return BadRequest(_response);
				}
				if (roleId == SD.Role_Store_Id)
				{
					if (userId != baseRequest.SenderId)
					{
						_response.IsSuccess = false;
						_response.ErrorMessages.Add("Bạn không có quyền này!");
						_response.StatusCode = HttpStatusCode.BadRequest;
						return BadRequest(_response);
					}
					if (negotiationInDb.OwnerPrice == null)
					{
						_response.IsSuccess = false;
						_response.ErrorMessages.Add("Chủ xe chưa ra giá, bạn không thể đồng ý!");
						_response.StatusCode = HttpStatusCode.BadRequest;
						return BadRequest(_response);
					}
					negotiationInDb.FinalPrice = negotiationInDb.OwnerPrice;
				}
				if (roleId == SD.Role_Owner_Id)
				{
					if (userId != baseRequest.ReceiverId)
					{
						_response.IsSuccess = false;
						_response.ErrorMessages.Add("Bạn không có quyền này!");
						_response.StatusCode = HttpStatusCode.BadRequest;
						return BadRequest(_response);
					}
					negotiationInDb.FinalPrice = negotiationInDb.StorePrice;
				}
				negotiationInDb.Status = SD.Request_Accept;
				await _unitOfWork.NegotiationService.Update(negotiationInDb);
				_response.IsSuccess = true;
				_response.StatusCode = HttpStatusCode.OK;
				_response.Message = ("Bạn đã đồng ý thương lượng thành công!");
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

		[Authorize(Roles = "Owner, Store")]
		[HttpPut]
		[Route("Cancel")]
		public async Task<IActionResult> Cancel(int NegotiationId)
		{
			try
			{
				var userId = int.Parse(User.FindFirst("UserId")?.Value);
				var roleId = int.Parse(User.FindFirst("RoleId")?.Value);
				var negotiationInDb = await _unitOfWork.NegotiationService.GetFirst(x => x.NegotiationId == NegotiationId);
				if (negotiationInDb == null)
				{
					_response.IsSuccess = false;
					_response.ErrorMessages.Add("Không tìm thấy yêu cầu này!");
					_response.StatusCode = HttpStatusCode.NotFound;
					return NotFound(_response);
				}
				var baseRequest = await _unitOfWork.RequestService.GetFirst();
				if (negotiationInDb.Status != SD.Request_Pending || baseRequest.Status != SD.Request_Pending)
				{
					_response.IsSuccess = false;
					_response.ErrorMessages.Add("Không thể đồng ý yêu cầu, quá trình này đã kết thúc!");
					_response.StatusCode = HttpStatusCode.BadRequest;
					return BadRequest(_response);
				}
				if (roleId == SD.Role_Store_Id)
				{
					if (userId != baseRequest.SenderId)
					{
						_response.IsSuccess = false;
						_response.ErrorMessages.Add("Bạn không có quyền này!");
						_response.StatusCode = HttpStatusCode.BadRequest;
						return BadRequest(_response);
					}
					negotiationInDb.FinalPrice = negotiationInDb.OwnerPrice;
				}
				if (roleId == SD.Role_Owner_Id)
				{
					if (userId != baseRequest.ReceiverId)
					{
						_response.IsSuccess = false;
						_response.ErrorMessages.Add("Bạn không có quyền này!");
						_response.StatusCode = HttpStatusCode.BadRequest;
						return BadRequest(_response);
					}
					negotiationInDb.FinalPrice = negotiationInDb.OwnerPrice;
				}

				negotiationInDb.Status = SD.Request_Cancel;
				await _unitOfWork.NegotiationService.Update(negotiationInDb);
				
				baseRequest.Status = SD.Request_Cancel;
				await _unitOfWork.RequestService.Update(baseRequest);

				_response.IsSuccess = true;
				_response.StatusCode = HttpStatusCode.OK;
				_response.Message = ("Bạn đã hủy yêu cầu thành công!");
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
