using API.DTO;
using API.DTO.MotorbikeDTO;
using API.DTO.RequestDTO;
using API.Utility;
using AutoMapper;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.UnitOfWork;
using System.Data;
using System.Net;
namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private ApiResponse _response;
        private readonly IMapper _mapper;

        public RequestController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _response = new ApiResponse();
            _mapper = mapper;
        }

        //[Authorize]
        //[HttpGet]
        //public async Task<IActionResult> Get()
        //{
        //    try
        //    {
        //        var listDatabase = await _unitOfWork.RequestService.Get(includeProperties: SD.GetRequestArray);
        //        var listResponse = _mapper.Map<List<RequestResponseDTO>>(listDatabase);
        //        if (listDatabase == null || listDatabase.Count() <= 0)
        //        {
        //            _response.ErrorMessages.Add("Không tìm thấy yêu cầu nào!");
        //            _response.IsSuccess = false;
        //            _response.StatusCode = HttpStatusCode.NotFound;
        //            return NotFound(_response);
        //        }
        //        else
        //        {
        //            _response.IsSuccess = true;
        //            _response.StatusCode = HttpStatusCode.OK;
        //            _response.Result = listResponse;
        //        }
        //        return Ok(_response);
        //    }
        //    catch (Exception ex)
        //    {
        //        _response.IsSuccess = false;
        //        _response.StatusCode = HttpStatusCode.BadRequest;
        //        _response.ErrorMessages = new List<string>()
        //        {
        //            ex.ToString()
        //        };
        //        return BadRequest(_response);
        //    }
        //}

        [Authorize]
        [HttpGet("GetBySenderID")]
        public async Task<IActionResult> GetBySenderID(int SenderID)
        {
            try
            {
                var listDatabase = await _unitOfWork.RequestService.Get(e => e.SenderId == SenderID, includeProperties: SD.GetRequestArray);
                var listResponse = _mapper.Map<List<RequestResponseDTO>>(listDatabase);
                if (listDatabase == null || listDatabase.Count() <= 0)
                {
                    _response.ErrorMessages.Add("Không tìm thấy yêu cầu nào!");
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                else
                {
                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Result = listResponse;
                }
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
                return BadRequest(_response);
            }
        }

        [Authorize]
        [HttpGet("GetByReceiverID")]
        public async Task<IActionResult> GetByReceiverID(int ReceiverID)
        {
            try
            {
                var listDatabase = await _unitOfWork.RequestService.Get(e => e.ReceiverId == ReceiverID, includeProperties: SD.GetRequestArray);
                var listResponse = _mapper.Map<List<RequestResponseDTO>>(listDatabase);
                if (listDatabase == null || listDatabase.Count() <= 0)
                {
                    _response.ErrorMessages.Add("Không tìm thấy yêu cầu nào!");
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                else
                {
                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Result = listResponse;
                }
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
                return BadRequest(_response);
            }
        }

        [Authorize]
        [HttpGet("GetRequestAssociated_WithStore")]
        public async Task<IActionResult> GetRequestAssociated_WithStore(int StoreID)
        {
            try
            {
                var StoreData = await _unitOfWork.StoreDescriptionService.GetFirst(e => e.StoreId == StoreID);
                if (StoreData == null)
                {
                    _response.ErrorMessages.Add("Không tìm thấy cửa hàng !");
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                var userId = int.Parse(User.FindFirst("UserId")?.Value);
                var listDatabase = await _unitOfWork.RequestService.Get(e => ( e.ReceiverId == userId && e.SenderId == StoreData.UserId )
                                                                          || ( e.ReceiverId == StoreData.UserId && e.SenderId == userId )
                                                                            , includeProperties: SD.GetRequestWithStoreArray);
                var listResponse = _mapper.Map<List<Store_RequestResponseDTO>>(listDatabase);
                if (listDatabase == null || listDatabase.Count() <= 0)
                {
                    _response.ErrorMessages.Add("Không tìm thấy yêu cầu nào!");
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                else
                {
                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Result = listResponse;
                }
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
                return BadRequest(_response);
            }
        }

        [Authorize]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetByRequestId(int id)
        {
            try
            {
                var obj = await _unitOfWork.RequestService.GetFirst(e => e.RequestId == id, SD.GetRequestArray);
                var objResponse = _mapper.Map<RequestResponseDTO>(obj);
                if (obj == null)
                {
                    _response.ErrorMessages.Add("Không tìm thấy yêu cầu này!");
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                else
                {
                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Result = objResponse;
                }
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
                return BadRequest(_response);
            }
        }

        //[HttpPut]
        //[Authorize]
        //public async Task<IActionResult> UpdateRequest([FromQuery] int id, RequestRegisterDTO p)
        //{
        //    try
        //    {
        //        var obj = await _unitOfWork.RequestService.GetFirst(e => e.RequestId == id);
        //        if (obj == null)
        //        {
        //            _response.ErrorMessages.Add("Không tìm thấy yêu cầu này!");
        //            _response.IsSuccess = false;
        //            _response.StatusCode = HttpStatusCode.NotFound;
        //            return NotFound(_response);
        //        }
        //        else
        //        {
        //            _mapper.Map(p, obj);
        //            await _unitOfWork.RequestService.Update(obj);
        //            _response.IsSuccess = true;
        //            _response.StatusCode = HttpStatusCode.OK;
        //            _response.Result = obj;
        //        }
        //        return Ok(_response);
        //    }
        //    catch (Exception ex)
        //    {
        //        _response.IsSuccess = false;
        //        _response.StatusCode = HttpStatusCode.BadRequest;
        //        _response.ErrorMessages = new List<string>()
        //        {
        //            ex.ToString()
        //        };
        //        return BadRequest(_response);
        //    }
        //}

        //[HttpPost]
        //[Authorize]
        //[Route("RequestRegister")]
        //public async Task<IActionResult> RequestRegister(RequestRegisterDTO request)
        //{
        //    try
        //    {
        //        var newRequest = _mapper.Map<Request>(request);
        //        await _unitOfWork.RequestService.Add(newRequest);
        //        _response.IsSuccess = true;
        //        _response.StatusCode = HttpStatusCode.OK;
        //        _response.Result = newRequest;
        //        return Ok(_response);
        //    }
        //    catch (Exception ex)
        //    {
        //        _response.IsSuccess = false;
        //        _response.StatusCode = HttpStatusCode.BadRequest;
        //        _response.ErrorMessages = new List<string>()
        //        {
        //            ex.ToString()
        //        };
        //        return BadRequest(_response);
        //    }
        //}

    }
}
