using API.DTO;
using API.DTO.ContractDTO;
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
	public class ContractController : ControllerBase
	{
		private readonly IUnitOfWork _unitOfWork;
		private ApiResponse _response;
		private readonly IMapper _mapper;

		public ContractController(IUnitOfWork unitOfWork, IMapper mapper, IEmailSender emailSender)
		{
			_unitOfWork = unitOfWork;
			_response = new ApiResponse();
			_mapper = mapper;
		}

		[Authorize(Roles ="Store")]
		[HttpPost]
		[Route("CreateContract")]
		public async Task<IActionResult> CreateContract(int bookingId, ContractCreateDTO dto, List<IFormFile> images)
		{
			var userId = int.Parse(User.FindFirst("UserId")?.Value);
			if(dto.Content == null || images.Count() > 1)
			{
				_response.IsSuccess = false;
				_response.StatusCode = HttpStatusCode.BadRequest;
				_response.ErrorMessages.Add("Không tìm thấy yêu cầu!");
				return BadRequest(_response);
			}
			var booking = await _unitOfWork.BookingService.GetFirst(x => x.BookingId == bookingId);
			if(booking == null)
			{
				_response.IsSuccess = false;
				_response.StatusCode = HttpStatusCode.NotFound;
				_response.ErrorMessages.Add("Không tìm thấy yêu cầu!");
				return NotFound(_response);
			}
			var request = await _unitOfWork.RequestService.GetFirst(x => x.RequestId == booking.BaseRequestId);
			if(request == null)
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
			var newContract = _mapper.Map<Contract>(dto);

			return Ok();
		}
	}
}
