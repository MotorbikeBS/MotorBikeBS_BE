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
    public class MotorModelController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private ApiResponse _response;
        private readonly IMapper _mapper;

        public MotorModelController(IUnitOfWork unitOfWork, IMapper mapper)
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
                var list = await _unitOfWork.MotorModelService.Get();
                if (list == null || list.Count() <= 0)
                {
                    _response.ErrorMessages.Add("Can not found any Model!");
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
        public async Task<IActionResult> GetByModelId(int id)
        {
            try
            {
                var obj = await _unitOfWork.MotorModelService.GetFirst(e => e.ModelId == id);
                if (obj == null)
                {
                    _response.ErrorMessages.Add("Can not found any motor bike!");
                    _response.IsSuccess = false;
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
                _response.ErrorMessages = new List<string>()
                {
                    ex.ToString()
                };
                return BadRequest(_response);
            }
        }
        [HttpPut]
        public async Task<IActionResult> UpdateModel([FromQuery] int id, ModelRegisterDTO p)
        {
            try
            {
                var obj = await _unitOfWork.MotorModelService.GetFirst(e => e.ModelId == id);
                if (obj == null || id != p.ModelId)
                {
                    _response.ErrorMessages.Add("Can not found any Model!");
                    _response.IsSuccess = false;
                    return NotFound(_response);
                }
                else
                {
                    obj = _mapper.Map<MotorbikeModel>(p);
                    await _unitOfWork.MotorModelService.Update(obj);
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
