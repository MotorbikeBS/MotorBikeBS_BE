﻿using API.DTO;
using API.DTO.CommentDTO;
using API.DTO.MotorbikeDTO;
using API.DTO.RequestDTO;
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
using System.Linq;
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
        public DateTime VnDate = DateTime.Now.ToLocalTime();

        public CommentController(IHubContext<NotificationHub> notificationContext, IUnitOfWork unitOfWork, IMapper mapper, IEmailSender emailSender)
        {
            _notificationContext = notificationContext;
            _unitOfWork = unitOfWork;
            _response = new ApiResponse();
            _mapper = mapper;
        }

        [Authorize]
        [HttpGet("GetByCommentID")]
        public async Task<IActionResult> GetByCommentId(int CommentID)
        {
            try
            {
                var obj = await _unitOfWork.CommentService.GetFirst(e => e.CommentId == CommentID && e.Status == SD.Request_Accept, includeProperties: SD.GetCommentArray);
                if (obj == null)
                {
                    _response.ErrorMessages.Add("Không tìm thấy bình luận nào!");
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                else
                {
                    var objResponse = _mapper.Map<CommentResponseDTO>(obj);
                    if (objResponse.InverseReply != null)
                    {
                        objResponse.InverseReply = objResponse.InverseReply.Where(reply => reply.Status == "ACCEPT").ToList();
                    }
                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Result = objResponse;
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

        //[Authorize]
        //[HttpGet("GetByUserId_Sender")]
        //public async Task<IActionResult> GetByUserId_Sender(int UserID)
        //{
        //    try
        //    {
        //        var request = await _unitOfWork.RequestService.Get(e => e.ReceiverId == UserID || e.SenderId == UserID);
        //        List<Comment> allComments = new List<Comment>();

        //        foreach (Request p in request)
        //        {
        //            var comments = await _unitOfWork.CommentService.Get(e => e.RequestId == p.RequestId && e.Status == SD.Request_Accept
        //                                                                && e.UserId == UserID && e.ReplyId == null, includeProperties: SD.GetCommentArray);
        //            allComments.AddRange(comments);
        //        }
        //        if (allComments == null)
        //        {
        //            _response.ErrorMessages.Add("Không tìm thấy thông báo nào!");
        //            _response.IsSuccess = false;
        //            _response.StatusCode = HttpStatusCode.NotFound;
        //            return NotFound(_response);
        //        }
        //        else
        //        {
        //            var listComment = allComments.Where(comment => comment.InverseReply.Any(reply => reply.UserId == UserID)).ToList();
        //            var listResponse = _mapper.Map<List<CommentResponseDTO>>(listComment);
        //            _response.IsSuccess = true;
        //            _response.StatusCode = HttpStatusCode.OK;
        //            _response.Result = listResponse;
        //        }
        //        return Ok(_response);
        //    }
        //    catch (Exception ex)
        //    {
        //        _response.IsSuccess = false;
        //        _response.StatusCode = HttpStatusCode.BadRequest;
        //        _response.ErrorMessages = new List<string>()
        //        {
        //            ex.ToString()
        //        };
        //        return BadRequest(_response);
        //    }
        //}

        [Authorize]
        [HttpGet("AverageStar")]
        public async Task<IActionResult> AverageStar(int StoreID)
        {
            try
            {
                int? total = 0;
                var store = await _unitOfWork.StoreDescriptionService.GetFirst(e => e.StoreId == StoreID);

                if (store == null)
                {
                    _response.ErrorMessages.Add("Không tìm thấy thông tin cửa hàng nào!");
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                else
                {
                    var request = await _unitOfWork.RequestService.Get(e => e.ReceiverId == store.UserId || e.SenderId == store.UserId);
                    List<Comment> allComments = new List<Comment>();
                    foreach (Request p in request)
                    {
                        var comments = await _unitOfWork.CommentService.Get(e => e.RequestId == p.RequestId && e.Status == SD.Request_Accept 
                                                                                && e.UserId != store.UserId && e.ReplyId == null);
                        allComments.AddRange(comments);
                    }
                    if (allComments == null || allComments.Count == 0)
                    {
                        _response.ErrorMessages.Add("Không tìm thấy thông báo nào!");
                        _response.IsSuccess = false;
                        _response.StatusCode = HttpStatusCode.NotFound;
                        return NotFound(_response);
                    }
                    else
                    {
                        foreach (var comment in allComments)
                        {
                            total += comment.Rating;
                        }
                        double? Avg = allComments.Count > 0 ? (double)total / allComments.Count : 0;
                        _response.IsSuccess = true;
                        _response.StatusCode = HttpStatusCode.OK;
                        _response.Result = new { TotalComment = allComments.Count, AverageRating = Avg };
                        return Ok(_response);
                    }
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
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
                var request = await _unitOfWork.RequestService.Get(e => e.ReceiverId == store.UserId || e.SenderId == store.UserId);
                List<Comment> allComments = new List<Comment>();

                foreach (Request p in request)
                {
                    var comments = await _unitOfWork.CommentService.Get(e => e.RequestId == p.RequestId && e.Status == SD.Request_Accept && e.ReplyId == null, includeProperties: SD.GetCommentArray);
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
                    allComments = allComments.OrderByDescending(c => c.CommentId).ToList();
                    var listResponse = _mapper.Map<List<CommentResponseDTO>>(allComments);
                    foreach (var comment in listResponse)
                    {
                        if (comment.InverseReply != null)
                        {
                            comment.InverseReply = comment.InverseReply.Where(reply => reply.Status == "ACCEPT").ToList();
                        }
                    }
                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Result = listResponse;
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
        [HttpGet("GetHistoryCommentByRequestID")]
        public async Task<IActionResult> GetHistoryCommentByRequestID(int RequestID)
        {
            try
            {
                var request = await _unitOfWork.RequestService.Get(e => e.RequestId == RequestID);
                if (request == null)
                {
                    _response.ErrorMessages.Add("Không tìm thấy yêu cầu!");
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                List<Comment> allComments = new List<Comment>();

                foreach (Request p in request)
                {
                    var comments = await _unitOfWork.CommentService.Get(e => e.RequestId == p.RequestId && e.Status != SD.Request_Accept && e.ReplyId == null, includeProperties: "InverseReply");
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
                    allComments = allComments.OrderByDescending(c => c.CommentId).ToList();
                    var listResponse = _mapper.Map<List<CommentResponseDTO>>(allComments);
                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Result = listResponse;
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
        public async Task<IActionResult> CommentRegister( int RequestID, [FromForm] CommentRegisterDTO comment)
        {
            try
            {
                var rs = InputValidation.CommentValidation(comment, 0); 
                if (rs != "")
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Result = false;
                    _response.ErrorMessages.Add(rs);
                    return BadRequest(_response);
                }
                else
                {
                    var request = await _unitOfWork.RequestService.GetFirst(e => e.RequestId == RequestID);
                    if (request == null)
                    {
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.Result = false;
                        _response.ErrorMessages.Add("Chỉ được bình luận khi trải nghiệm chức năng hệ thống");
                        return BadRequest(_response);
                    }
                    DateTime localDate = DateTime.Now.ToLocalTime();
                    var userId = int.Parse(User.FindFirst("UserId")?.Value);
                    var newComment = _mapper.Map<Comment>(comment);
                    newComment.RequestId = RequestID;
                    newComment.Status = SD.Request_Accept;
                    newComment.UserId = userId;
                    newComment.CreateAt = localDate;
                    newComment.UpdateAt = null;
                    if (newComment.ReplyId == 0) newComment.ReplyId = null;
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

        [HttpPost]
        [Authorize]
        [Route("ReplyComment")]
        public async Task<IActionResult> ReplyComment([FromForm] CommentRegisterDTO comment, int ReplyId)
        {
            try
            {
                var rs = InputValidation.CommentValidation(comment, ReplyId);
                if (rs != "")
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Result = false;
                    _response.ErrorMessages.Add(rs);
                    return BadRequest(_response);
                }
                var commentReply = await _unitOfWork.CommentService.GetFirst(e => e.CommentId == ReplyId);
                if(commentReply == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Result = false;
                    _response.ErrorMessages.Add("Chưa chọn bình luận nào để trả lời!");
                    return BadRequest(_response);
                }
                else
                {
                    var request = await _unitOfWork.RequestService.GetFirst(e => e.RequestId == commentReply.RequestId);
                    if (request == null)
                    {
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.Result = false;
                        _response.ErrorMessages.Add("Chỉ được bình luận khi trải nghiệm chức năng hệ thống");
                        return BadRequest(_response);
                    }
                    var userId = int.Parse(User.FindFirst("UserId")?.Value);
                    var newComment = _mapper.Map<Comment>(comment);
                    newComment.RequestId = commentReply.RequestId;
                    newComment.Status = SD.Request_Accept;
                    newComment.UserId = userId;
                    newComment.CreateAt = VnDate;
                    newComment.UpdateAt = null;
                    newComment.ReplyId = ReplyId;
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

        [HttpPut]
        [Authorize]
        [Route("UpdateComment")]
        public async Task<IActionResult> UpdateComment(int CommentID, [FromForm] CommentRegisterDTO comment)
        {
            try
            {                
                var obj = await _unitOfWork.CommentService.GetFirst(e => e.CommentId == CommentID);
                if (obj == null)
                {
                    _response.ErrorMessages.Add("Không tìm thấy bình luận này!");
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                else
                {

                    var rs = InputValidation.CommentValidation(comment, obj.ReplyId);
                    if (rs != "")
                    {
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.Result = false;
                        _response.ErrorMessages.Add(rs);
                        return BadRequest(_response);
                    }
                    var userId = int.Parse(User.FindFirst("UserId")?.Value);
                    obj.Status = "UPDATE";
                    obj.UpdateAt = VnDate;
                    //Set oldComment to Delete and create new Comment
                    await _unitOfWork.CommentService.Update(obj);
                    var newComment = _mapper.Map<Comment>(comment);
                    newComment.RequestId = obj.RequestId;
                    newComment.Status = SD.Request_Accept;
                    newComment.UserId = userId;
                    newComment.CreateAt = VnDate;
                    newComment.UpdateAt = VnDate;
                    newComment.Status = SD.Request_Accept;
                    newComment.ReplyId = obj.ReplyId;
                    await _unitOfWork.CommentService.Add(newComment);
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
        [Authorize]
        [Route("DeleteComment")]
        public async Task<IActionResult> DeleteComment(int CommentID)
        {
            try
            {
                var obj = await _unitOfWork.CommentService.GetFirst(e => e.CommentId == CommentID);
                if (obj == null)
                {
                    _response.ErrorMessages.Add("Không tìm thấy bình luận này!");
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                else
                {
                    var userId = int.Parse(User.FindFirst("UserId")?.Value);
                    obj.Status = "DELETE";
                    obj.UpdateAt = VnDate;
                    //Set oldComment to Delete and create new Comment
                    await _unitOfWork.CommentService.Update(obj);
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
