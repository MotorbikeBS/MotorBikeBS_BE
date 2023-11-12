using API.DTO;
using API.DTO.BillDTO;
using API.DTO.BookingDTO;
using API.DTO.MotorbikeDTO;
using API.DTO.UserDTO;
using API.Utility;
using API.Validation;
using AutoMapper;
using Azure;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Service;
using Service.UnitOfWork;
using System.Net;
using System.Runtime.CompilerServices;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private ApiResponse _response;
        private readonly IMapper _mapper;

        public BillController(IUnitOfWork unitOfWork, IMapper mapper, IEmailSender emailSender)
        {
            _unitOfWork = unitOfWork;
            _response = new ApiResponse();
            _mapper = mapper;
        }

        [Authorize]
        [HttpGet("GetByStoreId")]
        public async Task<IActionResult> GetByStoreId(int ReceiverID)
        {
            try
            {
                var listDatabase = await _unitOfWork.BillService.Get(e => e.StoreId == ReceiverID, includeProperties: SD.GetBillArray);
                var listResponse = _mapper.Map<List<BillResponseDTO>>(listDatabase);
                if (listDatabase == null || listDatabase.Count() <= 0)
                {
                    _response.ErrorMessages.Add("Không tìm thấy hóa đơn nào!");
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return NotFound(_response);
                }
                else
                {
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
        [HttpGet("GetByUserId")]
        public async Task<IActionResult> GetByUserId(int UserId)
        {
            try
            {
                var listDatabase = await _unitOfWork.BillService.Get(e => e.UserId == UserId, includeProperties: SD.GetBillArray);
                var listResponse = _mapper.Map<List<BillResponseDTO>>(listDatabase);
                if (listDatabase == null || listDatabase.Count() <= 0)
                {
                    _response.ErrorMessages.Add("Không tìm thấy hóa đơn nào!");
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return NotFound(_response);
                }
                else
                {
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
        [HttpGet("GetByBillID")]
        public async Task<IActionResult> GetByBillID(int BillID)
        {
            try
            {
                var listDatabase = await _unitOfWork.BillService.GetFirst(e => e.BillConfirmId == BillID, includeProperties:  SD.GetBillArray);
                var listResponse = _mapper.Map<BillResponseDTO>(listDatabase);
                if (listDatabase == null)
                {
                    _response.ErrorMessages.Add("Không tìm thấy hóa đơn nào!");
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return NotFound(_response);
                }
                else
                {
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
        [Authorize(Roles = "Store")]
        [Route("CreateBill-InStock")]
        public async Task<IActionResult> BillforInStock(int newUser, int MotorID)
        {
            try
            {
                var obj = await _unitOfWork.MotorBikeService.GetFirst(e => e.MotorId == MotorID);
                if (obj == null)
                {
                    _response.ErrorMessages.Add("Không tìm thấy xe này!");
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                else if (obj.MotorStatusId != SD.Status_Posting)
                {
                    _response.ErrorMessages.Add("Xe đã chọn không phải xe có sẵn tại cửa hàng!");
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                else
                {
                    int requestPost = 0;
                    var userId = int.Parse(User.FindFirst("UserId")?.Value);
                    var Store = await _unitOfWork.StoreDescriptionService.GetFirst(e => e.UserId == userId);
                    int StoreID = (Store == null) ? 0 : Store.StoreId;
                    if (StoreID != obj.StoreId && userId != obj.OwnerId)
                    {
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.IsSuccess = false;
                        _response.ErrorMessages.Add("Xe không thuộc quyền của người dùng!");
                        return BadRequest(_response);
                    }
                    else
                    {
                        // If newUser is null, send Ownership to AdminID which can't access this motor anymore
                        if (newUser == 0) { newUser = SD.AdminID; }
                        //-----------
                        int check = 0;
                        foreach (var PostingType in SD.RequestPostingTypeArray)
                        {
                            var request = await _unitOfWork.RequestService.GetFirst(
                                    e => e.MotorId == MotorID && e.RequestTypeId == PostingType && e.Status == SD.Request_Accept
                            );
                            if (request != null) 
                            {
                                check += 1;
                                //Get requestPost to cancel Posting
                                requestPost = request.RequestId;
                            }
                        }
                        if (check == 0)
                        {
                            _response.StatusCode = HttpStatusCode.BadRequest;
                            _response.IsSuccess = false;
                            _response.ErrorMessages.Add("Xe chưa được đăng lên sàn!");
                            return BadRequest(_response);
                        }
                    }
                    //Add Request
                    Request requestNew = new()
                    {
                        MotorId = MotorID,
                        ReceiverId = newUser,
                        SenderId = userId,
                        Time = DateTime.Now,
                        RequestTypeId = SD.Request_MotorTranfer_Id,
                        Status = SD.Request_Pending
                    };
                    await _unitOfWork.RequestService.Add(requestNew);
                    //-----------
                    //Add Cus Bill
                    var requestCus = await _unitOfWork.RequestService.GetLast(
                                    e => e.MotorId == MotorID && e.RequestTypeId == SD.Request_MotorTranfer_Id && e.Status == SD.Request_Pending
                    );
                    if (requestCus == null)
                    {
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.IsSuccess = false;
                        _response.ErrorMessages.Add("Không tìm thấy yêu cầu!");
                        return BadRequest(_response);
                    }
                    BillConfirm CusBill = new()
                    {
                        MotorId = MotorID,
                        UserId = userId,
                        StoreId = (int)obj.StoreId,
                        Price = obj.Price,
                        CreateAt = DateTime.Now,
                        Status = SD.Request_Accept,
                        RequestId = requestCus.RequestId
                    };
                    await _unitOfWork.BillService.Add(CusBill);
                    //Update Bill-request to done
                    requestCus.Status = SD.Request_Accept;
                    await _unitOfWork.RequestService.Update(requestCus);
                    //Cancel Posting-request
                    var requestPosting = await _unitOfWork.RequestService.GetFirst(e => e.RequestId == requestPost);
                    requestPosting.Status = SD.Request_Cancel;
                    await _unitOfWork.RequestService.Update(requestPosting);
                    //Update Motor Ownership
                    obj.MotorStatusId = SD.Status_Storage;
                    obj.StoreId = null;
                    obj.OwnerId = newUser;
                    await _unitOfWork.MotorBikeService.Update(obj);
                    //---------------------
                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Result = CusBill;
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
        [Authorize(Roles = "Store")]
        [Route("CreateBill-Consignment")]
        public async Task<IActionResult> BillforConsignment(int newUser, int MotorID)
        {
            try
            {                
                var NegoRequest = await _unitOfWork.RequestService.GetLast(e => e.MotorId == MotorID
                                                                        && e.RequestTypeId == SD.Request_Negotiation_Id
                                                                        && e.Status == SD.Request_Accept
                );
                if (NegoRequest == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("Giá xe chưa được thương lượng với chủ sở hữu!");
                    return BadRequest(_response);
                }
                else if (NegoRequest.Status != SD.Request_Accept)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("Xe đang trong quá trình thương lượng giá cả!");
                    return BadRequest(_response);
                }
                var obj = await _unitOfWork.MotorBikeService.GetFirst(e => e.MotorId == NegoRequest.MotorId);
                if (obj == null)
                {
                    _response.ErrorMessages.Add("Không tìm thấy xe này!");
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                else if (obj.MotorStatusId != SD.Status_Consignment)
                {
                    _response.ErrorMessages.Add("Xe đã chọn không phải xe ký gửi tại cửa hàng!");
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                else
                {
                    int requestPost = 0;    
                    var userId = int.Parse(User.FindFirst("UserId")?.Value);
                    var Store = await _unitOfWork.StoreDescriptionService.GetFirst(e => e.UserId == userId);
                    int StoreID = (Store == null) ? 0 : Store.StoreId;
                    if (StoreID != obj.StoreId && userId != obj.OwnerId)
                    {
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.IsSuccess = false;
                        _response.ErrorMessages.Add("Xe không thuộc quyền của người dùng!");
                        return BadRequest(_response);
                    }
                    else
                    {
                        // If newUser is null, send Ownership to AdminID which can't access this motor anymore
                        if (newUser == 0) { newUser = SD.AdminID; }
                        //-----------
                        int check = 0;
                        foreach (var PostingType in SD.RequestPostingTypeArray)
                        {
                            var request = await _unitOfWork.RequestService.GetLast(
                                    e => e.MotorId == NegoRequest.MotorId && e.RequestTypeId == PostingType && e.Status == SD.Request_Accept
                            );
                            if (request != null) 
                            {
                                check += 1;
                                //Get requestPost to cancel Posting
                                requestPost = request.RequestId;
                            }
                        }
                        if (check == 0)
                        {
                            _response.StatusCode = HttpStatusCode.BadRequest;
                            _response.IsSuccess = false;
                            _response.ErrorMessages.Add("Xe chưa được đăng lên sàn!");
                            return BadRequest(_response);
                        }
                    }
                    //Add Tranfer Request 
                    Request requestNew = new()
                    {
                        MotorId = NegoRequest.MotorId,
                        ReceiverId = newUser,
                        SenderId = userId,
                        Time = DateTime.Now,
                        RequestTypeId = SD.Request_MotorTranfer_Id,
                        Status = SD.Request_Pending
                    };
                    await _unitOfWork.RequestService.Add(requestNew);
                    //Add Cus Bill
                    var requestTranfer = await _unitOfWork.RequestService.GetLast(
                                    e => e.MotorId == NegoRequest.MotorId && e.RequestTypeId == SD.Request_MotorTranfer_Id && e.Status == SD.Request_Pending
                    );
                    if (requestTranfer == null)
                    {
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.IsSuccess = false;
                        _response.ErrorMessages.Add("Không tìm thấy yêu cầu!");
                        return BadRequest(_response);
                    }
                    BillConfirm CusBill = new()
                    {
                        MotorId = (int)NegoRequest.MotorId,
                        UserId = newUser,
                        StoreId = (int)obj.StoreId,
                        Price = obj.Price,
                        CreateAt = DateTime.Now,
                        Status = SD.Request_Accept,
                        RequestId = requestTranfer.RequestId
                    };
                    await _unitOfWork.BillService.Add(CusBill);
                    //Update Bill-request to done
                    requestTranfer.Status = SD.Request_Accept;
                    await _unitOfWork.RequestService.Update(requestTranfer);
                    //Cancel Posting-request
                    var requestPosting = await _unitOfWork.RequestService.GetFirst(e => e.RequestId == requestPost);
                    requestPosting.Status = SD.Request_Cancel;
                    await _unitOfWork.RequestService.Update(requestPosting);
                    //*** Add OwnerBill ***
                    var Negotiation = await _unitOfWork.NegotiationService.GetFirst(e => e.BaseRequestId == NegoRequest.RequestId);
                    if (Negotiation == null)
                    {
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.IsSuccess = false;
                        _response.ErrorMessages.Add("Không tìm thấy giao dịch thương lượng giá cả với chủ xe!");
                        return BadRequest(_response);
                    }
                    BillConfirm OwnerBill = new()
                    {
                        MotorId = (int)NegoRequest.MotorId,
                        UserId = obj.OwnerId,
                        StoreId = (int)obj.StoreId,
                        Price = Negotiation.FinalPrice,
                        CreateAt = DateTime.Now,
                        Status = SD.Request_Accept,
                        RequestId = NegoRequest.RequestId
                    };
                    await _unitOfWork.BillService.Add(OwnerBill);
                    //Cancel OwnerPosting
                    var OwnerPostingRequest = await _unitOfWork.RequestService.GetLast(e => e.MotorId == NegoRequest.MotorId
                                                            && e.SenderId == obj.OwnerId
                                                            && e.RequestTypeId == requestPosting.RequestTypeId && e.Status == SD.Request_Accept
                    );
                    OwnerPostingRequest.Status = SD.Request_Cancel;
                    await _unitOfWork.RequestService.Update(OwnerPostingRequest);
                    //Update Motor Ownership
                    obj.MotorStatusId = SD.Status_Storage;
                    obj.StoreId = null;
                    obj.OwnerId = newUser;
                    await _unitOfWork.MotorBikeService.Update(obj);
                    //---------------------
                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Result = CusBill;
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
        [Authorize(Roles = "Store")]
        [Route("CreateBill-nonConsignment")]
        public async Task<IActionResult> BillforNonConsignment(int MotorID)
        {
            try
            {
                //Check Negotiation request
                var NegoRequest = await _unitOfWork.RequestService.GetLast(e => e.MotorId == MotorID
                                                                        && e.RequestTypeId == SD.Request_Negotiation_Id
                                                                        && e.Status == SD.Request_Accept
                );
                if (NegoRequest == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("Giá xe chưa được thương lượng với chủ sở hữu!");
                    return BadRequest(_response);
                }
                else if (NegoRequest.Status != SD.Request_Accept)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("Xe đang trong quá trình thương lượng giá cả!");
                    return BadRequest(_response);
                }
                //Check BuyerBooking request
                var BuyerRequest = await _unitOfWork.RequestService.GetLast(e => e.MotorId == MotorID
                                                                        && e.RequestTypeId == SD.Request_Booking_Id
                                                                        && e.Status == SD.Request_Accept
                );
                if (BuyerRequest == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("Không tìm thấy yêu cầu đặt lịch xem xe!");
                    return BadRequest(_response);
                }
                var BuyerBooking = await _unitOfWork.BuyerBookingService.GetFirst(e => e.RequestId == BuyerRequest.RequestId);
                if (BuyerBooking == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("Không tìm thấy lịch hẹn!");
                    return BadRequest(_response);
                }
                else if (NegoRequest.Status != SD.Request_Accept)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("Lịch xem xe chưa được duyệt!");
                    return BadRequest(_response);
                }
                //Check the same motor
                if (NegoRequest.MotorId != BuyerRequest.MotorId)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("Xe đang chọn và xe được đặt lịch không trùng khớp!");
                    return BadRequest(_response);
                }
                //Check MotorBike
                var obj = await _unitOfWork.MotorBikeService.GetFirst(e => e.MotorId == NegoRequest.MotorId);
                if (obj == null)
                {
                    _response.ErrorMessages.Add("Không tìm thấy xe này!");
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                else if (obj.MotorStatusId != SD.Status_nonConsignment)
                {
                    _response.ErrorMessages.Add("Xe đã chọn không phải xe không ký gửi tại cửa hàng!");
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                else
                {
                    int requestPost = 0;
                    var userId = int.Parse(User.FindFirst("UserId")?.Value);
                    var Store = await _unitOfWork.StoreDescriptionService.GetFirst(e => e.UserId == userId);
                    int StoreID = (Store == null) ? 0 : Store.StoreId;
                    if (StoreID != obj.StoreId && userId != obj.OwnerId)
                    {
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.IsSuccess = false;
                        _response.ErrorMessages.Add("Xe không thuộc quyền của người dùng!");
                        return BadRequest(_response);
                    }
                    else
                    {
                        // Get UserID from BuyerBooking request
                        var newUser = BuyerRequest.SenderId; 
                        //-----------
                        int check = 0;
                        foreach (var PostingType in SD.RequestPostingTypeArray)
                        {
                            var request = await _unitOfWork.RequestService.GetLast(
                                    e => e.MotorId == NegoRequest.MotorId && e.RequestTypeId == PostingType && e.Status == SD.Request_Accept
                            );
                            if (request != null) 
                            {
                                check += 1;
                                //Get requestPost to cancel Posting
                                requestPost = request.RequestId;
                            }
                        }
                        if (check == 0)
                        {
                            _response.StatusCode = HttpStatusCode.BadRequest;
                            _response.IsSuccess = false;
                            _response.ErrorMessages.Add("Xe chưa được đăng lên sàn!");
                            return BadRequest(_response);
                        }
                        //Add Request
                        Request requestNew = new()
                        {
                            MotorId = NegoRequest.MotorId,
                            ReceiverId = newUser,
                            SenderId = userId,
                            Time = DateTime.Now,
                            RequestTypeId = SD.Request_MotorTranfer_Id,
                            Status = SD.Request_Pending
                        };
                        await _unitOfWork.RequestService.Add(requestNew);
                        //Add Cus Bill
                        var requestCus = await _unitOfWork.RequestService.GetLast(
                                        e => e.MotorId == NegoRequest.MotorId && e.RequestTypeId == SD.Request_MotorTranfer_Id && e.Status == SD.Request_Pending
                        );
                        if (requestCus == null)
                        {
                            _response.StatusCode = HttpStatusCode.BadRequest;
                            _response.IsSuccess = false;
                            _response.ErrorMessages.Add("Không tìm thấy yêu cầu!");
                            return BadRequest(_response);
                        }
                        BillConfirm CusBill = new()
                        {
                            MotorId = (int) NegoRequest.MotorId,
                            UserId = (int) newUser,
                            StoreId = (int) obj.StoreId,
                            Price = obj.Price,
                            CreateAt = DateTime.Now,
                            Status = SD.Request_Accept,
                            RequestId = requestCus.RequestId
                        };
                        await _unitOfWork.BillService.Add(CusBill);
                        //Update Bill-request to done
                        requestCus.Status = SD.Request_Accept;
                        await _unitOfWork.RequestService.Update(requestCus);
                        //Cancel Posting-request
                        var requestPosting = await _unitOfWork.RequestService.GetFirst(e => e.RequestId == requestPost);
                        requestPosting.Status = SD.Request_Cancel;
                        await _unitOfWork.RequestService.Update(requestPosting);
                        //*** Add OwnerBill ***
                        var Negotiation = await _unitOfWork.NegotiationService.GetFirst(e => e.BaseRequestId == NegoRequest.RequestId);
                        if (Negotiation == null)
                        {
                            _response.StatusCode = HttpStatusCode.BadRequest;
                            _response.IsSuccess = false;
                            _response.ErrorMessages.Add("Không tìm thấy giao dịch thương lượng giá cả với chủ xe!");
                            return BadRequest(_response);
                        }
                        BillConfirm OwnerBill = new()
                        {
                            MotorId = (int)NegoRequest.MotorId,
                            UserId = obj.OwnerId,
                            StoreId = (int)obj.StoreId,
                            Price = Negotiation.FinalPrice,
                            CreateAt = DateTime.Now,
                            Status = SD.Request_Accept,
                            RequestId = NegoRequest.RequestId
                        };
                        await _unitOfWork.BillService.Add(OwnerBill);
                        //Cancel OwnerPosting
                        var OwnerPostingRequest = await _unitOfWork.RequestService.GetLast(e => e.MotorId == NegoRequest.MotorId 
                                                            && e.SenderId == obj.OwnerId
                                                            && e.RequestTypeId == requestPosting.RequestTypeId && e.Status == SD.Request_Accept
                        );
                        OwnerPostingRequest.Status = SD.Request_Cancel;
                        await _unitOfWork.RequestService.Update(OwnerPostingRequest);
                        //Update Motor Ownership
                        obj.MotorStatusId = SD.Status_Storage;
                        obj.StoreId = null;
                        obj.OwnerId = (int) newUser;
                        await _unitOfWork.MotorBikeService.Update(obj);
                        //---------------------
                        _response.IsSuccess = true;
                        _response.StatusCode = HttpStatusCode.OK;
                        _response.Result = CusBill;
                        return Ok(_response);
                    }                    
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
