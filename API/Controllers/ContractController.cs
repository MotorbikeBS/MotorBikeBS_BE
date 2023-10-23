using API.DTO;
using API.DTO.BookingNegotiationDTO;
using API.DTO.ContractDTO;
using API.Utility;
using API.Validation;
using AutoMapper;
using Azure;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.BlobImageService;
using Service.Service;
using Service.UnitOfWork;
using System.Net;

namespace API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ContractController : ControllerBase
	{
		private readonly IUnitOfWork _unitOfWork;
		private ApiResponse _response;
		private readonly IMapper _mapper;
		private readonly IBlobService _blobService;

		public ContractController(IUnitOfWork unitOfWork, IMapper mapper, IBlobService blobService)
		{
			_unitOfWork = unitOfWork;
			_response = new ApiResponse();
			_mapper = mapper;
			_blobService = blobService;
		}

		[Authorize(Roles ="Store")]
		[HttpPost]
		[Route("CreateContract")]
		public async Task<IActionResult> CreateContract(int bookingId,[FromForm] ContractCreateDTO dto, List<IFormFile> images)
		{
			try
			{
				var userId = int.Parse(User.FindFirst("UserId")?.Value);
				var rs = InputValidation.ContractValidation(dto.Content, images);
				if(rs != "")
				{
					_response.IsSuccess = false;
					_response.StatusCode = HttpStatusCode.BadRequest;
					_response.ErrorMessages.Add(rs);
					return BadRequest(_response);
				}

				var booking = await _unitOfWork.BookingService.GetFirst(x => x.BookingId == bookingId);
				if (booking == null)
				{
					_response.IsSuccess = false;
					_response.StatusCode = HttpStatusCode.NotFound;
					_response.ErrorMessages.Add("Không tìm thấy yêu cầu!");
					return NotFound(_response);
				}
				var negotiation = await _unitOfWork.NegotiationService.GetFirst(x => x.NegotiationId == booking.NegotiationId);
				if (booking == null)
				{
					_response.IsSuccess = false;
					_response.StatusCode = HttpStatusCode.NotFound;
					_response.ErrorMessages.Add("Không tìm thấy yêu cầu!");
					return NotFound(_response);
				}
				
				var request = await _unitOfWork.RequestService.GetFirst(x => x.RequestId == booking.BaseRequestId);
				if (request == null)
				{
					_response.IsSuccess = false;
					_response.StatusCode = HttpStatusCode.NotFound;
					_response.ErrorMessages.Add("Không tìm thấy yêu cầu!");
					return NotFound(_response);
				}
				var contract = await _unitOfWork.ContractService.GetFirst(x => x.BaseRequestId == request.RequestId);
				if(contract != null && contract.Status == SD.Request_Pending)
				{
					_response.IsSuccess = false;
					_response.StatusCode = HttpStatusCode.NotFound;
					_response.ErrorMessages.Add("Bạn đã tạo hợp đồng!");
					return NotFound(_response);
				}
				if (booking.Status == SD.Request_Cancel || request.Status == SD.Request_Cancel)
				{
					_response.IsSuccess = false;
					_response.StatusCode = HttpStatusCode.NotFound;
					_response.ErrorMessages.Add("Bạn không thể tạo hợp đồng với xe này!");
					return NotFound(_response);
				}
				if (request.SenderId != userId)
				{
					_response.IsSuccess = false;
					_response.StatusCode = HttpStatusCode.BadRequest;
					_response.ErrorMessages.Add("Bạn không có quyền này!");
					return BadRequest(_response);
				}
				var motor = await _unitOfWork.MotorBikeService.GetFirst(x => x.MotorId == request.MotorId);
				if(motor == null || motor.MotorStatusId != SD.Status_Consignment && motor.MotorStatusId != SD.Status_nonConsignment)
				{
					_response.IsSuccess = false;
					_response.StatusCode = HttpStatusCode.BadRequest;
					_response.ErrorMessages.Add("Xe này đã bán hoặc đã bị xóa!");
					return BadRequest(_response);
				}
				
				var newContract = _mapper.Map<Contract>(dto);
				newContract.StoreId = userId;
				newContract.CreatedAt = DateTime.Now;
				newContract.BookingId = bookingId;
				newContract.BaseRequestId = booking.BaseRequestId;
				newContract.MotorId = request.MotorId;
				newContract.Status = SD.Request_Pending;
				newContract.Price = negotiation.FinalPrice;
				newContract.NewOwner = userId;
				await _unitOfWork.ContractService.Add(newContract);
				foreach(var item in images)
				{
					string file = $"{Guid.NewGuid()}{Path.GetExtension(item.FileName)}";
					var img = await _blobService.UploadBlob(file, SD.Storage_Container, item);
					ContractImage image = new()
					{
						ContractId = newContract.ContractId,
						ImageLink = img
					};
					await _unitOfWork.ContractImageService.Add(image);
				}
				_response.IsSuccess = true;
				_response.Message = "Tạo hợp đồng thành công, vui lòng chờ người bán xác nhận!";
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

		[Authorize(Roles =("Store, Owner"))]
		[HttpGet]
		[Route("GetContract")]
		public async Task<IActionResult>GetContract()
		{
			try
			{
				var userId = int.Parse(User.FindFirst("UserId")?.Value);
				var roleId = int.Parse(User.FindFirst("RoleId")?.Value);
				IEnumerable<Request> list;
				if (roleId == SD.Role_Store_Id)
				{
					list = await _unitOfWork.RequestService.Get(x => x.SenderId == userId
						&& x.RequestTypeId == SD.Request_Negotiation_Id
						&& x.Status == SD.Request_Pending
						&& x.Negotiations.Any(n => n.Bookings.Any(m => m.Contracts.Any(s => s.Status == SD.Request_Pending))),
						includeProperties: new string[]
						{ "Negotiations", "Motor", "Motor.MotorStatus", "Motor.MotorbikeImages", "Negotiations.Bookings",
					  "Receiver", "Negotiations.Bookings.Contracts", "Negotiations.Bookings.Contracts.ContractImages" });
				}
				else
				{
					list = await _unitOfWork.RequestService.Get(x => x.ReceiverId == userId
						&& x.RequestTypeId == SD.Request_Negotiation_Id
						&& x.Status == SD.Request_Pending
						&& x.Negotiations.Any(n => n.Bookings.Any(m => m.Contracts.Any(s => s.Status == SD.Request_Pending))),
						includeProperties: new string[]
						{ "Negotiations", "Motor", "Motor.MotorStatus", "Motor.MotorbikeImages", "Negotiations.Bookings",
					  "Receiver", "Negotiations.Bookings.Contracts", "Negotiations.Bookings.Contracts.ContractImages" });
				}
				if (list.Count() > 0)
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
				_response.ErrorMessages.Add("Không có hợp đồng nào đang chờ!");
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

		[Authorize(Roles = ("Owner"))]
		[HttpPut]
		[Route("CancelContract")]
		public async Task<IActionResult> CancelContract(int contractId)
		{
			try
			{
				var userId = int.Parse(User.FindFirst("UserId")?.Value);
				var contract = await _unitOfWork.ContractService.GetFirst(x => x.ContractId == contractId);
				if (contract == null || contract.Status != SD.Request_Pending || contract.StoreId == null)
				{
					_response.IsSuccess = false;
					_response.StatusCode = HttpStatusCode.NotFound;
					_response.ErrorMessages.Add("Không tìm thấy hợp đồng!");
					return NotFound(_response);
				}
				var request = await _unitOfWork.RequestService.GetFirst(x => x.RequestId == contract.BaseRequestId);
				if (request == null || request.Status != SD.Request_Pending)
				{
					_response.IsSuccess = false;
					_response.StatusCode = HttpStatusCode.NotFound;
					_response.ErrorMessages.Add("Không tìm thấy yêu cầu!");
					return NotFound(_response);
				}
				if (request.ReceiverId != userId)
				{
					_response.IsSuccess = false;
					_response.StatusCode = HttpStatusCode.BadRequest;
					_response.ErrorMessages.Add("Bạn không có quyền này!");
					return BadRequest(_response);
				}
				IEnumerable<ContractImage> img = await _unitOfWork.ContractImageService.Get(x => x.ContractId == contractId);
				if(img.Count() > 0)
				{
					foreach (var item in img)
					{
						var oldLisenceImg = item.ImageLink.Split('/').Last();
						await _blobService.DeleteBlob(oldLisenceImg, SD.Storage_Container);
						await _unitOfWork.ContractImageService.Delete(item);
					}
				}
				await _unitOfWork.ContractService.Delete(contract);
				_response.IsSuccess = true;
				_response.StatusCode = HttpStatusCode.OK;
				_response.Message = "Từ chối thành công!";
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

		[Authorize(Roles =("Owner"))]
		[HttpPut]
		[Route("AcceptContract")]
		public async Task<IActionResult>AcceptContract(int contractId)
		{
			try
			{
				var userId = int.Parse(User.FindFirst("UserId")?.Value);
				var contract = await _unitOfWork.ContractService.GetFirst(x => x.ContractId == contractId);
				if (contract == null || contract.Status != SD.Request_Pending || contract.StoreId == null)
				{
					_response.IsSuccess = false;
					_response.StatusCode = HttpStatusCode.NotFound;
					_response.ErrorMessages.Add("Không tìm thấy hợp đồng!");
					return NotFound(_response);
				}
				var request = await _unitOfWork.RequestService.GetFirst(x => x.RequestId == contract.BaseRequestId);
				if (request == null || request.Status != SD.Request_Pending)
				{
					_response.IsSuccess = false;
					_response.StatusCode = HttpStatusCode.NotFound;
					_response.ErrorMessages.Add("Không tìm thấy yêu cầu!");
					return NotFound(_response);
				}
				if (request.ReceiverId != userId)
				{
					_response.IsSuccess = false;
					_response.StatusCode = HttpStatusCode.BadRequest;
					_response.ErrorMessages.Add("Bạn không có quyền này!");
					return BadRequest(_response);
				}
				var motor = await _unitOfWork.MotorBikeService.GetFirst(x => x.MotorId == request.MotorId);
				if (motor == null || motor.MotorStatusId != SD.Status_Consignment && motor.MotorStatusId != SD.Status_nonConsignment)
				{
					_response.IsSuccess = false;
					_response.StatusCode = HttpStatusCode.BadRequest;
					_response.ErrorMessages.Add("Xe này đã bán hoặc đã bị xóa!");
					return BadRequest(_response);
				}
				var negotiation = await _unitOfWork.NegotiationService.GetFirst(x => x.BaseRequestId == request.RequestId);
				if(negotiation == null || negotiation.FinalPrice == null)
				{
					_response.IsSuccess = false;
					_response.StatusCode = HttpStatusCode.NotFound;
					_response.ErrorMessages.Add("Không tìm thấy yêu cầu!");
					return NotFound(_response);
				}
				contract.Status = SD.Request_Accept;
				await _unitOfWork.ContractService.Update(contract);
				motor.MotorStatusId = SD.Status_Storage;
				motor.StoreId = (int)contract.StoreId;
				motor.Price = negotiation.FinalPrice;
				await _unitOfWork.MotorBikeService.Update(motor);
				request.Status = SD.Request_Accept;
				await _unitOfWork.RequestService.Update(request);

				IEnumerable<Request> requestList = await _unitOfWork.RequestService.Get(x => x.MotorId == motor.MotorId
				&& x.Status == SD.Request_Pending
				&& x.RequestTypeId == SD.Request_Negotiation_Id);
				if(requestList.Count() > 0)
				{
					foreach(var item in requestList)
					{
						item.Status = SD.Request_Cancel;
						await _unitOfWork.RequestService.Update(item);
					}
				}
				_response.IsSuccess = true;
				_response.StatusCode = HttpStatusCode.OK;
				_response.Message = "Đồng ý thành công, bạn đã hoàn tất mua bán";
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
