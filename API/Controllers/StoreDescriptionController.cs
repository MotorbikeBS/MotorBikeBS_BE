using API.DTO;
using API.DTO.UserDTO;
using API.Utility;
using API.Validation;
using AutoMapper;
using Core.Models;
using Microsoft.AspNetCore.Mvc;
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

        public StoreDescriptionController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _response = new ApiResponse();
        }

        [HttpGet]
        public async Task<ApiResponse> Get(string? status)
        {
            try
            {
                var store = await _unitOfWork.StoreDescriptionService.Get();
                if (status != null)
                    store = store.Where(x => x.Status == status);
                if (store == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.ErrorMessages.Add("Không tìm thấy cửa hàng nào!");
                    _response.IsSuccess = false;
                }
                else
                {
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.IsSuccess = true;
                    _response.Result = store;
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
		[Route("store-register")]
		public async Task<IActionResult> StoreRegister(StoreRegisterDTO store)
		{
			try
			{
                var rs = InputValidation.StoreRegisterValidation(store.UserId, store.StoreName, store.StorePhone, store.StoreEmail, store.Address, store.TaxCode);
                if(rs != "")
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Result = false;
                    _response.ErrorMessages.Add(rs);
                    return BadRequest(_response);
                }
                var storeInDb = await _unitOfWork.StoreDescriptionService.GetFirst(c => c.UserId == store.UserId);
                if(storeInDb != null)
                {
					_response.StatusCode = HttpStatusCode.BadRequest;
					_response.Result = false;
					_response.ErrorMessages.Add("Người dùng nãy đã đăng ký trở thành cửa hàng!");
					return BadRequest(_response);
				}
				var userInDb = await _unitOfWork.UserService.GetFirst(c => c.UserId == store.UserId);
				if (userInDb != null)
				{
					var newStore = _mapper.Map<StoreDesciption>(store);
					newStore.Status = SD.not_verify;
					await _unitOfWork.StoreDescriptionService.Add(newStore);
					_response.IsSuccess = true;
					_response.StatusCode = HttpStatusCode.OK;
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

		[HttpPost]
        [Route("VerifyStore")]
        public async Task<ApiResponse> VerifyStore(int storeId)
        {
            if (storeId <= 0)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.ErrorMessages.Add("Lỗi hệ thống!");
                _response.IsSuccess = false;
                return _response;
            }
			var store = await _unitOfWork.StoreDescriptionService.GetFirst(x => x.StoreId == storeId);
            if (store == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.ErrorMessages.Add("Không tìm thấy cửa hàng!");
                _response.IsSuccess = false;
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
                    _response.Result = store;
                }
                else
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages.Add("Cửa hàng đã được xác minh!");
                    _response.IsSuccess = false;
                }
            }
            return _response;
        }
    }
}