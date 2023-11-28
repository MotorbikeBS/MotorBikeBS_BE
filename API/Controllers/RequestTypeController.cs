using API.DTO;
using API.DTO.RequestDTO;
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
    public class RequestTypeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private ApiResponse _response;
        private readonly IMapper _mapper;

        public RequestTypeController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _response = new ApiResponse();
            _mapper = mapper;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var list = await _unitOfWork.RequestTypeService.Get();
                if (list == null || list.Count() <= 0)
                {
                    _response.ErrorMessages.Add("Không tìm thấy kiểu yêu cầu nào!");
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                else
                {
                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Result = list;
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
        public async Task<IActionResult> GetByRequestTypeId(int id)
        {
            try
            {
                var obj = await _unitOfWork.RequestTypeService.GetFirst(e => e.RequestTypeId == id);
                if (obj == null)
                {
                    _response.ErrorMessages.Add("Không tìm thấy kiểu yêu cầu này!");
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                else
                {
                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Result = obj;
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
        //[Authorize(Roles = "Admin")]
        //public async Task<IActionResult> UpdateRequestType([FromQuery] int id, Type_RequestRegisterDTO p)
        //{
        //    try
        //    {
        //        var obj = await _unitOfWork.RequestTypeService.GetFirst(e => e.RequestTypeId == id);
        //        if (obj == null)
        //        {
        //            _response.ErrorMessages.Add("Không tìm thấy kiểu yêu cầu này!");
        //            _response.IsSuccess = false;
        //            _response.StatusCode = HttpStatusCode.NotFound;
        //            return NotFound(_response);
        //        }
        //        else
        //        {
        //            _mapper.Map(p, obj);
        //            await _unitOfWork.RequestTypeService.Update(obj);
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
        //[Authorize(Roles = "Admin")]
        //[Route("RequestTypeRegister")]
        //public async Task<IActionResult> RequestRegister(Type_RequestRegisterDTO request)
        //{
        //    try
        //    {
        //        var newRequestType = _mapper.Map<RequestType>(request);
        //        await _unitOfWork.RequestTypeService.Add(newRequestType);
        //        _response.IsSuccess = true;
        //        _response.StatusCode = HttpStatusCode.OK;
        //        _response.Result = newRequestType;
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
