using API.DTO;
using AutoMapper;
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
                    _response.ErrorMessages.Add("Can not found any store!");
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
        [Route("VerifyStore")]
        public async Task<ApiResponse> VerifyStore(int userId)
        {
            var user = await _unitOfWork.UserService.GetFirst(x => x.UserId == userId);
            if (userId <= 0 || user == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.ErrorMessages.Add("Error!");
                _response.IsSuccess = false;
                return _response;
            }
            var store = await _unitOfWork.StoreDescriptionService.GetFirst(x => x.UserId == userId);
            if (store == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.ErrorMessages.Add("Can not found any store!");
                _response.IsSuccess = false;
            }
            else
            {
                if (store.Status != "ACTIVE")
                {
                    store.Status = "ACTIVE";
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
                    _response.ErrorMessages.Add("The store has been verified!");
                    _response.IsSuccess = false;
                }
            }
            return _response;
        }
    }
}