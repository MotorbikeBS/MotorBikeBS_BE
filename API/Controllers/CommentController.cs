using API.DTO;
using API.Hubs;
using AutoMapper;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Service.Service;
using Service.UnitOfWork;
using System.Net;

namespace SignalRNotifications.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly IHubContext<NotificationHub> _notificationContext;
        private readonly IUnitOfWork _unitOfWork;
        private ApiResponse _response;
        private readonly IMapper _mapper;

        public CommentController(IHubContext<NotificationHub> notificationContext, IUnitOfWork unitOfWork, IMapper mapper, IEmailSender emailSender)
        {
            _notificationContext = notificationContext;
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
                var list = await _unitOfWork.NotificationService.Get(includeProperties: "NotificationType");
                if (list == null || list.Count() <= 0)
                {
                    _response.ErrorMessages.Add("Không tìm thấy thông báo nào!");
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
        public async Task<IActionResult> GetByCommentId(int id)
        {
            try
            {
                var obj = await _unitOfWork.NotificationService.GetFirst(e => e.NotificationId == id, includeProperties: "Reply");
                if (obj == null)
                {
                    _response.ErrorMessages.Add("Không tìm thấy bình luận nào!");
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
        [HttpGet("GetByUserID")]
        public async Task<IActionResult> GetByUserId(int id)
        {
            try
            {
                var obj = await _unitOfWork.NotificationService.Get(e => e.UserId == id, includeProperties: "Rely");
                if (obj == null)
                {
                    _response.ErrorMessages.Add("Không tìm thấy thông báo nào!");
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
    }
}
