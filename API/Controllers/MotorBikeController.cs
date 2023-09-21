using API.DTO;
using API.DTO.MotorbikeDTO;
using Core.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.UnitOfWork;
using System.Collections.Generic;
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
		[HttpGet]
		public async Task<ApiResponse> Get()
		{
			try
			{
				var list = await _unitOfWork.MotorBikeService.Get();
				if (list == null || list.Count() <= 0)
				{
					_response.ErrorMessages.Add("Can not found any motor bike!");
					_response.IsSuccess = false;
					_response.StatusCode = HttpStatusCode.NotFound;
				}
				else
				{
					_response.IsSuccess = true;
					_response.StatusCode = HttpStatusCode.OK;
					_response.Result = list;
				}
				return _response;
			}
			catch (Exception ex)
			{
				_response.IsSuccess = false;
				_response.StatusCode = HttpStatusCode.BadRequest;
				_response.ErrorMessages = new List<string>()
				{
					ex.ToString()
				};
				return _response;
			}
		}

        [HttpGet("GetMotorByStore")]
        public async Task<ApiResponse> GetByStoreId(int StoreID)
        {
            try
            {
                var list = await _unitOfWork.MotorBikeService.Get(e => e.StoreId == StoreID);
                if (list == null || list.Count() <= 0)
                {
                    _response.ErrorMessages.Add("Can not found any motor bike!");
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                }
                else
                {
                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Result = list;
                }
                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages = new List<string>()
                {
                    ex.ToString()
                };
                return _response;
            }
        }

        [HttpGet("GetMotorByOwner")]
        public async Task<ApiResponse> GetByOwnerId(int OwnerID)
        {
            try
            {
                var list = await _unitOfWork.MotorBikeService.Get(e => e.OwnerId == OwnerID);
                if (list == null || list.Count() <= 0)
                {
                    _response.ErrorMessages.Add("Can not found any motor bike!");
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                }
                else
                {
                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Result = list;
                }
                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages = new List<string>()
                {
                    ex.ToString()
                };
                return _response;
            }
        }

        [HttpGet("{id:int}")]
		public async Task<ApiResponse> GetByMotorId(int id)
		{
			try
			{
				var obj = await _unitOfWork.MotorBikeService.GetFirst(e => e.MotorId == id);
				if (obj == null)
				{
					_response.ErrorMessages.Add("Can not found any motor bike!");
					_response.IsSuccess = false;
					_response.StatusCode = HttpStatusCode.NotFound;
				}
				else
				{
					_response.IsSuccess = true;
					_response.StatusCode = HttpStatusCode.OK;
					_response.Result = obj;
				}
				return _response;
			}
			catch (Exception ex)
			{
				_response.IsSuccess = false;
				_response.StatusCode = HttpStatusCode.BadRequest;
				_response.ErrorMessages = new List<string>()
				{
					ex.ToString()
				};
				return _response;
			}
		}

		//[HttpPost]
		//[Route("MotorRegister")]
		//public async Task<ApiResponse> MotorRegister(MotorRegisterDTO motor)
		//{
		//	try
		//	{
		//		var CertNum = await _unitOfWork.MotorBikeService.GetFirst(c => c.CertificateNumber == motor.CertificateNumber);
		//		if (CertNum != null)
		//		{
		//			_response.IsSuccess = false;
		//			_response.StatusCode = HttpStatusCode.BadRequest;
		//			_response.ErrorMessages.Add("Certificate Number already exist!");
		//		}
		//		else
		//		{
		//			var newMotor = _mapper.Map<Motorbike>(motor);
		//			await _unitOfWork.MotorBikeService.Add(newMotor);

		//			_response.IsSuccess = true;
		//			_response.StatusCode = HttpStatusCode.OK;
		//			_response.Result = newMotor;
		//		}
		//		return _response;
		//	}
		//	catch (Exception ex)
		//	{
		//		_response.IsSuccess = false;
		//		_response.StatusCode = HttpStatusCode.BadRequest;
		//		_response.ErrorMessages = new List<string>()
		//		{
		//			ex.ToString()
		//		};
		//		return _response;
		//	}
		//}
	}

}
