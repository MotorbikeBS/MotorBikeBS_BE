using API.DTO;
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
using System.Collections.Generic;
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
		private readonly IBlobService _blobService;

		public NegotiationController(IUnitOfWork unitOfWork, IMapper mapper, IBlobService blobService)
		{
			_unitOfWork = unitOfWork;
			_response = new ApiResponse();
			_mapper = mapper;
			_blobService = blobService;
		}

		[Authorize(Roles = "Store")]
		[HttpPost]
		[Route("CreateNegotiation")]
		public async Task<IActionResult> CreateNegotiation(int valuationId, NegotiationCreateDTO dto)
		{
			try
			{
				var userId = int.Parse(User.FindFirst("UserId")?.Value);
				var rs = InputValidation.NegoBookingTimeValidation(dto.StartTime, dto.EndTime, dto.FinalPrice);
				if (!string.IsNullOrEmpty(rs))
				{
					_response.IsSuccess = false;
					_response.StatusCode = HttpStatusCode.BadRequest;
					_response.ErrorMessages.Add(rs);
					return BadRequest(_response);
				}

				var valuationInDb = await _unitOfWork.ValuationService.GetFirst(x => x.ValuationId == valuationId);
				if (valuationInDb == null)
				{
					_response.IsSuccess = false;
					_response.StatusCode = HttpStatusCode.NotFound;
					_response.ErrorMessages.Add("Không tìm thấy yêu cầu!");
					return NotFound(_response);
				}

				var request = await _unitOfWork.RequestService.GetFirst(x => x.RequestId == valuationInDb.RequestId);
				if (request == null)
				{
					_response.IsSuccess = false;
					_response.StatusCode = HttpStatusCode.NotFound;
					_response.ErrorMessages.Add("Không tìm thấy yêu cầu!");
					return NotFound(_response);
				}
				if (request.SenderId != userId)
				{
					_response.IsSuccess = false;
					_response.StatusCode = HttpStatusCode.BadRequest;
					_response.ErrorMessages.Add("Bạn không có quyền này!");
					return BadRequest(_response);
				}
				var list = await _unitOfWork.RequestService.Get(x => x.SenderId == userId
						&& x.RequestTypeId == SD.Request_Negotiation_Id
						&& x.MotorId == request.MotorId
						&& x.Status != SD.Request_Cancel
						&& x.Valuations.Any(m => m.Negotiations.Any()));

				if (list.Count() > 0)
				{
					_response.IsSuccess = false;
					_response.StatusCode = HttpStatusCode.BadRequest;
					_response.ErrorMessages.Add("Bạn đã tạo thương lượng!");
					return BadRequest(_response);
				}
				if (valuationInDb.Status == SD.Request_Cancel || request.Status == SD.Request_Cancel)
				{
					_response.IsSuccess = false;
					_response.StatusCode = HttpStatusCode.NotFound;
					_response.ErrorMessages.Add("Định giá chưa được xác nhận, chưa thể tạo thương lượng!");
					return NotFound(_response);
				}

				var motor = await _unitOfWork.MotorBikeService.GetFirst(x => x.MotorId == request.MotorId);
				if (motor == null || motor.MotorStatusId != SD.Status_Consignment && motor.MotorStatusId != SD.Status_nonConsignment)
				{
					_response.IsSuccess = false;
					_response.StatusCode = HttpStatusCode.BadRequest;
					_response.ErrorMessages.Add("Xe này đã bán hoặc đã bị xóa!");
					return BadRequest(_response);
				}
				var store = await _unitOfWork.StoreDescriptionService.GetFirst(x => x.UserId == userId);
				var newNego = _mapper.Map<Negotiation>(dto);
                newNego.StoreId = store.StoreId;
                newNego.CreatedAt = DateTime.Now;
                newNego.MotorId = motor.MotorId;
                newNego.Status = SD.Request_Pending;
                newNego.ValuationId = valuationId;
                newNego.BaseRequestId = request.RequestId;
				await _unitOfWork.NegotiationService.Add(newNego);

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

		//[Authorize(Roles = "Store")]
		//[HttpPut]
		//[Route("ReUpContract")]
		//public async Task<IActionResult> ReUpContract(int contractId, [FromForm] NegotiationCreateDTO dto, List<IFormFile> images)
		//{
		//	try
		//	{
		//		var userId = int.Parse(User.FindFirst("UserId")?.Value);
		//		var rs = InputValidation.ContractValidation(dto.Content, images);
		//		if (rs != "")
		//		{
		//			_response.IsSuccess = false;
		//			_response.StatusCode = HttpStatusCode.BadRequest;
		//			_response.ErrorMessages.Add(rs);
		//			return BadRequest(_response);
		//		}

		//		var contract = await _unitOfWork.ContractService.GetFirst(x => x.ContractId == contractId);
		//		if (contract == null)
		//		{
		//			_response.IsSuccess = false;
		//			_response.StatusCode = HttpStatusCode.NotFound;
		//			_response.ErrorMessages.Add("Không tìm thấy yêu cầu!");
		//			return NotFound(_response);
		//		}
		//		if (contract.Status != SD.Request_Cancel)
		//		{
		//			_response.IsSuccess = false;
		//			_response.StatusCode = HttpStatusCode.NotFound;
		//			_response.ErrorMessages.Add("Không thể tải lại hợp đồng!");
		//			return NotFound(_response);
		//		}

		//		var request = await _unitOfWork.RequestService.GetFirst(x => x.RequestId == contract.BaseRequestId);
		//		if (request == null)
		//		{
		//			_response.IsSuccess = false;
		//			_response.StatusCode = HttpStatusCode.NotFound;
		//			_response.ErrorMessages.Add("Không tìm thấy yêu cầu!");
		//			return NotFound(_response);
		//		}
		//		if (request.SenderId != userId)
		//		{
		//			_response.IsSuccess = false;
		//			_response.StatusCode = HttpStatusCode.BadRequest;
		//			_response.ErrorMessages.Add("Bạn không có quyền này!");
		//			return BadRequest(_response);
		//		}

		//		var motor = await _unitOfWork.MotorBikeService.GetFirst(x => x.MotorId == request.MotorId);
		//		if (motor == null || motor.MotorStatusId != SD.Status_Consignment && motor.MotorStatusId != SD.Status_nonConsignment)
		//		{
		//			_response.IsSuccess = false;
		//			_response.StatusCode = HttpStatusCode.BadRequest;
		//			_response.ErrorMessages.Add("Xe này đã bán hoặc đã bị xóa!");
		//			return BadRequest(_response);
		//		}
		//		var store = await _unitOfWork.StoreDescriptionService.GetFirst(x => x.UserId == userId);
		//		var oldImage = await _unitOfWork.ContractImageService.Get(x => x.ContractId == contractId);

		//		if (oldImage.Count() > 0)
		//		{
		//			foreach (var item in oldImage)
		//			{
		//				var oldLisenceImg = item.ImageLink.Split('/').Last();
		//				await _blobService.DeleteBlob(oldLisenceImg, SD.Storage_Container);
		//				await _unitOfWork.ContractImageService.Delete(item);
		//			}
		//		}

		//		contract.Status = SD.Request_Pending;
		//		contract.Content = dto.Content;
		//		await _unitOfWork.ContractService.Update(contract);

		//		foreach (var item in images)
		//		{
		//			string file = $"{Guid.NewGuid()}{Path.GetExtension(item.FileName)}";
		//			var img = await _blobService.UploadBlob(file, SD.Storage_Container, item);
		//			ContractImage image = new()
		//			{
		//				ContractId = contract.ContractId,
		//				ImageLink = img
		//			};
		//			await _unitOfWork.ContractImageService.Add(image);
		//		}
		//		_response.IsSuccess = true;
		//		_response.Message = "Tải lại hợp đồng thành công, vui lòng chờ người bán xác nhận!";
		//		_response.StatusCode = HttpStatusCode.OK;
		//		return Ok(_response);
		//	}
		//	catch (Exception ex)
		//	{
		//		_response.IsSuccess = false;
		//		_response.StatusCode = HttpStatusCode.BadRequest;
		//		_response.ErrorMessages = new List<string>()
		//				{
		//					ex.ToString()
		//				};
		//		return BadRequest();
		//	}
		//}

		[Authorize(Roles = ("Store, Owner"))]
		[HttpGet]
		[Route("GetNegotiation")]
		public async Task<IActionResult> GetNegotiation()
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
						&& x.Status != SD.Request_Cancel
						&& x.Valuations.Any(m => m.Negotiations.Any()),
						includeProperties: new string[]
						{ "Valuations", "Motor", "Motor.MotorStatus", "Motor.MotorbikeImages",
					  "Receiver", "Valuations.Negotiations"});
				}
				else
				{
					list = await _unitOfWork.RequestService.Get(x => x.ReceiverId == userId
						&& x.RequestTypeId == SD.Request_Negotiation_Id
						&& x.Status != SD.Request_Cancel
						&& x.Valuations.Any(m => m.Negotiations.Any(s => s.Status == SD.Request_Pending
						|| s.Status == SD.Request_Accept)),
						includeProperties: new string[]
						{ "Valuations", "Motor", "Motor.MotorStatus", "Motor.MotorbikeImages",
					  "Sender", "Sender.StoreDesciptions", "Valuations.Negotiations"});
				}
				if (list.Count() > 0)
				{
					var response = _mapper.Map<List<RequestNegotiationResponseDTO>>(list);

					response.ForEach(item => item.Motor.Owner = null);
					_response.IsSuccess = true;
					_response.StatusCode = HttpStatusCode.OK;
					_response.Result = response;
					return Ok(_response);
				}
				_response.IsSuccess = false;
				_response.StatusCode = HttpStatusCode.NotFound;
				_response.ErrorMessages.Add("Không có kết quả thương lượng nào đang chờ!");
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

		[Authorize(Roles = ("Owner, Store"))]
		[HttpPut]
		[Route("CancelNegotiation")]
		public async Task<IActionResult> CancelNegotiation(int negotiationId)
		{
			try
			{
				var userId = int.Parse(User.FindFirst("UserId")?.Value);
                var roleId = int.Parse(User.FindFirst("RoleId")?.Value);
                var nego = await _unitOfWork.NegotiationService.GetFirst(x => x.NegotiationId == negotiationId);
				if (nego == null || nego.Status != SD.Request_Pending || nego.StoreId == null)
				{
					_response.IsSuccess = false;
					_response.StatusCode = HttpStatusCode.NotFound;
					_response.ErrorMessages.Add("Không tìm thấy kết quả thương lượng nào!");
					return NotFound(_response);
				}
				var request = await _unitOfWork.RequestService.GetFirst(x => x.RequestId == nego.BaseRequestId);
				if (request == null || request.Status != SD.Request_Pending)
				{
					_response.IsSuccess = false;
					_response.StatusCode = HttpStatusCode.NotFound;
					_response.ErrorMessages.Add("Không tìm thấy yêu cầu!");
					return NotFound(_response);
				}
                if (roleId == SD.Role_Owner_Id)
                {
                    if (request.ReceiverId != userId)
                    {
                        _response.IsSuccess = false;
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.ErrorMessages.Add("Bạn không có quyền này!");
                        return BadRequest(_response);
                    }
                }
                if (roleId == SD.Role_Store_Id)
                {
                    if (request.SenderId != userId)
                    {
                        _response.IsSuccess = false;
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.ErrorMessages.Add("Bạn không có quyền này!");
                        return BadRequest(_response);
                    }
                }
                //IEnumerable<ContractImage> img = await _unitOfWork.ContractImageService.Get(x => x.ContractId == contractId);
                //if(img.Count() > 0)
                //{
                //	foreach (var item in img)
                //	{
                //		var oldLisenceImg = item.ImageLink.Split('/').Last();
                //		await _blobService.DeleteBlob(oldLisenceImg, SD.Storage_Container);
                //		await _unitOfWork.ContractImageService.Delete(item);
                //	}
                //}
                //await _unitOfWork.ContractService.Delete(contract);
                nego.Status = SD.Request_Cancel;
				await _unitOfWork.NegotiationService.Update(nego);
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

		[Authorize(Roles = ("Owner"))]
		[HttpPut]
		[Route("AcceptNegotation")]
		public async Task<IActionResult> AcceptNegotation(int negotiationId)
		{
			try
			{
				var userId = int.Parse(User.FindFirst("UserId")?.Value);
				var nego = await _unitOfWork.NegotiationService.GetFirst(x => x.NegotiationId == negotiationId);
				if (nego == null || nego.Status != SD.Request_Pending || nego.StoreId == null)
				{
					_response.IsSuccess = false;
					_response.StatusCode = HttpStatusCode.NotFound;
					_response.ErrorMessages.Add("Không tìm thấy yêu cầu thương lượng nào!");
					return NotFound(_response);
				}
				var request = await _unitOfWork.RequestService.GetFirst(x => x.RequestId == nego.BaseRequestId);
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
                //if(negotiation == null || negotiation.FinalPrice == null)
                //{
                //	_response.IsSuccess = false;
                //	_response.StatusCode = HttpStatusCode.NotFound;
                //	_response.ErrorMessages.Add("Không tìm thấy yêu cầu!");
                //	return NotFound(_response);
                //}

                nego.Status = SD.Request_Accept;
				await _unitOfWork.NegotiationService.Update(nego);
				motor.MotorStatusId = SD.Status_Storage;
				motor.StoreId = nego.StoreId;
				motor.Price = negotiation.FinalPrice;
				await _unitOfWork.MotorBikeService.Update(motor);
				request.Status = SD.Request_Accept;
				await _unitOfWork.RequestService.Update(request);

				IEnumerable<Request> requestList = await _unitOfWork.RequestService.Get(x => x.MotorId == motor.MotorId
				&& x.SenderId != userId
				&& x.Status == SD.Request_Pending
				&& x.RequestTypeId == SD.Request_Negotiation_Id);
				if (requestList.Count() > 0)
				{
					foreach (var item in requestList)
					{
						item.Status = SD.Request_Cancel;
						await _unitOfWork.RequestService.Update(item);
						var negoCancel = await _unitOfWork.NegotiationService.GetFirst(x => x.BaseRequestId == item.RequestId);
						if (negoCancel != null)
						{
                            negoCancel.Status = SD.Request_Cancel;
							await _unitOfWork.NegotiationService.Update(negoCancel);
						}
					}
				}
				_response.IsSuccess = true;
				_response.StatusCode = HttpStatusCode.OK;
				_response.Message = "Đồng ý thành công";
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

		//[Authorize(Roles = ("Owner))]
		//[HttpPut]
		//[Route("RejectNegotiation")]
		//public async Task<IActionResult> RejectNegotiation(int contractId)
		//{
		//	try
		//	{
		//		var userId = int.Parse(User.FindFirst("UserId")?.Value);
		//		var roleId = int.Parse(User.FindFirst("RoleId")?.Value);
		//		var contract = await _unitOfWork.ContractService.GetFirst(x => x.ContractId == contractId);
		//		if (contract == null || contract.Status != SD.Request_Pending || contract.StoreId == null)
		//		{
		//			_response.IsSuccess = false;
		//			_response.StatusCode = HttpStatusCode.NotFound;
		//			_response.ErrorMessages.Add("Không tìm thấy hợp đồng!");
		//			return NotFound(_response);
		//		}
		//		var request = await _unitOfWork.RequestService.GetFirst(x => x.RequestId == contract.BaseRequestId);
		//		if (request == null || request.Status != SD.Request_Pending)
		//		{
		//			_response.IsSuccess = false;
		//			_response.StatusCode = HttpStatusCode.NotFound;
		//			_response.ErrorMessages.Add("Không tìm thấy yêu cầu!");
		//			return NotFound(_response);
		//		}
		//		if (roleId == SD.Role_Owner_Id)
		//		{
		//			if (request.ReceiverId != userId)
		//			{
		//				_response.IsSuccess = false;
		//				_response.StatusCode = HttpStatusCode.BadRequest;
		//				_response.ErrorMessages.Add("Bạn không có quyền này!");
		//				return BadRequest(_response);
		//			}
		//		}
		//		if (roleId == SD.Role_Store_Id)
		//		{
		//			if (request.SenderId != userId)
		//			{
		//				_response.IsSuccess = false;
		//				_response.StatusCode = HttpStatusCode.BadRequest;
		//				_response.ErrorMessages.Add("Bạn không có quyền này!");
		//				return BadRequest(_response);
		//			}
		//		}

		//		IEnumerable<ContractImage> img = await _unitOfWork.ContractImageService.Get(x => x.ContractId == contractId);
		//		if (img.Count() > 0)
		//		{
		//			foreach (var item in img)
		//			{
		//				var oldLisenceImg = item.ImageLink.Split('/').Last();
		//				await _blobService.DeleteBlob(oldLisenceImg, SD.Storage_Container);
		//				await _unitOfWork.ContractImageService.Delete(item);
		//			}
		//		}

		//		contract.Status = SD.Request_Reject;
		//		await _unitOfWork.ContractService.Update(contract);

		//		request.Status = SD.Request_Cancel;
		//		await _unitOfWork.RequestService.Update(request);

		//		_response.IsSuccess = true;
		//		_response.StatusCode = HttpStatusCode.OK;
		//		_response.Message = "Từ chối thành công!";
		//		return Ok(_response);
		//	}
		//	catch (Exception ex)
		//	{
		//		_response.IsSuccess = false;
		//		_response.StatusCode = HttpStatusCode.BadRequest;
		//		_response.ErrorMessages = new List<string>()
		//		{
		//			ex.ToString()
		//		};
		//		return BadRequest();
		//	}
		//}
	}
}
