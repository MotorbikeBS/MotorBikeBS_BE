using API.DTO;
using Microsoft.AspNetCore.Mvc;
using Service.UnitOfWork;
using System.Collections.Generic;
using System.Net;

namespace API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserController : ControllerBase
	{
		private readonly IUnitOfWork _unitOfWork;
		private ApiResponse _response;

		public UserController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
			_response = new ApiResponse();
		}

		[HttpGet]
		public async Task<ApiResponse>Get()
		{
			try
			{
				var user = await _unitOfWork.UserService.Get();
				if (user == null || user.Count() <= 0)
				{
					_response.IsSuccess = false;
					_response.StatusCode = HttpStatusCode.NotFound;
					_response.ErrorMessages.Add("Can't found any user!");
				}
				else
				{
					_response.IsSuccess = true;
					_response.StatusCode = HttpStatusCode.OK;
					_response.Result = user;
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
		public async Task<ApiResponse>GetById(int id)
		{
			try
			{
			var user = await _unitOfWork.UserService.GetFirst(x => x.UserId == id);
			if(user == null)
			{
				_response.IsSuccess = false;
				_response.StatusCode = HttpStatusCode.NotFound;
				_response.ErrorMessages.Add("Can't found user!");
			}
				else
				{
					_response.IsSuccess = true;
					_response.StatusCode = HttpStatusCode.OK;
					_response.Result = user;
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
