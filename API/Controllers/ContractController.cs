using API.DTO;
using API.DTO.ContractDTO;
using API.Utility;
using API.Validation;
using AutoMapper;
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

				var requestDuplicate = await _unitOfWork.ContractService.GetFirst(x => x.BookingId == bookingId
				&& x.Status == SD.Request_Pending);
				if(requestDuplicate != null)
				{
					_response.IsSuccess = false;
					_response.ErrorMessages.Add("Bạn đã tạo hợp đồng!");
					_response.StatusCode = HttpStatusCode.BadRequest;
					return BadRequest(_response);
				}
				
				var request = await _unitOfWork.RequestService.GetFirst(x => x.RequestId == booking.BaseRequestId);
				if (request == null)
				{
					_response.IsSuccess = false;
					_response.StatusCode = HttpStatusCode.NotFound;
					_response.ErrorMessages.Add("Không tìm thấy yêu cầu!");
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
	}
}
