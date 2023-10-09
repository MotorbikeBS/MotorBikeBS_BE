using API.DTO;
using API.DTO.MotorbikeDTO;
using API.Validation;
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
        [Authorize]
        public async Task<IActionResult> Get()
        {
            try
            {
                var list = await _unitOfWork.MotorModelService.Get(includeProperties: "Brand");
                var listResponse = _mapper.Map<List<ModelResponseDTO>>(list);
                if (list == null || list.Count() <= 0)
                {
                    _response.ErrorMessages.Add("Không tìm thấy mẫu nào!");
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

        [HttpGet("{id:int}")]
        [Authorize]
        public async Task<IActionResult> GetByModelId(int id)
        {
            try
            {
                var obj = await _unitOfWork.MotorModelService.GetFirst(e => e.ModelId == id, includeProperties: "Brand");
                var objResponse = _mapper.Map<List<ModelResponseDTO>>(obj);
                if (obj == null)
                {
                    _response.ErrorMessages.Add("Không tồn tại mãu này!");
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

        [HttpPut]
        [Authorize(Roles = "Store,Owner")]
        public async Task<IActionResult> UpdateModel([FromQuery] int id, ModelRegisterDTO p)
        {
            try
            {
                var rs = InputValidation.ValidateTitle(p, "mẫu xe"); ;
                if (rs != "")
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Result = false;
                    _response.ErrorMessages.Add(rs);
                    return BadRequest(_response);
                }
                var obj = await _unitOfWork.MotorModelService.GetFirst(e => e.ModelId == id);
                if (obj == null)
                {
                    _response.ErrorMessages.Add("Không tồn tại mẫu nào!");
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                else
                {
                    _mapper.Map(p, obj);
                    await _unitOfWork.MotorModelService.Update(obj);
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
        [Authorize(Roles = "Store,Owner")]
        [Route("ModelRegister")]
        public async Task<IActionResult> ModelRegister(int brandID, ModelRegisterDTO model)
        {
            try
            {
                var rs = InputValidation.ValidateTitle(p, "mẫu xe"); ;
                if (rs != "")
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Result = false;
                    _response.ErrorMessages.Add(rs);
                    return BadRequest(_response);
                }
                var brand = await _unitOfWork.MotorBrandService.GetFirst(c => c.BrandId == brandID);
                if (brand == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.ErrorMessages.Add("Không tồn tại hãng xe mục tiêu");
                    return NotFound(_response);
                }
                else
                {
                    var checkmodel = await _unitOfWork.MotorModelService.GetFirst(c => c.ModelName== model.ModelName);
                    if (checkmodel != null)
                    {
                        _response.IsSuccess = false;
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.ErrorMessages.Add("Mẫu xe \"" + model.ModelName + "\" đã tồn tại");
                        return BadRequest(_response);
                    }
                    else
                    {
                        var newModel = _mapper.Map<MotorbikeModel>(model);
                        await _unitOfWork.MotorModelService.Add(newModel);
                        _response.IsSuccess = true;
                        _response.StatusCode = HttpStatusCode.OK;
                        _response.Result = newModel;
                    }                    
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
