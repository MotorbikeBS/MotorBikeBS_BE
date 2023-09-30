using API.DTO;
using API.DTO.MotorbikeDTO;
using API.DTO.UserDTO;
using AutoMapper;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.UnitOfWork;
using System.Net;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MotorBikeController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private ApiResponse _response;
        private readonly IMapper _mapper;

        public MotorBikeController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _response = new ApiResponse();
            _mapper = mapper;
        }

        [HttpGet("GetAllWithSpecificStatus")]
        public async Task<IActionResult> GetAllWithSpecificStatus(int StatusID )
        {
            try
            {
                var list = await _unitOfWork.MotorBikeService.Get(e => e.MotorStatusId == StatusID);
                if (list == null || list.Count() <= 0)
                {
                    _response.ErrorMessages.Add("Không tìm thấy xe nào!");
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
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

        [HttpGet("GetAllOnExchange")]
        public async Task<IActionResult> GetAllOnExchange()
        {
            try
            {
                var Status = await _unitOfWork.MotorStatusService.GetFirst(e => e.Title.Equals("POSTING"));
                var StatusDTO = _mapper.Map<StatusRegisterDTO>(Status);
                var list = await _unitOfWork.MotorBikeService.Get(e => e.MotorStatusId == StatusDTO.MotorStatusId);
                if (list == null || list.Count() <= 0)
                {
                    _response.ErrorMessages.Add("Không tìm thấy xe nào!");
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
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

        [Authorize(Roles = "Store")]
        [HttpGet("GetAllOnStoreExchange")]
        public async Task<IActionResult> GetAllOnStoreExchange()
        {
            try
            {
                var Status = await _unitOfWork.MotorStatusService.GetFirst(e => e.Title.Equals("SALE_REQUEST"));
                var list = await _unitOfWork.MotorBikeService.Get(e => e.MotorStatusId == Status.MotorStatusId);
                if (list == null || list.Count() <= 0)
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

        [HttpGet("GetMotorByStoreId")]
        public async Task<IActionResult> GetByStoreId(int StoreID)
        {
            try
            {
                var list = await _unitOfWork.MotorBikeService.Get(e => e.StoreId == StoreID);
                if (list == null || list.Count() <= 0)
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

        [HttpGet("GetMotorByOwner")]
        public async Task<IActionResult> GetByOwnerId(int OwnerID)
        {
            try
            {
                var list = await _unitOfWork.MotorBikeService.Get(e => e.OwnerId == OwnerID);
                if (list == null || list.Count() <= 0)
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
        public async Task<IActionResult> GetByMotorId(int id)
        {
            try
            {
                var obj = await _unitOfWork.MotorBikeService.GetFirst(e => e.MotorId == id);
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
        [Authorize(Roles = "Store,Owner")]
        [Route("UpdateMotor")]
        public async Task<IActionResult> UpdateMotor(int id, MotorRegisterDTO p)
        {
            try
            {
                var obj = await _unitOfWork.MotorBikeService.GetFirst(e => e.MotorId == id);
                if (obj == null)
                {
                    _response.ErrorMessages.Add("Không tìm thấy xe này!");
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                else
                {
                    _mapper.Map(p, obj);
                    await _unitOfWork.MotorBikeService.Update(obj);

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
        [Route("MotorRegister")]
        public async Task<IActionResult> MotorRegister(MotorRegisterDTO motor)
        {
            try
            {
                var CertNum = await _unitOfWork.MotorBikeService.GetFirst(c => c.CertificateNumber == motor.CertificateNumber);
                if (CertNum != null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages.Add("Mã số chứng nhận đăng ký xe không đúng!");
                    return BadRequest(_response);
                }
                else
                {
                    var newMotor = _mapper.Map<Motorbike>(motor);
                    await _unitOfWork.MotorBikeService.Add(newMotor);

                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Result = newMotor;
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
    }

}
