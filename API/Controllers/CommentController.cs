using API.DTO;
using API.DTO.CommentDTO;
using API.DTO.MotorbikeDTO;
using API.Hubs;
using API.Utility;
using API.Validation;
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
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetByCommentId(int CommentID)
        {
            try
            {
                var obj = await _unitOfWork.CommentService.GetFirst(e => e.CommentId == CommentID && e.Status == SD.Request_Accept, includeProperties: "Reply");
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
        public async Task<IActionResult> GetByUserId_Sender(int UserID)
        {
            try
            {
                var request = await _unitOfWork.RequestService.Get(e => e.SenderId == UserID);
                List<Comment> allComments = new List<Comment>();

                foreach (Request p in request)
                {
                    var comments = await _unitOfWork.CommentService.Get(e => e.RequestId == p.RequestId && e.Status == SD.Request_Accept, includeProperties: "Rely");
                    allComments.AddRange(comments);
                }
                if (allComments == null)
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
                    _response.Result = allComments;
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
        [HttpGet("GetByStoreId_Receiver")]
        public async Task<IActionResult> GetByStoreId_Receiver(int StoreID)
        {
            try
            {
                var store = await _unitOfWork.StoreDescriptionService.GetFirst(e => e.StoreId == StoreID);
                if (store == null)
                {
                    _response.ErrorMessages.Add("Không tìm thấy thông tin cửa hàng nào!");
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                var request = await _unitOfWork.RequestService.Get(e => e.ReceiverId == store.UserId);
                List<Comment> allComments = new List<Comment>();

                foreach (Request p in request)
                {
                    var comments = await _unitOfWork.CommentService.Get(e => e.RequestId == p.RequestId && e.Status == SD.Request_Accept, includeProperties: "Rely");
                    allComments.AddRange(comments);
                }
                if (allComments == null)
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
                    _response.Result = allComments;
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
        [Authorize]
        [Route("CommentRegister")]
        public async Task<IActionResult> CommentRegister(CommentRegisterDTO comment)
        {
            try
            {
                var rs = InputValidation.CommentValidation(comment); 
                if (rs != "")
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Result = false;
                    _response.ErrorMessages.Add(rs);
                    return BadRequest(_response);
                }
                else
                {
                    var request = await _unitOfWork.RequestService.Get(e => e.RequestId == comment.RequestId);
                    if (request == null)
                    {
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.Result = false;
                        _response.ErrorMessages.Add("Chỉ được bình luận khi trải nghiệm chức năng hệ thống");
                        return BadRequest(_response);
                    }
                    var newComment = _mapper.Map<Comment>(comment);
                    newComment.Status = SD.Request_Accept;
                    await _unitOfWork.CommentService.Add(newComment);
                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Result = newComment;
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
