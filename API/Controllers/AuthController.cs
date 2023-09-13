using API.DTO;
using AutoMapper;
using Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Service.UnitOfWork;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;

namespace API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		public IConfiguration _configuration;
		private readonly IUnitOfWork _unitOfWork;
		private ApiResponse _response;
		private readonly IMapper _mapper;

		public AuthController(IConfiguration configuration, IUnitOfWork unitOfWork, IMapper mapper)
		{
			_configuration = configuration;
			_unitOfWork = unitOfWork;
			_response = new ApiResponse();
			_mapper = mapper;
		}

		[HttpPost]
		[Route("Login")]
		public async Task<ApiResponse>Login(LoginDTO obj)
		{
			try
			{
				var Dto = _mapper.Map<User>(obj);
				var user = await _unitOfWork.UserService.Login(Dto);
				if(user == null)
				{
					_response.IsSuccess = false;
					_response.StatusCode = HttpStatusCode.BadRequest;
					_response.ErrorMessages.Add("Wrong Email or Password!");
				}
				else
				{
					var role = await _unitOfWork.RoleService.GetFirst(x => x.RoleId == user.RoleId);
					var claims = new[] {
						new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
						new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
						new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
						new Claim("AccountId", user.UserId.ToString()),
						new Claim("Name", user.UserName),
						new Claim(ClaimTypes.Role, role.Title.ToString()),
						new Claim("RoleName", role.Title.ToString()),
						new Claim("Email", user.Email)
					};

					var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
					var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
					var token = new JwtSecurityToken(
						_configuration["Jwt:Issuer"],
						_configuration["Jwt:Audience"],
						claims,
						expires: DateTime.UtcNow.AddDays(5),
						signingCredentials: signIn);

					var tokenObject = new { Token = new JwtSecurityTokenHandler().WriteToken(token) };
					var tokenJson = JsonConvert.SerializeObject(tokenObject);
					_response.IsSuccess = true;
					_response.StatusCode = HttpStatusCode.OK;
					_response.Result = tokenObject;
				}
				return _response;
			}
			catch
			{
				_response.IsSuccess = false;
				_response.StatusCode = HttpStatusCode.BadRequest;
				_response.ErrorMessages.Add("Something wrong!");
				return _response;
			}
		}

		[HttpPost]
		[Route("Register")]
		public async Task<ApiResponse>Register(RegisterDTO user)
		{
			try
			{
				var userInDb = await _unitOfWork.UserService.GetFirst(c => c.Email == user.Email);
				if(userInDb == null)
				{
					var newUser = _mapper.Map<User>(user);
					await _unitOfWork.UserService.Add(newUser);
					_response.IsSuccess = true;
					_response.StatusCode = HttpStatusCode.OK;
					_response.Result = newUser;
				}
				else
				{
					_response.IsSuccess = false;
					_response.StatusCode = HttpStatusCode.BadRequest;
					_response.ErrorMessages.Add("Email is already exist!");
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
