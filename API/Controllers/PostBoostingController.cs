﻿using API.DTO;
using API.DTO.ContractDTO;
using API.DTO.PostBoostingDTO;
using API.Utility;
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
    public class PostBoostingController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private ApiResponse _response;
        private readonly IMapper _mapper;

        public PostBoostingController(IUnitOfWork unitOfWork, ApiResponse response, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _response = response;
            _mapper = mapper;
        }

        [Authorize(Roles = "Store")]
        [HttpPost]
        [Route("Boosting")]
        public async Task<IActionResult> Boosting(int motorId, PostBoostingCreateDTO dto)
        {
            try
            {
                var userId = int.Parse(User.FindFirst("UserId")?.Value);
                var rs = InputValidation.PostBoostingValidation(dto.StartTime, dto.EndTime, dto.Level);
                if(!string.IsNullOrEmpty(rs))
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages.Add(rs);
                    return BadRequest(_response);
                }
                var motor = await _unitOfWork.MotorBikeService.GetFirst(x => x.MotorId == motorId);
                
                if(motor == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages.Add("Không tìm thấy xe máy!");
                    return BadRequest(_response);
                }
                if(motor.StoreId == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages.Add("Không thể đẩy bài cho xe này");
                    return BadRequest(_response);
                }
                var store = await _unitOfWork.StoreDescriptionService.GetFirst(x => x.UserId == userId);

                if (store == null || motor.StoreId != store.StoreId)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages.Add("Đây không phải xe của cửa hàng này!");
                    return BadRequest(_response);
                }

                //var check = await _unitOfWork.RequestService.Get(x => x.SenderId == userId
                //&& x.RequestTypeId == SD.Request_Post_Boosting_Id
                //&& x.Po

                int pointPerDay = 3;

                int dayOfBoost = (dto.EndTime - dto.StartTime).Days;

                var totalCost = pointPerDay * dayOfBoost;

                if(totalCost > store.Point)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages.Add("Bạn không đủ điểm, vui lòng nạp thêm!");
                    return BadRequest(_response);
                }

                Request request = new()
                {
                    MotorId = motorId,
                    SenderId = userId,
                    Time = DateTime.Now,
                    RequestTypeId = SD.Request_Post_Boosting_Id,
                    //Status
                };

                await _unitOfWork.RequestService.Add(request);

                PointHistory point = new()
                {
                    RequestId = request.RequestId,
                    Qty = 1,
                    PointUpdatedAt = DateTime.Now,
                    Description = "Đẩy bài đăng",
                    //Action
                    StoreId = motor.StoreId.Value
                };

                await _unitOfWork.PointHistoryService.Add(point);

                PostBoosting postBoosting = new()
                {
                    StartTime = dto.StartTime,
                    EndTime = dto.EndTime,
                    Level = dto.Level,
                    MotorId = motorId,
                    HistoryId = point.PHistoryId,
                    Status = SD.Request_Accept
                };

                await _unitOfWork.PostBoostingService.Add(postBoosting);

                store.Point = store.Point - totalCost;

                await _unitOfWork.StoreDescriptionService.Update(store);

                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Message = "Đẩy bài thành công!";
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