﻿using API.DTO;
using API.DTO.PostBoostingDTO;
using API.DTO.UserDTO;
using API.DTO.WishlistDTO;
using API.Utility;
using AutoMapper;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.UnitOfWork;
using System.Net;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class WishListController : ControllerBase
	{
		private readonly IUnitOfWork _unitOfWork;
		private ApiResponse _response;
		private readonly IMapper _mapper;
        public DateTime VnDate = DateTime.Now.ToLocalTime();
        public WishListController(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_response = new ApiResponse();
			_mapper = mapper;
		}

		[Authorize(Roles ="Customer")]
		[HttpGet]
		[Route("GetWishList")]
		public async Task<IActionResult> GetWishList()
		{
			try
			{
				var userId = int.Parse(User.FindFirst("UserId")?.Value);
				var list = await _unitOfWork.WishListService.Get(x => x.UserId == userId
				&& x.Motor.MotorStatusId != SD.Status_Storage,
				includeProperties: new string[] { "Motor", "Motor.MotorStatus", "Motor.MotorbikeImages", "Motor.Store", "Motor.MotorType" });
				if (list == null || list.Count()<1)
				{
					_response.IsSuccess = false;
					_response.StatusCode = HttpStatusCode.NotFound;
					_response.ErrorMessages.Add("Mục yêu thích đang trống!");
					return NotFound(_response);
				}
				var response = _mapper.Map<IEnumerable<WishlistResponseDTO>>(list);

                var listPostBoosting = await _unitOfWork.PostBoostingService.Get(x => x.Status == SD.Request_Accept
                                                                                && x.StartTime < VnDate && x.EndTime > VnDate);
                foreach (var wishList in response)
                {
                    var matchingPostBoosting = listPostBoosting.FirstOrDefault(p => p.MotorId == wishList.MotorId);

                    if (matchingPostBoosting != null)
                    {
                        var boostingInfor = _mapper.Map<PostBoostingCreateDTO>(matchingPostBoosting);
                        wishList.Motor.Boosting = boostingInfor;
                    }
                }
                foreach (var r in response)
                {
                    foreach (var PostingType in SD.RequestPostingTypeArray)
                    {
                        var request = await _unitOfWork.RequestService.GetLast(
                                e => e.MotorId == r.MotorId && e.RequestTypeId == PostingType && e.Status == SD.Request_Accept
                        );
                        if (request != null) { r.Motor.PostingAt = request?.Time; }
                    }
                }
                _response.Message = list.Count().ToString();
				_response.IsSuccess = true;
				_response.Result = response;
				_response.StatusCode = HttpStatusCode.OK;
				return Ok(_response);
			}
			catch(Exception ex)
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

		[Authorize(Roles = "Customer")]
		[HttpPost]
		[Route("AddToWishList")]
		public async Task<IActionResult> AddToWishList(int motorId)
		{
			try
			{
				var motor = await _unitOfWork.MotorBikeService.GetFirst(x => x.MotorId == motorId);
				if (motor == null)
				{
					_response.IsSuccess = false;
					_response.StatusCode = HttpStatusCode.NotFound;
					_response.ErrorMessages.Add("Không tìm thấy xe máy!");
					return NotFound(_response);
				}
				if (motor.MotorStatusId != SD.Status_Posting 
					&& motor.MotorStatusId != SD.Status_nonConsignment
					&& motor.MotorStatusId != SD.Status_Consignment)
				{
					_response.IsSuccess = false;
					_response.StatusCode = HttpStatusCode.NotFound;
					_response.ErrorMessages.Add("Không tìm thấy xe máy!");
					return NotFound(_response);
				}
				var motorModel = await _unitOfWork.MotorModelService.GetFirst(x => x.ModelId == motor.ModelId);
				var motorBrand = await _unitOfWork.MotorBrandService.GetFirst(x => x.BrandId == motorModel.BrandId);
				if (motorModel == null || motorBrand == null)
				{
					_response.IsSuccess = false;
					_response.StatusCode = HttpStatusCode.NotFound;
					_response.ErrorMessages.Add("Không tìm thấy thương hiệu xe máy!");
					return NotFound(_response);
				}

				var userId = int.Parse(User.FindFirst("UserId")?.Value);
				IEnumerable<Wishlist> list = await _unitOfWork.WishListService.Get(x => x.UserId == userId);
				var duplicateWishList = list.FirstOrDefault(x => x.MotorId == motorId);
				if(duplicateWishList != null)
				{
					_response.IsSuccess = false;
					_response.StatusCode = HttpStatusCode.BadRequest;
					_response.ErrorMessages.Add("Xe máy này đã có trong mục yêu thích!");
					return BadRequest(_response);
				}
				string name = motorBrand.BrandName + " " + motorModel.ModelName;
				Wishlist wishlist = new Wishlist()
				{
					UserId = userId,
					MotorId = motorId,
                    MotorName = name,
				};
				await _unitOfWork.WishListService.Add(wishlist);
				_response.IsSuccess = true;
				_response.StatusCode = HttpStatusCode.OK;
				_response.Message = "Đã thêm vào mục yêu thích!";
				return Ok(_response);
			}
			catch(Exception ex)
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

		[Authorize(Roles ="Customer")]
		[HttpDelete]
		[Route("DeleteWishList")]
		public async Task<IActionResult> DeleteWishList(int motorId)
		{
			try
			{
				var userId = int.Parse(User.FindFirst("UserId")?.Value);
				IEnumerable<Wishlist> list = await _unitOfWork.WishListService.Get(x => x.UserId == userId);
				if(list == null)
				{
					_response.IsSuccess = false;
					_response.StatusCode = HttpStatusCode.NotFound;
					_response.ErrorMessages.Add("Mục yêu thích đang trống!");
					return NotFound(_response);
				}
				var wishList = list.FirstOrDefault(x => x.MotorId == motorId);
				if (wishList == null)
				{
					_response.IsSuccess = false;
					_response.StatusCode = HttpStatusCode.NotFound;
					_response.ErrorMessages.Add("Không tìm thấy mục yêu thích này!");
					return NotFound(_response);
				}
				await _unitOfWork.WishListService.Delete(wishList);
				_response.IsSuccess = true;
				_response.StatusCode = HttpStatusCode.OK;
				_response.Message = "Đã xóa khỏi mục yêu thích!";
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

		[Authorize(Roles = "Customer")]
		[HttpDelete]
		[Route("DeleteAllWishList")]
		public async Task<IActionResult> DeleteAllWishList()
		{
			try
			{
				var userId = int.Parse(User.FindFirst("UserId")?.Value);
				IEnumerable<Wishlist> list = await _unitOfWork.WishListService.Get(x => x.UserId == userId);
				if (list == null)
				{
					_response.IsSuccess = false;
					_response.StatusCode = HttpStatusCode.NotFound;
					_response.ErrorMessages.Add("Mục yêu thích đang trống!");
					return NotFound(_response);
				}
				foreach(var item in list)
				{
					await _unitOfWork.WishListService.Delete(item);
				}
				_response.IsSuccess = true;
				_response.StatusCode = HttpStatusCode.OK;
				_response.Message = "Đã xóa tất cả khỏi mục yêu thích!";
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
