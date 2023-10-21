using API.DTO;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Service;
using Service.UnitOfWork;

namespace API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ContractController : ControllerBase
	{
		private readonly IUnitOfWork _unitOfWork;
		private ApiResponse _response;
		private readonly IMapper _mapper;

		public ContractController(IUnitOfWork unitOfWork, IMapper mapper, IEmailSender emailSender)
		{
			_unitOfWork = unitOfWork;
			_response = new ApiResponse();
			_mapper = mapper;
		}

		[Authorize(Roles ="Store")]
		[HttpPost]
		[Route("CreateContract")]
		public async Task<IActionResult> CreateContract(int bookingId)
		{
			return Ok();
		}
	}
}
