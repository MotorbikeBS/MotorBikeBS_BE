using API.DTO;
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
	public class MotorBikeController : ControllerBase
	{
		private readonly IUnitOfWork _unitOfWork;
		private ApiResponse _response;

		public MotorBikeController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
			_response = new ApiResponse();
		}
		//[Authorize(Roles ="Admin")]
		[HttpGet]
		public async Task<ApiResponse>Get()
		{
			try
			{
				var list = await _unitOfWork.MotorBikeService.Get();
				if (list == null || list.Count() <= 0)
				{
					_response.ErrorMessages.Add("Can not found any motor bike!");
					_response.IsSuccess = false;
					_response.StatusCode = HttpStatusCode.NotFound;
				}
				else
				{
					_response.IsSuccess = true;
					_response.StatusCode = HttpStatusCode.OK;
					_response.Result = list;
				}
				return _response;
			}
			catch (Exception ex)
			{
				_response.IsSuccess = false;
				_response.StatusCode = HttpStatusCode.BadRequest;
				_response.ErrorMessages = new List<string>()
				{
					ex.ToString()
				};
				return _response;
			}
		}

		[HttpGet("{id:int}")]
		public async Task<ApiResponse> GetById(int id)
		{
			try
			{
				var obj = await _unitOfWork.MotorBikeService.GetFirst(e => e.MotorId == id);
				if (obj == null)
				{
					_response.ErrorMessages.Add("Can not found any motor bike!");
					_response.IsSuccess = false;
					_response.StatusCode = HttpStatusCode.NotFound;
				}
				else
				{
					_response.IsSuccess = true;
					_response.StatusCode = HttpStatusCode.OK;
					_response.Result = obj;
				}
				return _response;
			}
			catch (Exception ex)
			{
				_response.IsSuccess = false;
				_response.StatusCode = HttpStatusCode.BadRequest;
				_response.ErrorMessages = new List<string>()
				{
					ex.ToString()
				};
				return _response;
			}
		}
	}
}
