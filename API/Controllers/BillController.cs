using API.DTO;
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
                var listDatabase = await _unitOfWork.BillService.Get(e => e.StoreId == ReceiverID, includeProperties: "Request");
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
                    _response.Result = listDatabase;
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
                var listDatabase = await _unitOfWork.BillService.Get(e => e.UserId == UserId, includeProperties: "Request");
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
                    _response.Result = listDatabase;
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
                var listDatabase = await _unitOfWork.BillService.GetFirst(e => e.BillConfirmId == BillID, includeProperties: "Request");
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
                    _response.Result = listDatabase;
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
        [Authorize(Roles = "Store")]
        [Route("BillforInStock")]
        public async Task<IActionResult> BillforInStock(int MotorID, int newUser)
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
                else
                {
                    var userId = int.Parse(User.FindFirst("UserId")?.Value);
                    if (userId != obj.StoreId && userId != obj.OwnerId)
                    {
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.IsSuccess = false;
                        _response.ErrorMessages.Add("Xe không thuộc quyền của người dùng!");
                        return BadRequest(_response);
                    }
                    else
                    {
                        int check = 0;
                        foreach (var PostingType in SD.RequestPostingTypeArray)
                        {
                            var request = await _unitOfWork.RequestService.GetFirst(
                                    e => e.MotorId == MotorID && e.RequestTypeId == PostingType && e.Status == SD.Request_Accept
                            );
                            if (request != null) { check += 1; }
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
                    //Update request to done
                    requestCus.Status = SD.Request_Accept;
                    await _unitOfWork.RequestService.Update(requestCus);
                    //Update Motor Ownership
                    obj.MotorStatusId = SD.Status_Storage;
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
    }
}