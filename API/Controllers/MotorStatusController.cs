using API.DTO;
using API.DTO.MotorbikeDTO;
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
    public class MotorStatusController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private ApiResponse _response;
        private readonly IMapper _mapper;

        public MotorStatusController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _response = new ApiResponse();
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            try
            {
                var list = await _unitOfWork.MotorStatusService.Get();
                if (list == null || list.Count() <= 0)
                {
                    _response.ErrorMessages.Add("Không tìm thấy trạng thái nào!");
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

        [HttpGet("{id:int}")]
        [Authorize]
        public async Task<IActionResult> GetByStatusId(int id)
        {
            try
            {
                var obj = await _unitOfWork.MotorStatusService.GetFirst(e => e.MotorStatusId == id);
                if (obj == null)
                {
                    _response.ErrorMessages.Add("Không tìm thấy xe nào!");
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
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateStatus([FromQuery] int id, StatusRegisterDTO p)
        {
            try
            {
                var obj = await _unitOfWork.MotorStatusService.GetFirst(e => e.MotorStatusId == id);
                if (obj == null)
                {
                    _response.ErrorMessages.Add("Không tìm thấy trạng thái nào!");
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                else
                {
                    _mapper.Map(p, obj);
                    await _unitOfWork.MotorStatusService.Update(obj);
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
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("StatusRegister")]
        public async Task<IActionResult> StatusRegister(StatusRegisterDTO status)
        {
            try
            {
                var CertNum = await _unitOfWork.MotorStatusService.GetFirst(c => c.Title == status.Title);
                if (CertNum != null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages.Add("Trạng thái xe \"" + status.Title + "\" đã tồn tại");
                    return BadRequest(_response);
                }
                else
                {
                    var newstatus = _mapper.Map<MotorbikeStatus>(status);
                    await _unitOfWork.MotorStatusService.Add(newstatus);
                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Result = newstatus;
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
