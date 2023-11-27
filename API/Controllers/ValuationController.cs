using API.DTO;
using API.DTO.BookingDTO;
using API.DTO.NegotiationDTO;
using API.DTO.UserDTO;
using API.Utility;
using API.Validation;
using AutoMapper;
using Azure.Core;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Service;
using Service.UnitOfWork;
using System.Linq;
using System.Net;
using Request = Core.Models.Request;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuationController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private ApiResponse _response;
        private readonly IMapper _mapper;
        public DateTime VnDate = DateTime.Now.ToLocalTime();

        public ValuationController(IUnitOfWork unitOfWork, IMapper mapper, IEmailSender emailSender)
        {
            _unitOfWork = unitOfWork;
            _response = new ApiResponse();
            _mapper = mapper;
        }

        [Authorize(Roles = ("Store"))]
        [HttpPost]
        [Route("StartValuation")]
        public async Task<IActionResult> StartValuation(int motorId, ValuationCreateDTO dto)
        {
            try
            {
                var rs = InputValidation.ValuationValidation(dto.StorePrice, dto.Description);
                if (!string.IsNullOrEmpty(rs))
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages.Add(rs);
                    return BadRequest(_response);
                }
                var userId = int.Parse(User.FindFirst("UserId")?.Value);
                var motor = await _unitOfWork.MotorBikeService.GetFirst(x => x.MotorId == motorId);
                if (motor == null)
                {
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("Không tìm thấy xe máy!");
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }


                IEnumerable<Request> list = await _unitOfWork.RequestService.Get(x => x.SenderId == userId
                && x.RequestTypeId == SD.Request_Negotiation_Id
                && x.MotorId == motorId
                && x.Status != SD.Request_Cancel && x.Status != SD.Request_Expired
                && x.Valuations.Any(y => y.Status != SD.Request_Cancel && y.Status != SD.Request_Expired));


                if (list.Count() > 0)
                {
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("Bạn đang trong quá trình làm việc với xe này!");
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                if (motor.MotorStatusId == SD.Status_Consignment || motor.MotorStatusId == SD.Status_nonConsignment)
                {
                    Request request = new()
                    {
                        MotorId = motorId,
                        ReceiverId = motor.OwnerId,
                        SenderId = userId,
                        Time = VnDate,
                        RequestTypeId = SD.Request_Negotiation_Id,
                        Status = SD.Request_Pending
                    };
                    await _unitOfWork.RequestService.Add(request);

                    var valuationCreate = _mapper.Map<Valuation>(dto);
                    valuationCreate.RequestId = request.RequestId;
                    valuationCreate.Status = SD.Request_Pending;
                    valuationCreate.StorePrice = dto.StorePrice;
                    valuationCreate.Description = dto.Description;

                    await _unitOfWork.ValuationService.Add(valuationCreate);

                    _response.IsSuccess = true;
                    _response.Message = "Định giá thành công vui lòng chờ người bán xác nhận!";
                    _response.StatusCode = HttpStatusCode.OK;
                    return Ok(_response);
                }
                else
                {
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("Bạn không thể định giá xe này!");
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
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
                return BadRequest();
            }
        }

        //[Authorize(Roles = ("Store"))]
        //[HttpPost]
        //[Route("AcceptDefaultPrice")]
        //public async Task<IActionResult> AcceptDefaultPrice(int motorId)
        //{
        //	try
        //	{
        //		var userId = int.Parse(User.FindFirst("UserId")?.Value);
        //		var motor = await _unitOfWork.MotorBikeService.GetFirst(x => x.MotorId == motorId);
        //		if (motor == null)
        //		{
        //			_response.IsSuccess = false;
        //			_response.ErrorMessages.Add("Không tìm thấy xe máy!");
        //			_response.StatusCode = HttpStatusCode.NotFound;
        //			return NotFound(_response);
        //		}

        //		IEnumerable<Request> list = await _unitOfWork.RequestService.Get(x => x.SenderId == userId
        //		&& x.RequestTypeId == SD.Request_Negotiation_Id
        //		&& x.MotorId == motorId
        //		&& x.Status == SD.Request_Pending);

        //		if (list.Count() > 0)
        //		{
        //			_response.IsSuccess = false;
        //			_response.ErrorMessages.Add("Bạn đã xác nhận mua. Vui lòng thể tiến hành đặt lịch.!");
        //			_response.StatusCode = HttpStatusCode.BadRequest;
        //			return BadRequest(_response);
        //		}
        //		if (motor.MotorStatusId == SD.Status_Consignment || motor.MotorStatusId == SD.Status_nonConsignment)
        //		{
        //			Request request = new()
        //			{
        //				MotorId = motorId,
        //				ReceiverId = motor.OwnerId,
        //				SenderId = userId,
        //				Time = VnDate,
        //				RequestTypeId = SD.Request_Negotiation_Id,
        //				Status = SD.Request_Pending
        //			};
        //			await _unitOfWork.RequestService.Add(request);

        //			var negotiationCreate = new Negotiation()
        //			{
        //				RequestId = request.RequestId,
        //				StartTime = VnDate,
        //				EndTime = VnDate,
        //				Status = SD.Request_Accept,
        //			};

        //			await _unitOfWork.NegotiationService.Add(negotiationCreate);

        //			_response.IsSuccess = true;
        //			_response.Message = "Thành công, vui lòng đặt lịch cho chủ xe!";
        //			_response.StatusCode = HttpStatusCode.OK;
        //			return Ok(_response);
        //		}
        //		else
        //		{
        //			_response.IsSuccess = false;
        //			_response.ErrorMessages.Add("Bạn không thể thương lượng xe này!");
        //			_response.StatusCode = HttpStatusCode.BadRequest;
        //			return BadRequest(_response);
        //		}
        //	}
        //	catch (Exception ex)
        //	{
        //		_response.IsSuccess = false;
        //		_response.StatusCode = HttpStatusCode.BadRequest;
        //		_response.ErrorMessages = new List<string>()
        //		{
        //			ex.ToString()
        //		};
        //		return BadRequest();
        //	}
        //}

        [Authorize(Roles = "Store, Owner")]
        [HttpGet]
        [Route("GetValuationRequest")]
        public async Task<IActionResult> GetValuationRequest()
        {
            try
            {
                var roleId = int.Parse(User.FindFirst("RoleId")?.Value);
                var userId = int.Parse(User.FindFirst("UserId")?.Value);

                IEnumerable<Request> requestNegotiation = null;

                if (roleId == SD.Role_Owner_Id)
                {
                    requestNegotiation = await _unitOfWork.RequestService.Get(x => x.ReceiverId == userId
                    && x.RequestTypeId == SD.Request_Negotiation_Id
                    && x.Status != SD.Request_Cancel
                    && x.Motor.MotorStatusId != SD.Status_Storage
                    && x.Motor.MotorStatusId != SD.Status_Posting
                    && x.Valuations.Any(n => n.Status != SD.Request_Cancel),
                    includeProperties: new string[] { "Valuations", "Motor", "Motor.MotorStatus", "Motor.MotorType", "Motor.MotorbikeImages", "Sender.StoreDesciptions" });
                }

                if (roleId == SD.Role_Store_Id)
                {
                    requestNegotiation = await _unitOfWork.RequestService.Get(x => x.SenderId == userId
                    && x.RequestTypeId == SD.Request_Negotiation_Id
                    && x.Status != SD.Request_Cancel
                    && x.Motor.MotorStatusId != SD.Status_Storage
                    && x.Motor.MotorStatusId != SD.Status_Posting
                    && x.Valuations.Any(n => n.Status != SD.Request_Cancel),
                    includeProperties: new string[] { "Valuations", "Motor", "Motor.MotorStatus", "Motor.MotorType", "Motor.MotorbikeImages", "Receiver" });
                }
                var negotiationResponse = _mapper.Map<List<ValuationResponseRequestDTO>>(requestNegotiation);

                //negotiationResponse.ForEach(item =>
                //{
                //	item.Negotiations = item.Negotiations.Where(n => n.EndTime == null).ToList();
                //});

                //if (negotiationResponse.Any(item => item.Negotiations == null || item.Negotiations.Count == 0))
                //{
                //	_response.IsSuccess = false;
                //	_response.ErrorMessages.Add("Hiện tại không có xe nào đang thương lượng!");
                //	_response.StatusCode = HttpStatusCode.NotFound;
                //	return NotFound(_response);


                if (negotiationResponse.Count() < 1)
                {
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("Hiện tại không có xe nào đang trong quá trình định giá!");
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                if (roleId == SD.Role_Store_Id)
                {
                    negotiationResponse.ForEach(item => item.Motor.Owner = null);
                }
                negotiationResponse.ForEach(item => item.Motor.Requests = null);
                negotiationResponse.ForEach(item => item.Motor.MotorStatus.Motorbikes = null);
                negotiationResponse.ForEach(item => item.Motor.MotorType.Motorbikes = null);


                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = negotiationResponse;
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

        //[Authorize(Roles = "Owner, Store")]
        //[HttpPut]
        //[Route("ChangePrice")]
        //public async Task<IActionResult> EditNegotiation(int NegotiationId, NegotiationUpdateDTO dto)
        //{
        //	try
        //	{
        //		var userId = int.Parse(User.FindFirst("UserId")?.Value);
        //		var roleId = int.Parse(User.FindFirst("RoleId")?.Value);
        //		var negotiationInDb = await _unitOfWork.NegotiationService.GetFirst(x => x.NegotiationId == NegotiationId);
        //		if (negotiationInDb == null)
        //		{
        //			_response.IsSuccess = false;
        //			_response.ErrorMessages.Add("Không tìm thấy yêu cầu này!");
        //			_response.StatusCode = HttpStatusCode.NotFound;
        //			return NotFound(_response);
        //		}
        //		var baseRequest = await _unitOfWork.RequestService.GetFirst(x => x.RequestId == negotiationInDb.RequestId);
        //		if (negotiationInDb.Status != SD.Request_Pending || baseRequest.Status != SD.Request_Pending)
        //		{
        //			_response.IsSuccess = false;
        //			_response.ErrorMessages.Add("Không thể thương lượng, quá trình này đã kết thúc!");
        //			_response.StatusCode = HttpStatusCode.BadRequest;
        //			return BadRequest(_response);
        //		}
        //		if(negotiationInDb.ExpiredTime < VnDate)
        //		{
        //			_response.IsSuccess = false;
        //			_response.ErrorMessages.Add("Không thể thương lượng, đã quá thời gian thương lượng, vui lòng hủy yêu cầu!");
        //			_response.StatusCode = HttpStatusCode.BadRequest;
        //			return BadRequest(_response);
        //		}
        //		if (dto.Price == null)
        //		{
        //			_response.IsSuccess = false;
        //			_response.StatusCode = HttpStatusCode.BadRequest;
        //			_response.ErrorMessages.Add("Vui lòng nhập giá mong muốn!");
        //			return BadRequest(_response);
        //		}
        //		if (dto.Price < 1000000 || dto.Price > 1000000000)
        //		{
        //			_response.IsSuccess = false;
        //			_response.StatusCode = HttpStatusCode.BadRequest;
        //			_response.ErrorMessages.Add("Vui lòng nhập giá hợp lệ, thấp nhất là 1.000.000VNĐ!");
        //			return BadRequest(_response);
        //		}

        //		var request = await _unitOfWork.RequestService.GetFirst(x => x.RequestId == negotiationInDb.RequestId && x.RequestTypeId == SD.Request_Negotiation_Id);
        //		if (roleId == SD.Role_Owner_Id)
        //		{
        //			if (userId != request.ReceiverId)
        //			{
        //				_response.IsSuccess = false;
        //				_response.ErrorMessages.Add("Bạn không có quyền này!");
        //				_response.StatusCode = HttpStatusCode.BadRequest;
        //				return BadRequest(_response);
        //			}
        //			//if(userId == negotiationInDb.LastChangeUserId)
        //			//{
        //			//	_response.IsSuccess = false;
        //			//	_response.ErrorMessages.Add("Kết quả trả giá trước đó chưa được phản hồi ! Vui lòng đợi phản hồi trước khi thực hiện trả giá kế tiếp!");
        //			//	_response.StatusCode = HttpStatusCode.BadRequest;
        //			//	return BadRequest(_response);
        //			//}
        //			//negotiationInDb.OwnerPrice = dto.Price;
        //		}
        //		else if (roleId == SD.Role_Store_Id)
        //		{
        //			if (userId != request.SenderId)
        //			{
        //				_response.IsSuccess = false;
        //				_response.ErrorMessages.Add("Bạn không có quyền này!");
        //				_response.StatusCode = HttpStatusCode.BadRequest;
        //				return BadRequest(_response);
        //			}
        //			//if (userId == negotiationInDb.LastChangeUserId)
        //			//{
        //			//	_response.IsSuccess = false;
        //			//	_response.ErrorMessages.Add("Kết quả trả giá trước đó chưa được phản hồi ! Vui lòng đợi phản hồi trước khi thực hiện trả giá kế tiếp!");
        //			//	_response.StatusCode = HttpStatusCode.BadRequest;
        //			//	return BadRequest(_response);
        //			//}
        //			//negotiationInDb.StorePrice = dto.Price;
        //		}
        //		//negotiationInDb.LastChangeUserId = userId;
        //		await _unitOfWork.NegotiationService.Update(negotiationInDb);
        //		_response.IsSuccess = true;
        //		_response.StatusCode = HttpStatusCode.OK;
        //		_response.Message = ("Trả giá thành công");
        //		return Ok(_response);

        //	}
        //	catch (Exception ex)
        //	{
        //		_response.IsSuccess = false;
        //		_response.StatusCode = HttpStatusCode.BadRequest;
        //		_response.ErrorMessages = new List<string>()
        //				{
        //					ex.ToString()
        //				};
        //		return BadRequest();
        //	}
        //}

        [Authorize(Roles = "Owner")]
        [HttpPut]
        [Route("Accept")]
        public async Task<IActionResult> Accept(int valuationId)
        {
            try
            {
                var userId = int.Parse(User.FindFirst("UserId")?.Value);
                var roleId = int.Parse(User.FindFirst("RoleId")?.Value);
                var valuationInDb = await _unitOfWork.ValuationService.GetFirst(x => x.ValuationId == valuationId);
                if (valuationInDb == null)
                {
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("Không tìm thấy yêu cầu này!");
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                var baseRequest = await _unitOfWork.RequestService.GetFirst(x => x.RequestId == valuationInDb.RequestId);
                if (valuationInDb.Status != SD.Request_Pending || baseRequest.Status != SD.Request_Pending)
                {
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("Không thể đồng ý yêu cầu, quá trình này đã kết thúc!");
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                //if (negotiationInDb.ExpiredTime < VnDate)
                //{
                //	_response.IsSuccess = false;
                //	_response.ErrorMessages.Add("Không thể đồng ý, đã quá thời gian thương lượng, vui lòng hủy yêu cầu!");
                //	_response.StatusCode = HttpStatusCode.BadRequest;
                //	return BadRequest(_response);
                //}
                if (userId != baseRequest.ReceiverId)
                {
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("Bạn không có quyền này!");
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                valuationInDb.Status = SD.Request_Accept;
                //negotiationInDb.EndTime = VnDate;
                await _unitOfWork.ValuationService.Update(valuationInDb);

                var motor = await _unitOfWork.MotorBikeService.GetFirst(x => x.MotorId == baseRequest.MotorId);

                Notification newUserNoti = new()
                {
                    RequestId = baseRequest.RequestId,
                    UserId = baseRequest.SenderId,
                    Title = "Chủ xe đã đồng ý thương lượng",
                    Content = "Chủ xe " + motor.MotorName + " đã đồng ý thương lượng.",
                    NotificationTypeId = SD.NotificationType_Valuation,
                    Time = VnDate,
                    IsRead = false
                };
                await _unitOfWork.NotificationService.Add(newUserNoti);

                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Message = ("Bạn đã đồng ý cửa hàng này thành công!");
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

        [Authorize(Roles = "Owner, Store")]
        [HttpPut]
        [Route("Cancel")]
        public async Task<IActionResult> Cancel(int valuationId)
        {
            try
            {
                var userId = int.Parse(User.FindFirst("UserId")?.Value);
                var roleId = int.Parse(User.FindFirst("RoleId")?.Value);
                var valuationInDb = await _unitOfWork.ValuationService.GetFirst(x => x.ValuationId == valuationId);
                if (valuationInDb == null)
                {
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("Không tìm thấy yêu cầu này!");
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                var baseRequest = await _unitOfWork.RequestService.GetFirst(x => x.RequestId == valuationInDb.RequestId);
                if (valuationInDb.Status != SD.Request_Pending || baseRequest.Status != SD.Request_Pending)
                {
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("Không thể từ chối yêu cầu, quá trình này đã kết thúc!");
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                if (roleId == SD.Role_Store_Id)
                {
                    if (userId != baseRequest.SenderId)
                    {
                        _response.IsSuccess = false;
                        _response.ErrorMessages.Add("Bạn không có quyền này!");
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        return BadRequest(_response);
                    }

                }
                if (roleId == SD.Role_Owner_Id)
                {
                    if (userId != baseRequest.ReceiverId)
                    {
                        _response.IsSuccess = false;
                        _response.ErrorMessages.Add("Bạn không có quyền này!");
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        return BadRequest(_response);
                    }
                }

                valuationInDb.Status = SD.Request_Cancel;
                await _unitOfWork.ValuationService.Update(valuationInDb);

                baseRequest.Status = SD.Request_Cancel;
                await _unitOfWork.RequestService.Update(baseRequest);


                var motor = await _unitOfWork.MotorBikeService.GetFirst(x => x.MotorId == baseRequest.MotorId);

                Notification newUserNoti = new()
                {
                    RequestId = baseRequest.RequestId,
                    UserId = baseRequest.SenderId,
                    Title = "Chủ xe đã từ chối thương lượng",
                    Content = "Chủ xe " + motor.MotorName + " đã từ chối thương lượng.",
                    NotificationTypeId = SD.NotificationType_Valuation,
                    Time = VnDate,
                    IsRead = false
                };
                await _unitOfWork.NotificationService.Add(newUserNoti);

                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Message = ("Bạn đã hủy yêu cầu thành công!");
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
