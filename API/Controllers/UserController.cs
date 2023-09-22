using API.DTO;
using API.DTO.UserDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.UnitOfWork;
using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography;

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
		//[Authorize(Roles = "Admin")]
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
					_response.errors.Add("Không tìm thấy người dùng nào!");
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
				_response.errors = new List<string>()
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
				_response.errors.Add("Người dùng không tồn tại!");
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
				_response.errors = new List<string>()
				{
					ex.ToString()
				};
				return _response;
			}
		}

		[HttpPost]
		[Route("ChangePassword")]
		public async Task<ApiResponse>ChangePassword([FromQuery]int id, ResetPasswordDTO passwordDTO)
		{
			try
			{
				var user = await _unitOfWork.UserService.GetFirst(x => x.UserId == id);
				if( user == null)
				{
					_response.IsSuccess = false;
					_response.StatusCode = HttpStatusCode.BadRequest;
					_response.errors.Add("Người dùng không tồn tại!");
				}
				else
				{
					CreatePasswordHash(passwordDTO.Password, out byte[] passwordHash, out byte[] passwordSalt);
					user.PasswordHash = passwordHash;
					user.PasswordSalt = passwordSalt;
					await _unitOfWork.UserService.Update(user);
					_response.IsSuccess = true;
					_response.StatusCode = HttpStatusCode.OK;
					_response.Result = user;
				}
				return _response;
			}
			catch(Exception ex)
			{
				_response.IsSuccess = false;
				_response.StatusCode = HttpStatusCode.BadRequest;
				_response.errors = new List<string>()
				{
					ex.ToString()
				};
				return _response;
			}
		}

		private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
		{
			using (var hmac = new HMACSHA512())
			{
				passwordSalt = hmac.Key;
				passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
			}
		}
	}
}
