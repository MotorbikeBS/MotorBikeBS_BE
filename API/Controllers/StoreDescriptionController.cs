﻿using API.DTO;
using API.DTO.UserDTO;
using API.Utility;
using API.Validation;
using AutoMapper;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.BlobImageService;
using Service.UnitOfWork;
using System.Net;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreDescriptionController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private ApiResponse _response;
		private readonly IBlobService _blobService;

		public StoreDescriptionController(IUnitOfWork unitOfWork, IMapper mapper, IBlobService blobService)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_response = new ApiResponse();
			_blobService = blobService;
		}

		[Authorize(Roles = "Admin, Customer, Owner")]
		[HttpGet]
		public async Task<IActionResult> Get(string? status)
		{
			try
			{
				var roleId = int.Parse(User.FindFirst("RoleId")?.Value);
				if (roleId == SD.Role_Customer_Id || roleId == SD.Role_Owner_Id)
				{
					var storeForUser = await _unitOfWork.StoreDescriptionService.Get(x => x.Status == SD.active, includeProperties: "StoreImages");
					if (storeForUser == null)
					{
						_response.StatusCode = HttpStatusCode.NotFound;
						_response.ErrorMessages.Add("Không tìm thấy cửa hàng nào!");
						_response.IsSuccess = false;
						return NotFound(_response);
					}
					_response.StatusCode = HttpStatusCode.OK;
					_response.IsSuccess = true;
					_response.Result = storeForUser;
					return Ok(_response);
				}
				var store = await _unitOfWork.StoreDescriptionService.Get(includeProperties: "StoreImages");
				if (status != null)
					store = store.Where(x => x.Status == status);
				if (store == null)
				{
					_response.StatusCode = HttpStatusCode.NotFound;
					_response.ErrorMessages.Add("Không tìm thấy cửa hàng nào!");
					_response.IsSuccess = false;
					return NotFound(_response);
				}
				else
				{
					_response.StatusCode = HttpStatusCode.OK;
					_response.IsSuccess = true;
					_response.Result = store;
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
		[HttpGet("{id:int}")]
		public async Task<IActionResult> GetById(int id)
		{
			try
			{
				var userId = int.Parse(User.FindFirst("UserId")?.Value);
				var roleId = int.Parse(User.FindFirst("RoleId")?.Value);
				if (roleId == SD.Role_Store_Id)
				{
					if (userId != id)
					{
						_response.IsSuccess = false;
						_response.StatusCode = HttpStatusCode.BadGateway;
						_response.ErrorMessages.Add("Bạn không có quyền thực hiện chức năng này!");
						return NotFound(_response);
					}
					var storeOwner = await _unitOfWork.StoreDescriptionService.GetFirst(x => x.StoreId == userId, includeProperties: "StoreImages");
					_response.IsSuccess = true;
					_response.StatusCode = HttpStatusCode.OK;
					_response.Result = storeOwner;
					return Ok(_response);
				}
				var store = await _unitOfWork.StoreDescriptionService.GetFirst(x => x.StoreId == id, includeProperties: "StoreImages");
				if (store == null)
				{
					_response.StatusCode = HttpStatusCode.NotFound;
					_response.ErrorMessages.Add("Không tìm thấy cửa hàng!");
					_response.IsSuccess = false;
					return NotFound(_response);
				}
				else
				{
					_response.StatusCode = HttpStatusCode.OK;
					_response.IsSuccess = true;
					_response.Result = store;
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

		[Authorize(Roles = "Customer, Owner")]
		[HttpPost]
		[Route("store-register")]
		public async Task<IActionResult> StoreRegister([FromForm] StoreRegisterDTO store)
		{
			try
			{
				var rs = InputValidation.StoreRegisterValidation(store.StoreName, store.StorePhone, store.StoreEmail, store.Address, store.TaxCode);
				if (rs != "")
				{
					_response.StatusCode = HttpStatusCode.BadRequest;
					_response.Result = false;
					_response.ErrorMessages.Add(rs);
					return BadRequest(_response);
				}
				if (store.File == null || store.File.Length == 0)
				{
					_response.StatusCode = HttpStatusCode.BadRequest;
					_response.Result = false;
					_response.ErrorMessages.Add("Vui lòng chọn hình ảnh cửa hàng!");
					return BadRequest(_response);
				}

				var userId = int.Parse(User.FindFirst("UserId")?.Value);
				var storeInDb = await _unitOfWork.StoreDescriptionService.GetFirst(c => c.UserId == userId);
				var taxCodeInDb = await _unitOfWork.StoreDescriptionService.GetFirst(x => x.TaxCode == store.TaxCode);

				if (taxCodeInDb != null && storeInDb.Status != SD.refuse)
				{
					_response.StatusCode = HttpStatusCode.BadRequest;
					_response.Result = false;
					_response.ErrorMessages.Add("Mã số thuế này đã được đăng ký!");
					return BadRequest(_response);
				}

				if (storeInDb != null && storeInDb.Status != SD.refuse)
				{
					_response.StatusCode = HttpStatusCode.BadRequest;
					_response.Result = false;
					_response.ErrorMessages.Add("Người dùng nãy đã đăng ký trở thành cửa hàng!");
					return BadRequest(_response);
				}
				
				var userInDb = await _unitOfWork.UserService.GetFirst(c => c.UserId == userId);
				if (userInDb != null)
				{
					if(storeInDb != null && storeInDb.Status == SD.refuse)
					{
						var newStore = _mapper.Map(store, storeInDb);
						newStore.Status = SD.not_verify;
						await _unitOfWork.StoreDescriptionService.Update(newStore);

						var oldImg = await _unitOfWork.StoreImageService.GetFirst(x => x.StoreId == storeInDb.StoreId);
						var link = oldImg.ImageLink.Split('/').Last();

						await _blobService.DeleteBlob(link, SD.Storage_Container);
						await _unitOfWork.StoreImageService.Delete(oldImg);

						string fileName = $"{Guid.NewGuid()}{Path.GetExtension(store.File.FileName)}";
						var img = await _blobService.UploadBlob(fileName, SD.Storage_Container, store.File);
						StoreImage image = new()
						{
							ImageLink = img,
							StoreId = newStore.StoreId
						};
						await _unitOfWork.StoreImageService.Add(image);
					}
					else
					{
						var newStore = _mapper.Map<StoreDesciption>(store);
						newStore.Status = SD.not_verify;
						newStore.UserId = userId;
						newStore.StoreCreatedAt = DateTime.Now;
						await _unitOfWork.StoreDescriptionService.Add(newStore);

											string fileName = $"{Guid.NewGuid()}{Path.GetExtension(store.File.FileName)}";
					var img = await _blobService.UploadBlob(fileName, SD.Storage_Container, store.File);
					StoreImage image = new()
					{
						ImageLink = img,
						StoreId = newStore.StoreId
					};
					await _unitOfWork.StoreImageService.Add(image);
					}
					_response.IsSuccess = true;
					_response.StatusCode = HttpStatusCode.OK;
					_response.Message = ("Đăng ký thành công, vui lòng chờ xác thực!");
					return Ok(_response);
				}
				else
				{
					_response.IsSuccess = false;
					_response.StatusCode = HttpStatusCode.NotFound;
					_response.ErrorMessages.Add("Không tìm thấy người dùng!");
					return NotFound(_response);
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

		[Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("VerifyStore")]
        public async Task<IActionResult> VerifyStore(int storeId)
        {
            try
            {
                if (storeId <= 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages.Add("Lỗi hệ thống!");
                    _response.IsSuccess = false;
                    return BadRequest(_response);
                }
                var store = await _unitOfWork.StoreDescriptionService.GetFirst(x => x.StoreId == storeId);
                if (store == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.ErrorMessages.Add("Không tìm thấy cửa hàng!");
                    _response.IsSuccess = false;
                    return NotFound(_response);
                }
                else
                {
                    if (store.Status != SD.active)
                    {
                        var user = await _unitOfWork.UserService.GetFirst(x => x.UserId == store.UserId);
                        store.Status = SD.active;
                        await _unitOfWork.StoreDescriptionService.Update(store);
                        user.RoleId = 2;
                        await _unitOfWork.UserService.Update(user);
                        _response.StatusCode = HttpStatusCode.OK;
                        _response.IsSuccess = true;
                        _response.Message = "Xác minh thành công!";
                        return Ok(_response);
                    }
                    else
                    {
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.ErrorMessages.Add("Cửa hàng đã được xác minh!");
                        _response.IsSuccess = false;
                        return BadRequest(_response);
                    }
                }
            }catch(Exception ex)
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

		[Authorize(Roles ="Admin")]
		[HttpPut]
		[Route("RefuseStore")]
		public async Task<IActionResult>RefuseStore(int storeId)
		{
			try
			{
				var store = await _unitOfWork.StoreDescriptionService.GetFirst(x => x.StoreId == storeId);
				if (store == null)
				{
					_response.StatusCode = HttpStatusCode.NotFound;
					_response.ErrorMessages.Add("Không tìm thấy cửa hàng!");
					_response.IsSuccess = false;
					return NotFound(_response);
				}
				else
				{
					if (store.Status == SD.not_verify)
					{
						store.Status = SD.refuse;
						await _unitOfWork.StoreDescriptionService.Update(store);
						_response.StatusCode = HttpStatusCode.OK;
						_response.IsSuccess = true;
						_response.Message = "Từ chối thành công!";
						return Ok(_response);
					}
					else
					{
						_response.StatusCode = HttpStatusCode.BadGateway;
						_response.IsSuccess = false;
						_response.Message = "Không thành công!";
						return Ok(_response);
					}
				}
			}catch (Exception ex)
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

		[Authorize(Roles = "Admin")]
		[HttpPost]
		[Route("InactiveStore")]
		public async Task<IActionResult> InactiveStore(int storeId)
		{
			try
			{
				var store = await _unitOfWork.StoreDescriptionService.GetFirst(x => x.StoreId == storeId);
				if (store == null)
				{
					_response.StatusCode = HttpStatusCode.NotFound;
					_response.ErrorMessages.Add("Không tìm thấy cửa hàng!");
					_response.IsSuccess = false;
					return NotFound(_response);
				}
				else
				{
					if (store.Status == SD.active)
					{
						var user = await _unitOfWork.UserService.GetFirst(x => x.UserId == store.UserId);
						store.Status = SD.in_active;
						await _unitOfWork.StoreDescriptionService.Update(store);
						user.RoleId = 4;
						user.Status = SD.in_active;
						await _unitOfWork.UserService.Update(user);
						_response.StatusCode = HttpStatusCode.OK;
						_response.IsSuccess = true;
						_response.Message = "Ngưng hoạt động cửa hàng thành công!";
						return Ok(_response);
					}
					else
					{
						_response.StatusCode = HttpStatusCode.BadRequest;
						_response.ErrorMessages.Add("Cửa hàng đã được ngưng hoạt động!");
						_response.IsSuccess = false;
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
				return BadRequest(_response);
			}
		}
	}
}