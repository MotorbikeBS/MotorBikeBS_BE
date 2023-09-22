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
    public class MotorImageController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private ApiResponse _response;
        private readonly IMapper _mapper;

        public MotorImageController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _response = new ApiResponse();
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetByMotorId(int id)
        {
            try
            {
                var list = await _unitOfWork.MotorImageService.Get(e => e.MotorId == id);
                if (list == null || list.Count() <= 0)
                {
                    _response.ErrorMessages.Add("Can not found any Image!");
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
        public async Task<IActionResult> GetByImageId(int id)
        {
            try
            {
                var obj = await _unitOfWork.MotorImageService.GetFirst(e => e.ImageId == id);
                if (obj == null)
                {
                    _response.ErrorMessages.Add("Can not found any motor bike!");
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
        public async Task<IActionResult> UpdateImage([FromQuery] int id, ImageRegisterDTO p)
        {
            try
            {
                var obj = await _unitOfWork.MotorImageService.GetFirst(e => e.ImageId == id);
                if (obj == null || id != p.ImageId)
                {
                    _response.ErrorMessages.Add("Can not found any image!");
                    _response.IsSuccess = false;
                    return NotFound(_response);
                }
                else
                {
                    obj = _mapper.Map<MotorbikeImage>(p);
                    await _unitOfWork.MotorImageService.Update(obj);
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
