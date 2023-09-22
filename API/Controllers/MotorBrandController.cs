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
    public class MotorBrandController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private ApiResponse _response;
        private readonly IMapper _mapper;

        public MotorBrandController(IUnitOfWork unitOfWork, IMapper mapper)
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
                var list = await _unitOfWork.MotorBrandService.Get();
                if (list == null || list.Count() <= 0)
                {
                    _response.ErrorMessages.Add("Can not found any brand!");
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
        public async Task<IActionResult> GetByBrandId(int id)
        {
            try
            {
                var obj = await _unitOfWork.MotorBrandService.GetFirst(e => e.BrandId == id);
                if (obj == null)
                {
                    _response.ErrorMessages.Add("Can not found any brand!");
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
        public async Task<IActionResult> UpdateBrand([FromQuery] int id, BrandRegisterDTO p)
        {
            try
            {
                var obj = await _unitOfWork.MotorBrandService.GetFirst(e => e.BrandId == id);
                if (obj == null || id != p.BrandId)
                {
                    _response.ErrorMessages.Add("Can not found any brand!");
                    _response.IsSuccess = false;
                    return NotFound(_response);
                }
                else
                {
                    obj = _mapper.Map<MotorbikeBrand>(p);
                    await _unitOfWork.MotorBrandService.Update(obj);
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
    }
}
