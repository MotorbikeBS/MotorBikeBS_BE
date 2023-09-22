using API.DTO;
using API.DTO.MotorbikeDTO;
using AutoMapper;
using Core.Models;
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

        [HttpGet("GetAllOnExchange")]
        public async Task<IActionResult> GetAllOnExchange()
        {
            try
            {
                var list = await _unitOfWork.MotorBikeService.Get(e => e.MotorStatus.Equals("POSTING"));
                if (list == null || list.Count() <= 0)
                {
                    _response.ErrorMessages.Add("Can not found any motor bike!");
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

        [HttpGet("GetMotorByStore")]
        public async Task<IActionResult> GetByStoreId(int StoreID)
        {
            try
            {
                var list = await _unitOfWork.MotorBikeService.Get(e => e.StoreId == StoreID);
                if (list == null || list.Count() <= 0)
                {
                    _response.ErrorMessages.Add("Can not found any motor bike!");
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

        [HttpGet("GetMotorByOwner")]
        public async Task<IActionResult> GetByOwnerId(int OwnerID)
        {
            try
            {
                var list = await _unitOfWork.MotorBikeService.Get(e => e.OwnerId == OwnerID);
                if (list == null || list.Count() <= 0)
                {
                    _response.ErrorMessages.Add("Can not found any motor bike!");
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
        public async Task<IActionResult> GetByMotorId(int id)
        {
            try
            {
                var obj = await _unitOfWork.MotorBikeService.GetFirst(e => e.MotorId == id);
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

        [HttpPost]
        [Route("MotorRegister")]
        public async Task<IActionResult> MotorRegister(MotorRegisterDTO motor)
        {
            try
            {
                var CertNum = await _unitOfWork.MotorBikeService.GetFirst(c => c.CertificateNumber == motor.CertificateNumber);
                if (CertNum != null)
                {
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("Certificate Number already exist!");
                    return BadRequest(_response);
                }
                else
                {
                    var newMotor = _mapper.Map<Motorbike>(motor);
                    await _unitOfWork.MotorBikeService.Add(newMotor);

                    _response.IsSuccess = true;
                    _response.Result = newMotor;
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
