using API.DTO;
using API.DTO.ReportDTO;
using API.Utility;
using API.Validation;
using AutoMapper;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.BlobImageService;
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
        private readonly IBlobService _blobService;
        public DateTime VnDate = DateTime.Now.ToLocalTime();

        public ReportController(IUnitOfWork unitOfWork, IMapper mapper, IBlobService blobService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _response = new ApiResponse();
            _blobService = blobService;
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

                Request request = new()
                {
                    SenderId = userId,
                    ReceiverId = 1,
                    Time = VnDate,
                    RequestTypeId = SD.Request_Report_Id,
                    Status = SD.Request_Pending
                };

                await _unitOfWork.RequestService.Add(request);

                Report report = new()
                {
                    RequestId = request.RequestId,
                    Description = dto.Description,
                    Title = dto.Title,
                    Status = SD.Request_Pending
                };

                await _unitOfWork.ReportService.Add(report);

                foreach(var item in images)
                {
                    string file = $"{Guid.NewGuid()}{Path.GetExtension(item.FileName)}";
                    var img = await _blobService.UploadBlob(file, SD.Storage_Container, item);
                    ReportImage image = new()
                    {
                        ReportId = report.ReportId,
                        ImageLink = img
                    };
                    await _unitOfWork.ReportImageService.Add(image);
                }
                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Message = ("Báo cáo cửa hàng thành công!");
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
                return BadRequest();
            }
        }
    }
}
