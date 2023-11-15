using API.DTO;
using API.DTO.UserDTO;
using API.Validation;
using AutoMapper;
using Core.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using API.Utility;

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
        public DateTime VnDate = DateTime.Now.ToLocalTime();

        public AuthController(IConfiguration configuration, IUnitOfWork unitOfWork, IMapper mapper, IEmailSender emailSender)
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _emailSender = emailSender;
            _response = new ApiResponse();
            _mapper = mapper;
        }

		[HttpPost]
		[Route("user-register")]
		public async Task<IActionResult> UserRegister(RegisterDTO user)
		{
			try
			{
				var rs = InputValidation.RegisterValidation(user.UserName, user.Email, user.Password, user.PasswordConfirmed);
				if (rs != "")
				{
					_response.IsSuccess = true;
					_response.StatusCode = HttpStatusCode.BadRequest;
					_response.ErrorMessages.Add(rs);
					return BadRequest(_response);
				}
				user.UserName = InputValidation.CleanAndFormatUserName(user.UserName);
				var userInDb = await _unitOfWork.UserService.GetFirst(c => c.Email == user.Email);
				
				if (userInDb == null)
				{
					CreatePasswordHash(user.Password, out byte[] passwordHash, out byte[] passwordSalt);
					var newUser = _mapper.Map<User>(user);
					newUser.Status = SD.not_verify;
					newUser.PasswordHash = passwordHash;
					newUser.PasswordSalt = passwordSalt;
					newUser.VerifycationTokenExpires = VnDate.AddHours(3);
					newUser.RoleId = 4;
					//newUser.UserName = newUser.UserName.ToString().;
					newUser.VerifycationToken = CreateRandomToken();
					await _unitOfWork.UserService.Add(newUser);

					var subject = "Xác minh tài khoản";
					var htmlMessage = $"<p>Xin chào {newUser.UserName},<br>Vui lòng nhấn vào đây <a href=\"https://motorbikebs.azurewebsites.net/users/{newUser.UserId}/verify/{newUser.VerifycationToken}\">here</a> để xác minh tài khoản của bạn.</p>";

                    await _emailSender.SendEmailAsync(user.Email, subject, htmlMessage);

					_response.IsSuccess = true;
					_response.StatusCode = HttpStatusCode.OK;
					return Ok(_response);
				}
				else
				{
					if (userInDb != null && userInDb.Status.Equals(SD.not_verify))
					{
						CreatePasswordHash(user.Password, out byte[] passwordHash, out byte[] passwordSalt);
						userInDb.PasswordHash = passwordHash;
						userInDb.PasswordSalt = passwordSalt;
						userInDb.UserName = user.UserName;
						userInDb.VerifycationToken = CreateRandomToken();
						await _unitOfWork.UserService.Update(userInDb);

						var subject = "Xác minh tài khoản";
						var htmlMessage = $"<p>Xin chào {userInDb.UserName},<br>Vui lòng nhấn vào đây <a href=\"https://motorbikebs.azurewebsites.net/users/{userInDb.UserId}/verify/{userInDb.VerifycationToken}\">here</a> để xác minh tài khoản của bạn.</p>";

						await _emailSender.SendEmailAsync(user.Email, subject, htmlMessage);
						_response.IsSuccess = true;
						_response.StatusCode = HttpStatusCode.OK;
						_response.ErrorMessages.Add("Email này đã được đăng ký trước đây nhưng chưa xác nhận, vui lòng xác nhận email");
						return Ok(_response);
					}
					else
					{
						_response.IsSuccess = false;
						_response.StatusCode = HttpStatusCode.BadRequest;
						_response.ErrorMessages.Add("Email này đã được đăng ký!");
						return BadRequest(_response);
					}
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
				return Ok(_response);
			}
		}

		[HttpPost]
		[Route("login")]
		public async Task<IActionResult> Login(LoginDTO obj)
		{
			try
			{
				var Dto = _mapper.Map<User>(obj);
				var user = await _unitOfWork.UserService.GetFirst(x => x.Email == obj.Email);
				if (user == null)
				{
					_response.IsSuccess = false;
					_response.StatusCode = HttpStatusCode.NotFound;
					_response.ErrorMessages.Add("Email hoặc mật khẩu không đúng!");
					return NotFound(_response);
				}
				else if (!VerifyPasswordHash(obj.Password, user.PasswordHash, user.PasswordSalt))
				{
					_response.IsSuccess = false;
					_response.StatusCode = HttpStatusCode.BadRequest;
					_response.ErrorMessages.Add("Email hoặc mật khẩu không hợp lệ!");
					return BadRequest(_response);
				}
				else if (user.UserVerifyAt == null)
				{
					_response.IsSuccess = false;
					_response.StatusCode = HttpStatusCode.BadRequest;
					_response.ErrorMessages.Add("Người dùng chưa xác minh!");
					return BadRequest(_response);
				}
				else if(user.Status == SD.in_active)
				{
					_response.IsSuccess = false;
					_response.StatusCode = HttpStatusCode.BadRequest;
					_response.ErrorMessages.Add("Tài khoản đã bị khóa do vi phạm, vui lòng liên hệ Admin để được hỗ trợ!");
					return BadRequest(_response);
				}
				else
				{
					var role = await _unitOfWork.RoleService.GetFirst(x => x.RoleId == user.RoleId);
					var claims = new[] {
						new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
						new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
						new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
						new Claim("UserId", user.UserId.ToString()),
						new Claim("UserName", user.UserName),
						new Claim(ClaimTypes.Role, role.Title.ToString()),
						new Claim("RoleName", role.Title.ToString()),
						new Claim("RoleId", user.RoleId.ToString()),
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

					//var tokenObject = new { Token = new JwtSecurityTokenHandler().WriteToken(token) };
					var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
					var userResponse = _mapper.Map<LoginResponseDTO>(user);
					userResponse.Token = tokenString;
					_response.IsSuccess = true;
					_response.StatusCode = HttpStatusCode.OK;
					_response.Message = $"Chào mừng trở lại {user.UserName}";
					_response.Result = userResponse;
					return Ok(_response);
				}
			}
			catch
			{
				_response.IsSuccess = false;
				_response.StatusCode = HttpStatusCode.BadRequest;
				_response.ErrorMessages.Add("Lỗi hệ thống!");
				return BadRequest(_response);
			}
		}

		[HttpPost]
		[Route("verify-account")]
		public async Task<IActionResult> VerifyAccount(int id, string token)
		{
			try
			{
				var user = await _unitOfWork.UserService.GetFirst(x => x.VerifycationToken == token);
				if (user == null || user.UserId != id)
				{
					_response.IsSuccess = false;
					_response.StatusCode = HttpStatusCode.BadRequest;
					_response.ErrorMessages.Add("Mã xác minh không hợp lệ!");
					return BadRequest(_response);
				}
				else
				{
					if (user.VerifycationTokenExpires < VnDate)
					{
						_response.IsSuccess = false;
						_response.StatusCode = HttpStatusCode.BadRequest;
						_response.ErrorMessages.Add("Mã xác minh đã hết hạn!");
						return BadRequest(_response);
					}
					if (user.Status == SD.active)
					{
						_response.IsSuccess = false;
						_response.StatusCode = HttpStatusCode.BadRequest;
						_response.ErrorMessages.Add("Mã xác minh không hợp lệ!");
						return BadRequest(_response);
					}
					else
					{
						user.UserVerifyAt = DateTime.UtcNow;
						user.Status = SD.active;
						user.VerifycationToken = "";
						user.VerifycationTokenExpires = null;
						await _unitOfWork.UserService.Update(user);
						_response.IsSuccess = true;
						_response.StatusCode = HttpStatusCode.OK;
						_response.Message = "Xác minh thành công";
						return Ok(_response);
					}
				}
			}
			catch (Exception ex)
			{
				_response.IsSuccess = false;
				_response.StatusCode = HttpStatusCode.BadRequest;
				_response.ErrorMessages.Add("Lỗi hệ thống!");
				return BadRequest(_response);
			}
		}

		[HttpPost]
		[Route("forgot-password")]
		public async Task<IActionResult> ForgotPassword(string email)
		{
			try
			{
				if (!InputValidation.EmailValidation(email))
				{
					_response.IsSuccess = false;
					_response.StatusCode = HttpStatusCode.NotFound;
					_response.ErrorMessages.Add("Email không hợp lệ!");
					return NotFound(_response);
				}
				var user = await _unitOfWork.UserService.GetFirst(x => x.Email == email);
				if (user == null)
				{
					_response.IsSuccess = false;
					_response.StatusCode = HttpStatusCode.NotFound;
					_response.ErrorMessages.Add("Không tìm thấy email này!");
					return NotFound(_response);
				}
				else
				{
					if (user.Status == SD.not_verify)
					{
						_response.IsSuccess = false;
						_response.StatusCode = HttpStatusCode.NotFound;
						_response.ErrorMessages.Add("Tài khoản chưa xác minh!");
						return NotFound(_response);
					}
					else
					{
						user.PasswordResetToken = CreateRandomToken();
						user.ResetTokenExpires = VnDate.AddHours(3);
						await _unitOfWork.UserService.Update(user);

						var subject = "Thay đổi mật khẩu";
						var htmlMessage = $"<p>Xin chào {user.UserName},<br>Vui lòng nhấn vào đây <a href=\"https://motorbikebs.azurewebsites.net/user/{user.UserId}/reset-password/{user.PasswordResetToken}\">here</a> để đổi mật khẩu của bạn.</p>";

						await _emailSender.SendEmailAsync(user.Email, subject, htmlMessage);
						_response.IsSuccess = true;
						_response.StatusCode = HttpStatusCode.OK;
						_response.Message = "Vui lòng xác minh trong email!";
						return Ok(_response);
					}
				}
			}
			catch (Exception ex)
			{
				_response.IsSuccess = false;
				_response.StatusCode = HttpStatusCode.BadRequest;
				_response.ErrorMessages.Add("Lỗi hệ thống!");
				return Ok(_response);
			}
		}

		[HttpPost]
		[Route("reset-password")]
		public async Task<IActionResult> ResetPassword(string token, ResetPasswordDTO request)
		{
			try
			{
				if (!request.Password.Equals(request.PasswordConfirmed))
				{
					_response.IsSuccess = false;
					_response.StatusCode = HttpStatusCode.BadRequest;
					_response.ErrorMessages.Add("Mật khẩu xác minh không trùng với mật khẩu!");
					return BadRequest(_response);
				}
				if (request.Password.Length < 6 || request.PasswordConfirmed.Length < 6)
				{
					_response.IsSuccess = false;
					_response.StatusCode = HttpStatusCode.BadRequest;
					_response.ErrorMessages.Add("Độ dài mật khẩu phải từ 6 ký tự trở lên!");
					return BadRequest(_response);
				}
				var user = await _unitOfWork.UserService.GetFirst(x => x.PasswordResetToken == token);
				if (user == null)
				{
					_response.IsSuccess = false;
					_response.StatusCode = HttpStatusCode.BadRequest;
					_response.ErrorMessages.Add("Mã xác minh không lợp lệ!");
					return BadRequest(_response);
				}
				else if (user.ResetTokenExpires < VnDate)
				{
					_response.IsSuccess = false;
					_response.StatusCode = HttpStatusCode.BadRequest;
					_response.ErrorMessages.Add("Mã xác minh đã hết hạn!");
					return BadRequest(_response);
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
					_response.Message = "Mật khẩu đã được thay đổi!";
					return Ok(_response);
				}
			}
			catch (Exception ex)
			{
				_response.IsSuccess = false;
				_response.StatusCode = HttpStatusCode.BadRequest;
				_response.ErrorMessages.Add("Lỗi hệ thống!");
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
            return Convert.ToHexString(RandomNumberGenerator.GetBytes(32));
        }

    }
}