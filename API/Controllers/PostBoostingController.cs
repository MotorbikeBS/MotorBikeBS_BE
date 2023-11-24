using API.DTO;
using API.DTO.BookingDTO;
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
        public DateTime VnDate = DateTime.Now.ToLocalTime();

        public PostBoostingController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _response = new ApiResponse();
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
                if (!string.IsNullOrEmpty(rs))
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages.Add(rs);
                    return BadRequest(_response);
                }
                var motor = await _unitOfWork.MotorBikeService.GetFirst(x => x.MotorId == motorId);

                if (motor == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages.Add("Không tìm thấy xe máy!");
                    return BadRequest(_response);
                }
                if (motor.StoreId == null)
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

                var checkDuplicate = await _unitOfWork.RequestService.Get(x => x.SenderId == userId
                && x.MotorId == motorId
                && x.RequestTypeId == SD.Request_Post_Boosting_Id
                && x.PointHistories
                .Any(y => y.PostBoostings
                .Any(z => z.StartTime.Value.Day < VnDate.Day
                && z.EndTime.Value.Day > VnDate.Day)));

                if (checkDuplicate.Count() > 0)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages.Add("Tin này đã được đẩy!");
                    return BadRequest(_response);
                }

                int pointPerDay;
                if (dto.Level == 1)
                {
                    pointPerDay = 1;
                }
                else if (dto.Level == 2)
                {
                    pointPerDay = 2;
                }
                else
                {
                    pointPerDay = 3;
                }

                int dayOfBoost = dto.EndTime.Day - dto.StartTime.Day;

                var totalCost = pointPerDay * dayOfBoost;

                if (totalCost > store.Point)
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
                    Time = VnDate,
                    RequestTypeId = SD.Request_Post_Boosting_Id,
                    Status = SD.Request_Accept
                };

                await _unitOfWork.RequestService.Add(request);

                PointHistory point = new()
                {
                    RequestId = request.RequestId,
                    Qty = totalCost,
                    PointUpdatedAt = VnDate,
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
                    Status = SD.Request_Accept,
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

        [Authorize(Roles = "Store")]
        [Route("ExtendBoosting")]
        [HttpPut]
        public async Task<IActionResult> ExtendBoosting(int boostingId, PostBoostingUpdateDTO dto)
        {
            try
            {
                var userId = int.Parse(User.FindFirst("UserId")?.Value);
                if (dto.EndTime == default(DateTime))
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages.Add("Vui lòng chọn ngày kết thúc");
                    return BadRequest(_response);
                }
                
                var boosting = await _unitOfWork.PostBoostingService.GetFirst(x => x.BoostId == boostingId);
                if (boosting == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages.Add("Không tìm thấy thông tin!");
                    return BadRequest(_response);
                }
                if(boosting.EndTime.Value.Day < VnDate.Day)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages.Add("Không thể gia hạn!");
                    return BadRequest(_response);
                }

                var store = await _unitOfWork.StoreDescriptionService.GetFirst(x => x.UserId == userId);

                var motor = await _unitOfWork.MotorBikeService.GetFirst(x => x.MotorId == boosting.MotorId);

                if (motor == null || motor.StoreId == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages.Add("Không tìm thấy xe máy!");
                    return BadRequest(_response);
                }

                if (store == null || motor.StoreId != store.StoreId)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages.Add("Đây không phải xe của cửa hàng này!");
                    return BadRequest(_response);
                }

                //var result = await _unitOfWork.RequestService.GetFirst(x => x.SenderId == userId
                //&& x.MotorId == boosting.MotorId
                //&& x.RequestTypeId == SD.Request_Post_Boosting_Id
                //&& x.Status == SD.Request_Accept
                //&& x.PointHistories
                //.Any(y => y.PostBoostings
                //.Any(z => z.StartTime < VnDate
                //&& z.EndTime > VnDate && z.BoostId == boostingId)));

                //if (result == null)
                //{
                //    _response.IsSuccess = false;
                //    _response.StatusCode = HttpStatusCode.BadRequest;
                //    _response.ErrorMessages.Add("Không thể gia hạn!");
                //    return BadRequest(_response);
                //}

                var pointHistory = await _unitOfWork.PointHistoryService.GetFirst(x => x.PHistoryId == boosting.HistoryId);

                //var boosting = await _unitOfWork.PostBoostingService.GetFirst(x => x.HistoryId == pointHistory.PHistoryId);
                //if (boosting == null)
                //{
                //    _response.IsSuccess = false;
                //    _response.StatusCode = HttpStatusCode.BadRequest;
                //    _response.ErrorMessages.Add("Không thể gia hạn!");
                //    return BadRequest(_response);
                //}

                if (dto.EndTime.Date <= boosting.EndTime.Value.Date)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages.Add("Ngày kết thúc phải sau ngày cũ!");
                    return BadRequest(_response);
                }

                int extendDay = dto.EndTime.Day - boosting.EndTime.Value.Day;

                int pointPerDay;
                if (boosting.Level == 1)
                {
                    pointPerDay = 1;
                }
                else if (boosting.Level == 2)
                {
                    pointPerDay = 2;
                }
                else
                    pointPerDay = 3;

                var totalCost = pointPerDay * extendDay;

                if (totalCost > store.Point)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages.Add("Bạn không đủ điểm, vui lòng nạp thêm!");
                    return BadRequest(_response);
                }

                pointHistory.Qty = pointHistory.Qty + totalCost;
                await _unitOfWork.PointHistoryService.Update(pointHistory);

                boosting.EndTime = dto.EndTime;
                await _unitOfWork.PostBoostingService.Update(boosting);

                store.Point = store.Point - totalCost;

                await _unitOfWork.StoreDescriptionService.Update(store);

                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Message = "Gia hạn thành công!";
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

        [Authorize(Roles = "Store")]
        [Route("ChangeLevel")]
        [HttpPut]
        public async Task<IActionResult> ChangeLevel(int boostingId, PostBoostingChangeDTO dto)
        {
            try
            {
                var userId = int.Parse(User.FindFirst("UserId")?.Value);
                if (dto.Level == default || dto.Level < 1 || dto.Level > 3)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages.Add("Vui lòng chọn gói hợp lệ!");
                    return BadRequest(_response);
                }

                var boosting = await _unitOfWork.PostBoostingService.GetFirst(x => x.BoostId == boostingId);
                if(boosting.EndTime.Value.Date < DateTime.Now.Date)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages.Add("Đã hết hạn, không thề đổi gói!");
                    return BadRequest(_response);
                }

                var store = await _unitOfWork.StoreDescriptionService.GetFirst(x => x.UserId == userId);

                var motor = await _unitOfWork.MotorBikeService.GetFirst(x => x.MotorId == boosting.MotorId);

                if (motor == null || motor.StoreId == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages.Add("Không tìm thấy xe máy!");
                    return BadRequest(_response);
                }

                if (store == null || motor.StoreId != store.StoreId)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages.Add("Đây không phải xe của cửa hàng này!");
                    return BadRequest(_response);
                }

                //var result = await _unitOfWork.RequestService.GetFirst(x => x.SenderId == userId
                //&& x.MotorId == motor.MotorId
                //&& x.RequestTypeId == SD.Request_Post_Boosting_Id
                //&& x.Status == SD.Request_Accept
                //&& x.PointHistories
                //.Any(y => y.PostBoostings
                //.Any(z => z.StartTime < VnDate
                //&& z.EndTime > VnDate)));

                //if (result == null)
                //{
                //    _response.IsSuccess = false;
                //    _response.StatusCode = HttpStatusCode.BadRequest;
                //    _response.ErrorMessages.Add("Không thể đổi gói đẩy bài!");
                //    return BadRequest(_response);
                //}

                var pointHistory = await _unitOfWork.PointHistoryService.GetFirst(x => x.PHistoryId == boosting.HistoryId);


                //if (dto.EndTime.Date < boosting.EndTime.Value.Date)
                //{
                //    _response.IsSuccess = false;
                //    _response.StatusCode = HttpStatusCode.BadRequest;
                //    _response.ErrorMessages.Add("Ngày kết thúc không được trước ngày cũ!");
                //    return BadRequest(_response);
                //}
                var oldLevel = 0;
                if(boosting.Level == 1)
                {
                    if(dto.Level == 1)
                    {
                        _response.IsSuccess = false;
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.ErrorMessages.Add("Bạn phải chọn gói cao hơn!");
                        return BadRequest(_response);
                    }
                    oldLevel = 1;
                }
                else if (boosting.Level == 2)
                {
                    if (dto.Level == 1 || dto.Level == 2)
                    {
                        _response.IsSuccess = false;
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.ErrorMessages.Add("Bạn phải chọn gói cao hơn!");
                        return BadRequest(_response);
                    }
                    oldLevel = 2;
                }
                else
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages.Add("Bạn đang sử dụng gói cao cấp nhất!");
                    return BadRequest(_response);
                }


                //int totalDay = (dto.EndTime - boosting.StartTime.Value).Days;
                int newTotalDay = boosting.EndTime.Value.Day - VnDate.Day;
                int oldTotalDay = VnDate.Day - boosting.StartTime.Value.Day;
                int pointPerDay;
                if (dto.Level == 2)
                {
                    pointPerDay = 2;
                }
                else
                    pointPerDay = 3;

                //var totalCost = pointPerDay * totalDay;

                var newLevelCost = pointPerDay * newTotalDay;
                var oldLevelCost = oldLevel * oldTotalDay;
                var extendCost = newLevelCost - oldLevelCost;

                if (extendCost > store.Point)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages.Add("Bạn không đủ điểm, vui lòng nạp thêm!");
                    return BadRequest(_response);
                }

                pointHistory.Qty = pointHistory.Qty + extendCost;
                await _unitOfWork.PointHistoryService.Update(pointHistory);

                boosting.Level = dto.Level;
                await _unitOfWork.PostBoostingService.Update(boosting);

                store.Point = store.Point - extendCost;

                await _unitOfWork.StoreDescriptionService.Update(store);

                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Message = "Đổi gói đẩy bài thành công!";
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

        [HttpGet]
        [Authorize(Roles ="Store")]
        [Route("BoostingHistory")]
        public async Task<IActionResult>BoostingHistory()
        {
            try
            {
                var userId = int.Parse(User.FindFirst("UserId")?.Value);
                var request = await _unitOfWork.RequestService.Get(x => x.SenderId == userId
                && x.RequestTypeId == SD.Request_Post_Boosting_Id
                && x.PointHistories.Any(y => y.PostBoostings.Any(z => z.MotorId != null)),
                includeProperties: new string[] { "PointHistories", "Motor", "Motor.MotorStatus", "Motor.MotorbikeImages", "PointHistories.PostBoostings" });

                var expired = await _unitOfWork.RequestService.Get(x => x.SenderId == userId
                && x.RequestTypeId == SD.Request_Post_Boosting_Id
                && x.PointHistories.Any(y => y.PostBoostings.Any(z => z.EndTime < VnDate 
                && z.Status == SD.Request_Accept)));

                if (expired.Count() > 0)
                {
                    foreach(var item in expired)
                    {
                        var point = await _unitOfWork.PointHistoryService.GetFirst(x => x.RequestId == item.RequestId);
                        var boosting = await _unitOfWork.PostBoostingService.GetFirst(x => x.HistoryId == point.PHistoryId);
                        boosting.Status = SD.Request_Cancel;
                        await _unitOfWork.PostBoostingService.Update(boosting);
                    }
                }

                if (request.Count()  > 0 )
                {
                    var response = _mapper.Map<List<BoostingRequestResponseDTO>>(request);
                    response.ForEach(item => item.Motor.MotorStatus.Motorbikes = null);
                    response.ForEach(item => item.Motor.Requests = null);

                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Result = response;
                    return Ok(_response);
                }
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.ErrorMessages.Add("Không có xe nào đã được đẩy bài!");
                return NotFound(_response);
            }
            catch(Exception ex)
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


