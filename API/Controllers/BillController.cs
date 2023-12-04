using API.DTO;
using API.DTO.BillDTO;
using API.DTO.BillDTO.API.DTO.BillDTO;
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
using System.Runtime.InteropServices;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private ApiResponse _response;
        private readonly IMapper _mapper;
        public DateTime VnDate = DateTime.Now.ToLocalTime();

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
                var userInfo = await _unitOfWork.StoreDescriptionService.GetFirst(e => e.StoreId == ReceiverID);
                if (userInfo == null)
                {
                    _response.ErrorMessages.Add("Không tìm thấy thông tin cửa hàng!");
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
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
                var userInfo = await _unitOfWork.UserService.GetFirst(e => e.UserId == UserId);
                if (userInfo == null)
                {
                    _response.ErrorMessages.Add("Không tìm thấy thông tin người dùng!");
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
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

        [Authorize(Roles = "Store")]
        [HttpGet("GetIncome_MonthYear")]
        public async Task<IActionResult> GetIncome_MonthYear(DateTime startDate, DateTime endDate, string IncomeType)
        {
            try
            {
                var rs = InputValidation.IncomeDateValidation(startDate, endDate);
                if (rs != "")
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Result = false;
                    _response.ErrorMessages.Add(rs);
                    return BadRequest(_response);
                }
                if (!IncomeType.Equals("Month", StringComparison.OrdinalIgnoreCase) && !IncomeType.Equals("Year", StringComparison.OrdinalIgnoreCase))
                {
                    _response.ErrorMessages.Add("Vui lòng chọn kiểu tính thu nhập dựa trên Tháng hoặc Năm!");
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return NotFound(_response);
                }
                var userId = int.Parse(User.FindFirst("UserId")?.Value);
                var storeInfor = await _unitOfWork.StoreDescriptionService.GetFirst(e => e.UserId == userId);
                if (storeInfor == null)
                {
                    _response.ErrorMessages.Add("Không tìm thấy thông tin cửa hàng!");
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return NotFound(_response);
                }
                var listDatabase = await _unitOfWork.BillService.Get(e => e.StoreId == storeInfor.StoreId, includeProperties: "Request");
                var listResponse = _mapper.Map<List<IncomeBillResponseDTO>>(listDatabase);

                if (listDatabase == null || listDatabase.Count() <= 0)
                {
                    _response.ErrorMessages.Add("Chưa có dữ liệu");
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return NotFound(_response);
                }
                else
                {
                    decimal? TotalIncome = 0, TotalExpense = 0;
                    List<IncomeDTO> incomeList = new List<IncomeDTO>();
                    endDate = endDate.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
                    if (IncomeType.Equals("Month", StringComparison.OrdinalIgnoreCase))
                    {
                        for (DateTime currentMonth = startDate; (currentMonth <= endDate || currentMonth.Month == endDate.Month); currentMonth = currentMonth.AddMonths(1))
                        {
                            decimal? Income = 0, Expense = 0;
                            DateTime startFrom = new DateTime(currentMonth.Year, currentMonth.Month, 1);
                            DateTime endFrom = new DateTime(currentMonth.Year, currentMonth.Month, DateTime.DaysInMonth(currentMonth.Year, currentMonth.Month), 23, 59, 59);
                            if ((currentMonth.Month == endDate.Month) && (currentMonth.Year == endDate.Year))
                            { 
                                endFrom = endDate;
                            }
                            var monthBills = listResponse.Where(bill => bill.CreateAt.HasValue
                                && bill.CreateAt >= startFrom && bill.CreateAt.Value <= endFrom).ToList();
                            foreach (var bill in monthBills)
                            {
                                if (bill.Price != null)
                                {
                                    if (bill.Request.RequestTypeId == SD.Request_Negotiation_Id)
                                    {
                                        Expense += bill.Price;
                                        TotalExpense += bill.Price;
                                    }
                                    else
                                    {
                                        Income += bill.Price;
                                        TotalIncome += bill.Price;
                                    }
                                }
                            }
                            var income_clone = new IncomeDTO
                            {
                                IncomeTime = $"{currentMonth:MM/yyyy}",
                                Income = Income,
                                Expense = Expense,
                                Total = Income - Expense,
                                IncomeType = IncomeType
                            };
                            incomeList.Add(income_clone);
                        }
                    }
                    else if (IncomeType.Equals("Year", StringComparison.OrdinalIgnoreCase))
                    {
                        for (int year = startDate.Year; year <= endDate.Year; year++)
                        {
                            decimal? Income = 0, Expense = 0;
                            DateTime startFrom = new DateTime(year, 1, 1);
                            DateTime endFrom = new DateTime(year, 12, 31, 23, 59, 59);
                            if(year == startDate.Year)
                            {
                                startFrom = startDate;
                            }
                            if (year == endDate.Year)
                            {
                                endFrom = endDate;
                            }
                            var yearBills = listResponse.Where(bill => bill.CreateAt.HasValue
                               && bill.CreateAt >= startFrom && bill.CreateAt.Value <= endFrom).ToList();

                            foreach (var bill in yearBills)
                            {
                                if (bill.Price != null)
                                {
                                    if (bill.Request.RequestTypeId == SD.Request_Negotiation_Id)
                                    {
                                        Expense += bill.Price;
                                        TotalExpense += bill.Price;
                                    }
                                    else
                                    {
                                        Income += bill.Price;
                                        TotalIncome += bill.Price;
                                    }
                                }
                            }
                            var income_clone = new IncomeDTO
                            {
                                IncomeTime = $"{year}",
                                Income = Income,
                                Expense = Expense,
                                Total = Income - Expense,
                                IncomeType = IncomeType
                            };
                            incomeList.Add(income_clone);
                        }
                    }
                    var incomeTotal = new IncomeDTO
                    {
                        IncomeTime = IncomeType.Equals("Month", StringComparison.OrdinalIgnoreCase)
                                        ? $"From {startDate.Month}/{startDate.Year} to {endDate.Month}/{endDate.Year}"
                                        : $"From {startDate.Year} to {endDate.Year}",
                        Income = TotalIncome,
                        Expense = TotalExpense,
                        Total = TotalIncome - TotalExpense,
                        IncomeType = IncomeType
                    };
                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Result = new { IncomeType, storeInfor.StoreId, storeInfor.StoreName, Bills = incomeList , Total = incomeTotal };
                }
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
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
                    Request requestTranfer = new()
                    {
                        MotorId = MotorID,
                        ReceiverId = newUser,
                        SenderId = userId,
                        Time = VnDate,
                        RequestTypeId = SD.Request_MotorTranfer_Id,
                        Status = SD.Request_Pending
                    };
                    await _unitOfWork.RequestService.Add(requestTranfer);
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
                        CreateAt = VnDate,
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
                    //Add newUser Notification
                    Notification newUserNoti = new()
                    {
                        RequestId = requestTranfer.RequestId,
                        UserId = newUser,
                        Title = "Thông tin xe mới được vào kho",
                        Content = "Xe " + obj.MotorName + " đã được thêm vào kho xe của bạn.",
                        NotificationTypeId = SD.NotificationType_TranferOwnership,
                        Time = VnDate,
                        IsRead = false
                    };
                    await _unitOfWork.NotificationService.Add(newUserNoti);
                    //-----------------------
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
                        Time = VnDate,
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
                        CreateAt = VnDate,
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
                        CreateAt = VnDate,
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
                    //Add newUser Notification
                    Notification newUserNoti = new()
                    {
                        RequestId = requestTranfer.RequestId,
                        UserId = newUser,
                        Title = "Thông tin xe mới được vào kho",
                        Content = "Xe " + obj.MotorName + " đã được thêm vào kho xe của bạn.",
                        NotificationTypeId = SD.NotificationType_TranferOwnership,
                        Time = VnDate,
                        IsRead = false
                    };
                    await _unitOfWork.NotificationService.Add(newUserNoti);
                    //Add Owner Notification
                    Notification ownerNoti = new()
                    {
                        RequestId = NegoRequest.RequestId,
                        UserId = NegoRequest.ReceiverId,
                        Title = "Thông tin hóa đơn mới được thêm",
                        Content = "Xe " + obj.MotorName + " đã bán. Thông tin hóa đơn được thêm vào lịch sử của bạn.",
                        NotificationTypeId = SD.NotificationType_OwnerSoldOut,
                        Time = VnDate,
                        IsRead = false
                    };
                    await _unitOfWork.NotificationService.Add(ownerNoti);
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
        public async Task<IActionResult> BillforNonConsignment(int MotorID, int BuyerBookingID)
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
                var BuyerBooking = await _unitOfWork.BuyerBookingService.GetFirst(e => e.BookingId == BuyerBookingID);
                if (BuyerBooking == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("Không tìm thấy lịch hẹn!");
                    return BadRequest(_response);
                }
                var BuyerRequest = await _unitOfWork.RequestService.GetLast(e => e.RequestId == BuyerBooking.RequestId);
                if (BuyerRequest == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("Không tìm thấy yêu cầu đặt lịch xem xe!");
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
                        Request requestTranfer = new()
                        {
                            MotorId = NegoRequest.MotorId,
                            ReceiverId = newUser,
                            SenderId = userId,
                            Time = VnDate,
                            RequestTypeId = SD.Request_MotorTranfer_Id,
                            Status = SD.Request_Pending
                        };
                        await _unitOfWork.RequestService.Add(requestTranfer);
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
                            CreateAt = VnDate,
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
                            CreateAt = VnDate,
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
                        //Add newUser Notification
                        Notification newUserNoti = new()
                        {
                            RequestId = requestTranfer.RequestId,
                            UserId = newUser,
                            Title = "Thông tin xe mới được vào kho",
                            Content = "Xe " + obj.MotorName + " đã được thêm vào kho xe của bạn.",
                            NotificationTypeId = SD.NotificationType_TranferOwnership,
                            Time = VnDate,
                            IsRead = false
                        };
                        await _unitOfWork.NotificationService.Add(newUserNoti);
                        //Add Owner Notification
                        Notification ownerNoti = new()
                        {
                            RequestId = NegoRequest.RequestId,
                            UserId = NegoRequest.ReceiverId,
                            Title = "Thông tin hóa đơn mới được thêm",
                            Content = "Xe " + obj.MotorName + " đã bán. Thông tin hóa đơn được thêm vào lịch sử của bạn.",
                            NotificationTypeId = SD.NotificationType_OwnerSoldOut,
                            Time = VnDate,
                            IsRead = false
                        };
                        await _unitOfWork.NotificationService.Add(ownerNoti);
                        //-------------------------
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
