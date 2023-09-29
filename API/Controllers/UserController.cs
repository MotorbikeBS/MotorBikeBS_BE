using API.DTO;
using API.DTO.UserDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.UnitOfWork;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using API.Utility;
using AutoMapper;
using Core.Models;
using API.Validation;

namespace API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserController : ControllerBase
	{
		private readonly IUnitOfWork _unitOfWork;
		private ApiResponse _response;
		private readonly IMapper _mapper;

		public UserController(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_response = new ApiResponse();
			_mapper = mapper;
		}


		[Authorize(Roles = "Admin")]
		[HttpGet]
		public async Task<IActionResult> Get()
		{
			try
			{
				var user = await _unitOfWork.UserService.Get(includeProperties: "Role");
				if (user == null || user.Count() <= 0)
				{
					_response.IsSuccess = false;
					_response.StatusCode = HttpStatusCode.NotFound;
					_response.ErrorMessages.Add("Không tìm thấy người dùng nào!");
					return NotFound(_response);
				}
				else
				{
					var userResponse = _mapper.Map<IEnumerable<UserResponseDTO>>(user);
					_response.IsSuccess = true;
					_response.StatusCode = HttpStatusCode.OK;
					_response.Result = userResponse;
					return Ok(_response);
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

		[Authorize]
		[HttpGet("{id:int}")]
		public async Task<IActionResult> GetById(int id)
		{
			try
			{
				var userId = int.Parse(User.FindFirst("UserId")?.Value);
				var roleId = int.Parse(User.FindFirst("RoleId")?.Value);

				if (roleId == SD.Role_Customer_Id)
				{
					if (userId != id)
					{
						_response.IsSuccess = false;
						_response.StatusCode = HttpStatusCode.BadGateway;
						_response.ErrorMessages.Add("Bạn không có quyền thực hiện chức năng này!");
						return NotFound(_response);
					}
					var c = await _unitOfWork.UserService.GetFirst(x => x.UserId == userId);
					var userResponse = _mapper.Map<UserResponseDTO>(c);
					_response.IsSuccess = true;
					_response.StatusCode = HttpStatusCode.OK;
					_response.Result = userResponse;
					return Ok(_response);
				}
				var user = await _unitOfWork.UserService.GetFirst(x => x.UserId == id);
				if (user == null)
				{
					_response.IsSuccess = false;
					_response.StatusCode = HttpStatusCode.NotFound;
					_response.ErrorMessages.Add("Người dùng không tồn tại!");
					return NotFound(_response);
				}
				else
				{
					var userResponse = _mapper.Map<UserResponseDTO>(user);
					_response.IsSuccess = true;
					_response.StatusCode = HttpStatusCode.OK;
					_response.Result = userResponse;
					return Ok(_response);
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
				return BadRequest(_response);
			}
		}

		[Authorize]
		[HttpPut("{id:int}")]
		public async Task<IActionResult> Put(int id, UserUpdateDTO userUpdateDTO)
		{
			try
			{
				var rs = InputValidation.UserUpdateValidation(userUpdateDTO.UserName, userUpdateDTO.Phone, userUpdateDTO.Gender, userUpdateDTO.Dob, userUpdateDTO.IdCard, userUpdateDTO.Address);
				if(rs != "")
				{
					_response.IsSuccess = false;
					_response.StatusCode = HttpStatusCode.BadGateway;
					_response.ErrorMessages.Add(rs);
					return BadRequest(_response);
				}
				var userId = int.Parse(User.FindFirst("UserId")?.Value);

				if(userId != id)
				{
					_response.IsSuccess = false;
					_response.StatusCode = HttpStatusCode.BadRequest;
					_response.ErrorMessages.Add("Bạn không có quyền thực hiện chức năng này");
					return BadRequest(_response);
				}
				var user = await _unitOfWork.UserService.GetFirst(x => x.UserId == id);
				if(user == null)
				{
					_response.IsSuccess = false;
					_response.StatusCode = HttpStatusCode.NotFound;
					_response.ErrorMessages.Add("Không tìm thấy người dùng này");
					return NotFound(_response);
				}
				var userUpdate = _mapper.Map(userUpdateDTO, user);
				userUpdate.UserUpdatedAt = DateTime.Now;
				await _unitOfWork.UserService.Update(userUpdate);
				_response.IsSuccess = true;
				_response.StatusCode = HttpStatusCode.OK;
				_response.Message = ("Cập nhật thành công");
				return Ok(_response);
			}
			catch(Exception ex)
			{
				_response.IsSuccess = false;
				_response.StatusCode = HttpStatusCode.NotFound;
				_response.ErrorMessages = new List<string>()
				{
					ex.ToString()
				};
				return BadRequest(_response);
			}
		}

		[Authorize]
		[HttpPost]
		[Route("ChangePassword")]
		public async Task<IActionResult> ChangePassword([FromQuery] int id, ResetPasswordDTO passwordDTO)
		{
			try
			{
				var user = await _unitOfWork.UserService.GetFirst(x => x.UserId == id);
				if (user == null)
				{
					_response.IsSuccess = false;
					_response.StatusCode = HttpStatusCode.BadRequest;
					_response.ErrorMessages.Add("Người dùng không tồn tại!");
					return BadRequest(_response);
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
					return Ok(_response);
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
				return BadRequest(_response);
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