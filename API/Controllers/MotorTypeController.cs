using API.DTO;
using AutoMapper;
using Core.Models;
using Microsoft.AspNetCore.Mvc;
using Service.UnitOfWork;
using System.Net;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MotorTypeController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private ApiResponse _response;
        private readonly IMapper _mapper;

        public MotorTypeController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _response = new ApiResponse();
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var list = await _unitOfWork.MotorTypeService.Get();
                if (list == null || list.Count() <= 0)
                {
                    _response.ErrorMessages.Add("Không tìm thấy Danh mục nào!");
                    _response.IsSuccess = false;
                    return NotFound(_response);
                }
                else
                {
                    _response.IsSuccess = true;
                    _response.Result = list;
                }
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>()
                {
                    ex.ToString()
                };
                return BadRequest(_response);
            }
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetByTypeId(int id)
        {
            try
            {
                var obj = await _unitOfWork.MotorTypeService.GetFirst(e => e.MotorTypeId == id);
                if (obj == null)
                {
                    _response.ErrorMessages.Add("Không tìm thấy xe nào!");
                    _response.IsSuccess = false;
                    return NotFound(_response);
                }
                else
                {
                    _response.IsSuccess = true;
                    _response.Result = obj;
                }
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>()
                {
                    ex.ToString()
                };
                return BadRequest(_response);
            }
        }
        [HttpPut]
        public async Task<IActionResult> UpdateType([FromQuery] int id, TypeRegisterDTO p)
        {
            try
            {
                var obj = await _unitOfWork.MotorTypeService.GetFirst(e => e.MotorTypeId == id);
                if (obj == null || id != p.MotorTypeId)
                {
                    _response.ErrorMessages.Add("Không tìm thấy Danh mục nào!");
                    _response.IsSuccess = false;
                    return NotFound(_response);
                }
                else
                {
                    obj = _mapper.Map<MotorbikeType>(p);
                    await _unitOfWork.MotorTypeService.Update(obj);
                    _response.IsSuccess = true;
                    _response.Result = obj;
                }
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>()
                {
                    ex.ToString()
                };
                return BadRequest(_response);
            }
        }
        [HttpPost]
        [Route("TypeRegister")]
        public async Task<IActionResult> TypeRegister(TypeRegisterDTO type)
        {
            try
            {
                var CertNum = await _unitOfWork.MotorTypeService.GetFirst(c => c.Title == type.Title);
                if (CertNum != null)
                {
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("Danh mục xe \"" + type.Title +"\" đã tồn tại");
                    return BadRequest(_response);
                }
                else
                {
                    var newType = _mapper.Map<MotorbikeType>(type);
                    await _unitOfWork.MotorTypeService.Add(newType);
                    _response.IsSuccess = true;
                    _response.Result = newType;
                    return Ok(_response);
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>()
                {
                    ex.ToString()
                };
                return BadRequest(_response);
            }
        }
    }
}
