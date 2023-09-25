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
                    _response.ErrorMessages.Add("Không tìm thấy ảnh nào!");
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
                    _response.ErrorMessages.Add("Không tồn tại xe này!");
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
                    _response.ErrorMessages.Add("Không tồn tại xe này!");
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
        [HttpPost]
        [Route("ImageRegister")]
        public async Task<IActionResult> ImageRegister(int id, List<ImageRegisterDTO> images)
        {
            try
            {
                var CertNum = await _unitOfWork.MotorBikeService.GetFirst(c => c.MotorId == id);
                if (CertNum != null)
                {
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("Không tồn tại xe này!");
                    return BadRequest(_response);
                }
                else
                {
                    foreach (var p in images)
                    {
                        var newImage = _mapper.Map<MotorbikeImage>(p);
                        newImage.MotorId = id;
                        await _unitOfWork.MotorImageService.Add(newImage);
                        _response.IsSuccess = true;
                        _response.Result = newImage;
                    }
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
