using API.DTO;
using API.DTO.ReportDTO;
using API.Validation;
using AutoMapper;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.UnitOfWork;
using System.Net;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private ApiResponse _response;

        public ReportController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _response = new ApiResponse();
        }

        [Authorize(Roles =("Customer, Owner"))]
        [HttpPost]
        [Route("CreateReport")]
        public async Task<IActionResult>CreateReport(int storeId, [FromForm] ReportCreateDTO dto, List<IFormFile> images)
        {
            try
            {
                var rs = InputValidation.ReportValidation(storeId, dto.Title, dto.Description, images);
                if(!string.IsNullOrEmpty(rs))
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages.Add(rs);
                    return BadRequest(_response);
                }
                var userId = int.Parse(User.FindFirst("UserId")?.Value);
                var store = await _unitOfWork.StoreDescriptionService.GetFirst(x => x.StoreId == storeId);
                if(store == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.ErrorMessages.Add("Không tìm thấy cửa hàng!");
                    return NotFound(_response);
                }

                //Request request = new()
                //{
                //    SenderId = userId,
                //    ReceiverId = 1,

                //}
                return Ok();
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages = new List<string>()
                        {
                            ex.ToString()
                        };
                return BadRequest();
            }
        }
    }
}
