﻿using API.DTO;
using API.DTO.OwnerDTO;
using API.DTO.UserDTO;
using API.Utility;
using API.Validation;
using AutoMapper;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Service.Service;
using Service.UnitOfWork;
using System.Net;

namespace API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class OwnerController : ControllerBase
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IEmailSender _emailSender;
		private ApiResponse _response;
		private readonly IMapper _mapper;

		public OwnerController(IUnitOfWork unitOfWork, IMapper mapper, IEmailSender emailSender)
		{
			_unitOfWork = unitOfWork;
			_emailSender = emailSender;
			_response = new ApiResponse();
			_mapper = mapper;
		}

		[Authorize(Roles = "Customer")]
		[HttpPut]
		[Route("OwnerRegister")]
		public async Task<IActionResult> OwnerRegister(OwnerRegisterDTO owner)
        {
            try
			{
                var userId = int.Parse(User.FindFirst("UserId")?.Value);
                var request = await _unitOfWork.RequestService.Get(x => x.SenderId == userId && x.Status == SD.Request_Pending && x.RequestTypeId != SD.Request_Report_Id);
                if (request.Count() > 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Result = false;
                    _response.ErrorMessages.Add("Bạn vẫn còn trong quá trình làm việc với một số hoạt động khác, vui lòng kết thúc trước khi đăng ký trở thành chủ xe!");
                    return BadRequest(_response);
                }
                var rs = InputValidation.OwnerRegisterValidation(owner.Phone, owner.IdCard, owner.Address);
				if (rs != "")
				{
					_response.StatusCode = HttpStatusCode.BadRequest;
					_response.Result = false;
					_response.ErrorMessages.Add(rs);
					return BadRequest(_response);
				}
				else
				{
					
					var userInDb = await _unitOfWork.UserService.GetFirst(x => x.UserId == userId);

					if (userInDb == null)
					{
						_response.StatusCode = HttpStatusCode.NotFound;
						_response.Result = false;
						_response.ErrorMessages.Add("Lỗi hệ thống!");
						return NotFound(_response);
					}

					var idCardInDb = await _unitOfWork.UserService.GetFirst(x => x.IdCard == owner.IdCard);
					if(idCardInDb != null && idCardInDb.IdCard != userInDb.IdCard)
					{
						_response.StatusCode = HttpStatusCode.BadRequest;
						_response.Result = false;
						_response.ErrorMessages.Add("Mã căn cước này đã được đăng ký!");
						return BadRequest(_response);
					}
					
					var newOwner = _mapper.Map(owner, userInDb);
					newOwner.RoleId = 3;
					await _unitOfWork.UserService.Update(newOwner);
					_response.StatusCode = HttpStatusCode.OK;
					_response.Result = true;
					_response.Message = ("Đăng ký thành công, vui lòng đăng nhập lại!");
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
	}
}
