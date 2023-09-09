using AutoMapper;
using BusinessObject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MotorBikeBS_API.DTO;
using Repository.Interface;
using System.Net;

namespace MotorBikeBS_API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class MotorBikeController : ControllerBase
	{
		private readonly IMotorBikeRepository _repo;
		private ApiResponse _response;
		private readonly IMapper _mapper;
		public MotorBikeController(IMotorBikeRepository repo, IMapper mapper)
		{
			_repo = repo;
			_mapper = mapper;
			_response = new ApiResponse();
		}
		[HttpGet]
		public async Task<ApiResponse> Get()
		{
			List<Motorbike> products = await _repo.Get();
			if (products == null || products.Count == 0)
			{
				_response.StatusCode = HttpStatusCode.NotFound;
				_response.IsSuccess = false;
				_response.ErrorMessages.Add("Can't found any motor!");
			}
			else
			{
				_response.Result = products;
				_response.IsSuccess = true;
				_response.StatusCode = HttpStatusCode.OK;
			}
			return _response;
		}

		[HttpGet("{id:int}")]
		public async Task<ApiResponse> Get(int id)
		{
			Motorbike b = await _repo.GetById(id);
			if (b == null)
			{
				_response.StatusCode = HttpStatusCode.NotFound;
				_response.IsSuccess = false;
				_response.ErrorMessages.Add("Can't found any motor!");
			}
			else
			{
				_response.Result = b;
				_response.IsSuccess = true;
				_response.StatusCode = HttpStatusCode.OK;
			}
			return _response;
		}

		[HttpPost]
		public async Task<ApiResponse> Post([FromBody] MotobikeCreateDTO obj)
		{
			if (ModelState.IsValid)
			{
				try
				{
					var motor = _mapper.Map<Motorbike>(obj);
					await _repo.Create(motor);
					_response.Result = motor;
					_response.IsSuccess = true;
					_response.StatusCode = HttpStatusCode.OK;
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
			_response.IsSuccess = false;
			_response.StatusCode = HttpStatusCode.BadRequest;
			_response.ErrorMessages.Add($"{ModelState.Values.Select(e => e.Errors).ToList()}");
			return _response;

		}

		[HttpDelete("{id}")]
		public async Task<ApiResponse> Delete(int id)
		{
			try
			{
				await _repo.Delete(id);
				_response.IsSuccess = true;
				_response.StatusCode = HttpStatusCode.OK;
				return _response;
			}
			catch (Exception ex)
			{
				_response.IsSuccess = false;
				_response.StatusCode = HttpStatusCode.NotFound;
				_response.ErrorMessages.Add("Id not found");
				return _response;
			}
		}

		[HttpPut("{id:int}")]
		public async Task<ApiResponse> Put(int id, [FromBody] MotobikeCreateDTO obj)
		{
			if (ModelState.IsValid)
			{
				try
				{
					var motor = await _repo.GetById(id);
					if (motor == null)
					{
						_response.IsSuccess = false;
						_response.StatusCode = HttpStatusCode.NotFound;
					}
					else
					{
						var motorEdit = _mapper.Map<Motorbike>(obj);
						motorEdit.MotorId = id;
						await _repo.Update(motorEdit);
						_response.IsSuccess = true;
						_response.StatusCode = HttpStatusCode.OK;
						_response.Result = motorEdit;
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
			_response.IsSuccess = false;
			_response.StatusCode = HttpStatusCode.BadRequest;
			_response.ErrorMessages.Add($"{ModelState.Values.Select(e => e.Errors).ToList()}");
			return _response;
		}
	}
}
