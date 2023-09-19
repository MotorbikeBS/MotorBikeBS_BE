using API.DTO;
using API.DTO.UserDTO;
using API.Validation;
using AutoMapper;
using Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Service.Service;
using Service.UnitOfWork;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		public IConfiguration _configuration;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IEmailSender _emailSender;
		private ApiResponse _response;
		private readonly IMapper _mapper;

		public AuthController(IConfiguration configuration, IUnitOfWork unitOfWork, IMapper mapper, IEmailSender emailSender)
		{
			_configuration = configuration;
			_unitOfWork = unitOfWork;
			_emailSender = emailSender;
			_response = new ApiResponse();
			_mapper = mapper;
		}

		[HttpPost]
		[Route("CustomerRegister")]
		public async Task<ApiResponse> UserRegister(RegisterDTO user)
		{
			try
			{
				//if (!SingleSpaceBetweenNamesAttribute.SpaceValidation(user.UserName))
				//{
				//	_response.IsSuccess = false;
				//	_response.StatusCode = HttpStatusCode.BadRequest;
				//	_response.ErrorMessages.Add("User name is not valid!");
				//	return _response;
				//}
				var userInDb = await _unitOfWork.UserService.GetFirst(c => c.Email == user.Email);
				var role = await _unitOfWork.RoleService.GetFirst(x => x.RoleId == user.RoleId);
				if(role == null)
				{
					_response.IsSuccess = false;
					_response.StatusCode = HttpStatusCode.BadRequest;
					_response.ErrorMessages.Add("Role not exist!");
					return _response;
				}
				if (userInDb == null)
				{
					CreatePasswordHash(user.Password, out byte[] passwordHash, out byte[] passwordSalt);
					var newUser = _mapper.Map<User>(user);
					newUser.Status = "NOT VERIFY";
					newUser.PasswordHash = passwordHash;
					newUser.PasswordSalt = passwordSalt;
					newUser.VerifycationToken = CreateRandomToken();
					await _unitOfWork.UserService.Add(newUser);

					var subject = "Verify Token";
					var htmlMessage = $"<p>Hello {newUser.UserName},<br>Please click <a href=\"http://localhost:3000/users/reset-password?token={newUser.VerifycationToken}\">here</a> to verify your password.</p>";

					await _emailSender.SendEmailAsync(user.Email, subject, htmlMessage);

					_response.IsSuccess = true;
					_response.StatusCode = HttpStatusCode.OK;
					_response.Result = newUser;
				}
				else
				{
					if(userInDb.Status.Equals("NOT VERIFY"))
					{
						CreatePasswordHash(user.Password, out byte[] passwordHash, out byte[] passwordSalt);
						userInDb.PasswordHash = passwordHash;
						userInDb.PasswordSalt = passwordSalt;
						userInDb.UserName = user.UserName;
						userInDb.VerifycationToken = CreateRandomToken();
						await _unitOfWork.UserService.Update(userInDb);

						var subject = "Verify Token";
						var htmlMessage = $"<p>Hello {userInDb.UserName},<br>Please click <a href=\"http://localhost:3000/users/reset-password?token={userInDb.VerifycationToken}\">here</a> to verify your password.</p>";

						await _emailSender.SendEmailAsync(user.Email, subject, htmlMessage);
						_response.IsSuccess = true;
						_response.StatusCode = HttpStatusCode.OK;
						_response.Result = userInDb;
					}
					else
					{
						_response.IsSuccess = false;
						_response.StatusCode = HttpStatusCode.BadRequest;
						_response.ErrorMessages.Add("Email is already exist!");
					}
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

		[HttpPost]
		[Route("StoreRegister")]
		public async Task<ApiResponse> StoreRegister(StoreRegisterDTO store)
		{
			try
			{
				var userInDb = await _unitOfWork.UserService.GetFirst(c => c.UserId == store.UserId);
				if (userInDb != null)
				{
					var newStore = _mapper.Map<StoreDesciption>(store);
					newStore.Status = "NOT VERIFY";
					await _unitOfWork.StoreDescriptionService.Add(newStore);
					//userInDb.Status = "STORE NOT VERIFY";
					//await _unitOfWork.UserService.Update(userInDb);
					_response.IsSuccess = true;
					_response.StatusCode = HttpStatusCode.OK;
					_response.Result = newStore;
				}
				else
				{
					_response.IsSuccess = false;
					_response.StatusCode = HttpStatusCode.BadRequest;
					_response.ErrorMessages.Add("User not exist!");
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

		[HttpPost]
		[Route("Login")]
		public async Task<ApiResponse> Login(LoginDTO obj)
		{
			try
			{
				var Dto = _mapper.Map<User>(obj);
				var user = await _unitOfWork.UserService.GetFirst(x => x.Email == obj.Email);
				if (user == null)
				{
					_response.IsSuccess = false;
					_response.StatusCode = HttpStatusCode.BadRequest;
					_response.ErrorMessages.Add("User not found!");
				}
				else if (user.UserVerifyAt == null)
				{
					_response.IsSuccess = false;
					_response.StatusCode = HttpStatusCode.BadRequest;
					_response.ErrorMessages.Add("Not Verify!");
				}
				else if (!VerifyPasswordHash(obj.Password, user.PasswordHash, user.PasswordSalt))
				{
					_response.IsSuccess = false;
					_response.StatusCode = HttpStatusCode.BadRequest;
					_response.ErrorMessages.Add("Password is incorrect!");
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
						expires: DateTime.UtcNow.AddHours(5),
						signingCredentials: signIn);

					var tokenObject = new { Token = new JwtSecurityTokenHandler().WriteToken(token) };
					var tokenJson = JsonConvert.SerializeObject(tokenObject);
					_response.IsSuccess = true;
					_response.StatusCode = HttpStatusCode.OK;
					_response.Message = $"Welcome back {user.UserName}";
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
		[Route("VerifyAccount")]
		public async Task<ApiResponse> VerifyAccount(string token)
		{
			try
			{
				var user = await _unitOfWork.UserService.GetFirst(x => x.VerifycationToken == token);
				if (user == null)
				{
					_response.IsSuccess = false;
					_response.StatusCode = HttpStatusCode.BadRequest;
					_response.ErrorMessages.Add("Invalid Token!");
				}
				else
				{
					user.UserVerifyAt = DateTime.UtcNow;
					user.Status = "ACTIVE";
					await _unitOfWork.UserService.Update(user);
					_response.IsSuccess = true;
					_response.StatusCode = HttpStatusCode.OK;
					_response.Message = "Verify successfully";
				}
				return _response;
			}
			catch (Exception ex)
			{
				_response.IsSuccess = false;
				_response.StatusCode = HttpStatusCode.BadRequest;
				_response.ErrorMessages.Add("Something wrong!");
				return _response;
			}
		}

		[HttpPost]
		[Route("ForgotPassword")]
		public async Task<ApiResponse> ForgotPassword(string email)
		{
			try
			{
				var user = await _unitOfWork.UserService.GetFirst(x => x.Email == email);
				if (user == null)
				{
					_response.IsSuccess = false;
					_response.StatusCode = HttpStatusCode.BadRequest;
					_response.ErrorMessages.Add("Invalid email address!");
				}
				else
				{
					user.PasswordResetToken = CreateRandomToken();
					user.ResetTokenExpires = DateTime.Now.AddHours(3);
					await _unitOfWork.UserService.Update(user);

					var subject = "Reset Password";
					//var message = $"{newUser.VerifycationToken}";
					var htmlMessage = $"<p>Hello {user.UserName},<br>Please click <a href=\"http://localhost:3000/users/reset-password?token=${user.VerifycationToken}\">here</a> to reset your password.</p>";

					await _emailSender.SendEmailAsync(user.Email, subject, htmlMessage);
					_response.IsSuccess = true;
					_response.StatusCode = HttpStatusCode.OK;
				}
				return _response;
			}
			catch (Exception ex)
			{
				_response.IsSuccess = false;
				_response.StatusCode = HttpStatusCode.BadRequest;
				_response.ErrorMessages.Add("Something wrong!");
				return _response;
			}
		}

		[HttpPost]
		[Route("ResetPassword")]
		public async Task<ApiResponse> ResetPassword(ResetPasswordDTO request)
		{
			try
			{
				var user = await _unitOfWork.UserService.GetFirst(x => x.PasswordResetToken == request.Token);
				if (user == null)
				{
					_response.IsSuccess = false;
					_response.StatusCode = HttpStatusCode.BadRequest;
					_response.ErrorMessages.Add("Invalid token!");
				}
				else if (user.ResetTokenExpires < DateTime.Now)
				{
					_response.IsSuccess = false;
					_response.StatusCode = HttpStatusCode.BadRequest;
					_response.ErrorMessages.Add("Your token has expired!");
				}
				else
				{
					CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
					user.PasswordSalt = passwordSalt;
					user.PasswordHash = passwordHash;
					user.PasswordResetToken = null;
					user.ResetTokenExpires = null;
					await _unitOfWork.UserService.Update(user);
					_response.IsSuccess = false;
					_response.StatusCode = HttpStatusCode.OK;
					_response.Message = "Password changed!";
				}
				return _response;
			}
			catch (Exception ex)
			{
				_response.IsSuccess = false;
				_response.StatusCode = HttpStatusCode.BadRequest;
				_response.ErrorMessages.Add("Something wrong!");
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

		private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
		{
			using (var hmac = new HMACSHA512(passwordSalt))
			{
				var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
				return computedHash.SequenceEqual(passwordHash);
			}
		}

		private string CreateRandomToken()
		{
			return Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
		}

	}
}
