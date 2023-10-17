using API.DTO;
using API.DTO.MotorbikeDTO;
using API.Utility;
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

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var list = await _unitOfWork.MotorBrandService.Get(includeProperties: "MotorbikeModels");
                if (list == null || list.Count() <= 0)
                {
                    _response.ErrorMessages.Add("Không tìm thấy hãng nào!");
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
        public async Task<IActionResult> GetByBrandId(int id)
        {
            try
            {
                var obj = await _unitOfWork.MotorBrandService.GetFirst(e => e.BrandId == id, includeProperties: "MotorbikeModels");
                if (obj == null)
                {
                    _response.ErrorMessages.Add("Không tìm thấy hãng nào!");
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

        [Authorize]
        [HttpGet("search-BrandName")]
        public async Task<IActionResult> SearchByBrandName(string brandName)
        {
            try
            {

                var obj = await _unitOfWork.MotorBrandService.Get(expression: m => m.BrandName.Contains(brandName));
                if (obj != null && obj.Any())
                {
                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Result = obj;
                    return Ok(_response);
                }
                else
                {
                    _response.ErrorMessages.Add("Không tìm thấy hãng nào!");
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages = new List<string> { ex.ToString() };
                return BadRequest(_response);
            }
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateBrand([FromQuery] int id, BrandRegisterDTO Brand)
        {
            try
            {
                var rs = InputValidation.ValidateTitle(Brand, "hãng xe"); ;
                if (rs != "")
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Result = false;
                    _response.ErrorMessages.Add(rs);
                    return BadRequest(_response);
                }
                var obj = await _unitOfWork.MotorBrandService.GetFirst(e => e.BrandId == id);
                if (obj == null)
                {
                    _response.ErrorMessages.Add("Không tìm thấy hãng này!");
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                else
                {
                    _mapper.Map(Brand, obj);
                    var roleId = int.Parse(User.FindFirst("RoleId")?.Value);
                    InputValidation.StatusIfAdmin(Brand, roleId);
                    await _unitOfWork.MotorBrandService.Update(obj);
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
        [Authorize(Roles = "Store,Owner,Admin")]
        [Route("BrandRegister")]
        public async Task<IActionResult> BrandRegister(BrandRegisterDTO Brand)
        {
            try
            {
                var rs = InputValidation.ValidateTitle(Brand, "hãng xe"); ;
                if (rs != "")
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Result = false;
                    _response.ErrorMessages.Add(rs);
                    return BadRequest(_response);
                }
                var CertNum = await _unitOfWork.MotorBrandService.GetFirst(c => c.BrandName == Brand.BrandName);
                if (CertNum != null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages.Add("Hãng xe \"" + Brand.BrandName + "\" đã tồn tại");
                    return BadRequest(_response);
                }
                else
                {
                    var newBrand = _mapper.Map<MotorbikeBrand>(Brand);
                    var roleId = int.Parse(User.FindFirst("RoleId")?.Value);
                    InputValidation.StatusIfAdmin(newBrand, roleId);

                    await _unitOfWork.MotorBrandService.Add(newBrand);
                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Result = newBrand;
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
