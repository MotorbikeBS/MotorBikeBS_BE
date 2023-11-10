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
    public class NotificationController : ControllerBase
    {
        private readonly IHubContext<NotificationHub> _notificationContext;
        private readonly IUnitOfWork _unitOfWork;
        private ApiResponse _response;
        private readonly IMapper _mapper;

        public NotificationController(IHubContext<NotificationHub> notificationContext, IUnitOfWork unitOfWork, IMapper mapper, IEmailSender emailSender)
        {
            _notificationContext = notificationContext;
            _unitOfWork = unitOfWork;
            _response = new ApiResponse();
            _mapper = mapper;
        }

        //[HttpPost]
        //public HttpResponseMessage SendNotification(Notification obj)
        //{
        //    NotificationHub objNotifHub = new NotificationHub();
        //    Notification objNotif = new Notification();
        //    objNotif.UserId = obj.UserId;

        //    context.Configuration.ProxyCreationEnabled = false;
        //    context.Notifications.Add(objNotif);
        //    context.SaveChanges();

        //    objNotifHub.SendNotification(objNotif.SentTo);

        //    //var query = (from t in context.Notifications  
        //    //             select t).ToList();  

        //    return Request.CreateResponse(HttpStatusCode.OK);
        //}

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
        public async Task<IActionResult> GetByNotificationId(int id)
        {
            try
            {
                var obj = await _unitOfWork.NotificationService.GetFirst(e => e.NotificationId == id, includeProperties: "NotificationType");
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

        [Authorize]
        [HttpGet("GetByUserID")]
        public async Task<IActionResult> GetByUserId(int id)
        {
            try
            {
                var obj = await _unitOfWork.NotificationService.Get(e => e.UserId == id, includeProperties: "NotificationType");
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
        [Authorize]
        [HttpPut("markRead")]
        public async Task<IActionResult> markRead(int NotificationID)
        {
            try
            {
                var obj = await _unitOfWork.NotificationService.GetFirst(e => e.NotificationId == NotificationID, includeProperties: "NotificationType");
                if (obj == null)
                {
                    _response.ErrorMessages.Add("Không tìm thấy thông báo nào!");
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                else
                {
                    if (obj.IsRead == false || obj.IsRead == null)
                    {
                        obj.IsRead = true;
                    }
                    await _unitOfWork.NotificationService.Update(obj);
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
