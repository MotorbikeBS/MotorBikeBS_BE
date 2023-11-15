using API.DTO;
using API.DTO.FilterDTO;
using API.DTO.MotorbikeDTO;
using API.DTO.PostBoostingDTO;
using API.DTO.UserDTO;
using API.Utility;
using API.Validation;
using AutoMapper;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Service.BlobImageService;
using Service.PagingUriGenerator;
using Service.UnitOfWork;
using Service.Wrappers;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using VOP_API.Helpers;
using static System.Net.Mime.MediaTypeNames;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MotorBikeController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private ApiResponse _response;
        private readonly IMapper _mapper;
        private readonly IBlobService _blobService;
        private readonly IPagingUriService _uriService;

        public MotorBikeController(IUnitOfWork unitOfWork, IMapper mapper, IBlobService blobService, IPagingUriService pagingUriService)
        {
            _unitOfWork = unitOfWork;
            _response = new ApiResponse();
            _mapper = mapper;
            _blobService = blobService;
            _uriService = pagingUriService;
        }

        //[HttpGet("GetAllWithSpecificStatus")]
        //[Authorize(Roles = "Store,Owner")]
        //public async Task<IActionResult> GetAllWithSpecificStatus(int StatusID )
        //{
        //    try
        //    {
        //        var listDatabase = await _unitOfWork.MotorBikeService.Get(e => e.MotorStatusId == StatusID, SD.GetMotorArray);
        //        var listResponse = _mapper.Map<List<MotorResponseDTO>>(listDatabase);
        //        if (listDatabase == null || listDatabase.Count() <= 0)
        //        {
        //            _response.ErrorMessages.Add("Không tìm thấy xe nào!");
        //            _response.IsSuccess = false;
        //            _response.StatusCode = HttpStatusCode.BadRequest;
        //            return NotFound(_response);
        //        }
        //        else
        //        {
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
        [HttpGet("GetAllOnExchange")]
        public async Task<IActionResult> GetAllOnExchange([FromQuery] PaginationFilter paginationFilter)
        {
            try
            {
                // Paging
                if (paginationFilter == null)
                {
                    paginationFilter = new PaginationFilter();
                }
                var route = Request.Path.Value;
                var validFilter = new PaginationFilter(paginationFilter.PageNumber, paginationFilter.PageSize);
                //----------------------
                var listPostBoosting = await _unitOfWork.PostBoostingService.Get(x => x.Status == SD.Request_Accept 
                                                                                && x.StartTime < DateTime.Now && x.EndTime > DateTime.Now);
                var listDatabase = await _unitOfWork.MotorBikeService.Get(e => e.MotorStatusId != SD.Status_Storage && e.StoreId != null, SD.GetMotorArray);
                if (listDatabase == null || listDatabase.Count() <= 0)
                {
                    _response.ErrorMessages.Add("Không tìm thấy xe nào!");
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return NotFound(_response);
                }
                var listResponse = _mapper.Map<List<MotorResponseDTO>>(listDatabase);
                // (Boosting) Iterate through each MotorBike in listDatabase
                foreach (var motor in listResponse)
                {
                    // Check if the MotorBike is in the list of PostBoosting
                    var matchingPostBoosting = listPostBoosting.FirstOrDefault(p => p.MotorId == motor.MotorId);

                    //If yes, assign the PostBoosting object to the MotorBike
                    if (matchingPostBoosting != null)
                    {
                        var boostingInfor = _mapper.Map<PostBoostingCreateDTO>(matchingPostBoosting);
                        motor.Boosting = boostingInfor;
                    }
                }
                // Sort listDatabase by Level in descending order
                listResponse = listResponse.OrderByDescending(motor => motor.Boosting?.Level ?? 0).ToList();
                //---------------------------
                foreach (var r in listResponse)
                {
                    r.Owner = null;
                    foreach (var PostingType in SD.RequestPostingTypeArray)
                    {
                        var request = await _unitOfWork.RequestService.GetLast(
                                e => e.MotorId == r.MotorId && e.RequestTypeId == PostingType && e.Status == SD.Request_Accept
                        );
                        if (request != null) { r.PostingAt = request?.Time; }
                    }                    
                }
                // Paging
                var pagedData = listResponse.ToList().Skip((validFilter.PageNumber - 1) * validFilter.PageSize).Take(validFilter.PageSize);
                var totalRecords = listDatabase.Count();
                var pagedReponse = PaginationHelper.CreatePagedReponse<MotorResponseDTO>(pagedData.ToList(), validFilter, totalRecords, _uriService, route);
                //-------------------
                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = pagedReponse;
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
        [HttpGet("GetAllOnStoreExchange")]
        public async Task<IActionResult> GetAllOnStoreExchange([FromQuery] PaginationFilter paginationFilter)
        {
            try
            {
                // Paging
                if (paginationFilter == null)
                {
                    paginationFilter = new PaginationFilter();
                }
                var route = Request.Path.Value;
                var validFilter = new PaginationFilter(paginationFilter.PageNumber, paginationFilter.PageSize);
                //----------------------
                var listDatabase = await _unitOfWork.MotorBikeService.Get(
                    expression: e => (e.MotorStatusId == SD.Status_Consignment || e.MotorStatusId == SD.Status_nonConsignment) && e.StoreId == null,
                    includeProperties: SD.GetMotorArray
                );
                if (listDatabase == null || listDatabase.Count() <= 0)
                {
                    _response.ErrorMessages.Add("Không tìm thấy xe nào!");
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                var listResponse = _mapper.Map<List<MotorResponseDTO>>(listDatabase);
                // (Boosting) 
                var listPostBoosting = await _unitOfWork.PostBoostingService.Get(x => x.Status == SD.Request_Accept
                                                                                && x.StartTime < DateTime.Now && x.EndTime > DateTime.Now);
                // (Boosting) Iterate through each MotorBike in listDatabase
                foreach (var motor in listResponse)
                {
                    // Check if the MotorBike is in the list of PostBoosting
                    var matchingPostBoosting = listPostBoosting.FirstOrDefault(p => p.MotorId == motor.MotorId);

                    //If yes, assign the PostBoosting object to the MotorBike
                    if (matchingPostBoosting != null)
                    {
                        var boostingInfor = _mapper.Map<PostBoostingCreateDTO>(matchingPostBoosting);
                        motor.Boosting = boostingInfor;
                    }
                }
                // (Boosting) Sort listDatabase by Level in descending order
                listResponse = listResponse.OrderByDescending(motor => motor.Boosting?.Level ?? 0).ToList();
                //---------------------------
                foreach (var r in listResponse)
                {
                    r.Store = null;
                    foreach (var PostingType in SD.RequestPostingTypeArray)
                    {
                        var request = await _unitOfWork.RequestService.GetLast(
                                e => e.MotorId == r.MotorId && e.RequestTypeId == PostingType && e.Status == SD.Request_Accept
                        );
                        if (request != null) { r.PostingAt = request?.Time; }
                    }
                }                
                // Paging
                var pagedData = listResponse.ToList().Skip((validFilter.PageNumber - 1) * validFilter.PageSize).Take(validFilter.PageSize);
                var totalRecords = listDatabase.Count();
                var pagedReponse = PaginationHelper.CreatePagedReponse<MotorResponseDTO>(pagedData.ToList(), validFilter, totalRecords, _uriService, route);
                //-------------------
                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = pagedReponse;
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
        [HttpGet("GetMotorByStoreId")]
        public async Task<IActionResult> GetByStoreId(int StoreID)
        {
            try
            {
                var listDatabase = await _unitOfWork.MotorBikeService.Get(e => e.StoreId == StoreID,SD.GetMotorArray);
                if (listDatabase == null || listDatabase.Count() <= 0)
                {
                    _response.ErrorMessages.Add("Không tìm thấy xe nào!");
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                var listResponse = _mapper.Map<List<MotorResponseDTO>>(listDatabase);
                // (Boosting) 
                var listPostBoosting = await _unitOfWork.PostBoostingService.Get(x => x.Status == SD.Request_Accept
                                                                                && x.StartTime < DateTime.Now && x.EndTime > DateTime.Now);
                // (Boosting) Iterate through each MotorBike in listDatabase
                foreach (var motor in listResponse)
                {
                    // Check if the MotorBike is in the list of PostBoosting
                    var matchingPostBoosting = listPostBoosting.FirstOrDefault(p => p.MotorId == motor.MotorId);

                    //If yes, assign the PostBoosting object to the MotorBike
                    if (matchingPostBoosting != null)
                    {
                        var boostingInfor = _mapper.Map<PostBoostingCreateDTO>(matchingPostBoosting);
                        motor.Boosting = boostingInfor;
                    }
                }
                // (Boosting) Sort listDatabase by Level in descending order
                listResponse = listResponse.OrderByDescending(motor => motor.Boosting?.Level ?? 0).ToList();
                //---------------------------
                foreach (var r in listResponse)
                {
                    foreach (var PostingType in SD.RequestPostingTypeArray)
                    {
                        var request = await _unitOfWork.RequestService.GetLast(
                                e => e.MotorId == r.MotorId && e.RequestTypeId == PostingType && e.Status == SD.Request_Accept
                        );
                        if (request != null) { r.PostingAt = request?.Time; }
                    }
                }
                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = listResponse;
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
        [HttpGet("GetMotorByOwner")]
        public async Task<IActionResult> GetByOwnerId(int OwnerID)
        {
            try
            {
                var listDatabase = await _unitOfWork.MotorBikeService.Get(e => e.OwnerId == OwnerID,SD.GetMotorArray);
                var listResponse = _mapper.Map<List<MotorResponseDTO>>(listDatabase);                
                if (listDatabase == null || listDatabase.Count() <= 0)
                {
                    _response.ErrorMessages.Add("Không tìm thấy xe nào!");
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                else
                {
                    // (Boosting) 
                    var listPostBoosting = await _unitOfWork.PostBoostingService.Get(x => x.Status == SD.Request_Accept
                                                                                    && x.StartTime < DateTime.Now && x.EndTime > DateTime.Now);
                    // (Boosting) Iterate through each MotorBike in listDatabase
                    foreach (var motor in listResponse)
                    {
                        // Check if the MotorBike is in the list of PostBoosting
                        var matchingPostBoosting = listPostBoosting.FirstOrDefault(p => p.MotorId == motor.MotorId);

                        //If yes, assign the PostBoosting object to the MotorBike
                        if (matchingPostBoosting != null)
                        {
                            var boostingInfor = _mapper.Map<PostBoostingCreateDTO>(matchingPostBoosting);
                            motor.Boosting = boostingInfor;
                        }
                    }
                    // (Boosting) Sort listDatabase by Level in descending order
                    listResponse = listResponse.OrderByDescending(motor => motor.Boosting?.Level ?? 0).ToList();
                    //---------------------------
                    foreach (var r in listResponse)
                    {
                        foreach (var PostingType in SD.RequestPostingTypeArray)
                        {
                            var request = await _unitOfWork.RequestService.GetLast(
                                    e => e.MotorId == r.MotorId && e.RequestTypeId == PostingType && e.Status == SD.Request_Accept
                            );
                            if (request != null) { r.PostingAt = request?.Time; }
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
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetByMotorId(int id)
        {
            try
            {
                var listDatabase = await _unitOfWork.MotorBikeService.GetFirst(e => e.MotorId == id,SD.GetMotorArray);
                if (listDatabase == null)
                {
                    _response.ErrorMessages.Add("Không tìm thấy xe nào!");
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                var listResponse = _mapper.Map<MotorResponseDTO>(listDatabase);
                // (Boosting) 
                var listPostBoosting = await _unitOfWork.PostBoostingService.GetFirst(x => x.Status == SD.Request_Accept
                                                                                && x.MotorId == id
                                                                                && x.StartTime < DateTime.Now && x.EndTime > DateTime.Now);
                // (Boosting) Iterate through each MotorBike in listDatabase
                if (listPostBoosting != null)
                {
                    var boostingInfor = _mapper.Map<PostBoostingCreateDTO>(listPostBoosting);
                    listResponse.Boosting = boostingInfor;
                }
                // (Boosting) Sort listDatabase by Level in descending order
                foreach (var PostingType in SD.RequestPostingTypeArray)
                {
                    var request = await _unitOfWork.RequestService.GetLast(
                            e => e.MotorId == listResponse.MotorId && e.RequestTypeId == PostingType && e.Status == SD.Request_Accept
                    );
                    if (request != null) { listResponse.PostingAt = request?.Time; }
                }
                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = listResponse;
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
        [HttpGet("search-MotorName")]
        public async Task<IActionResult> SearchByMotorName(string motorName)
        {
            try
            {
                var roleId = int.Parse(User.FindFirst("RoleId")?.Value);
                motorName = InputValidation.RemoveExtraSpaces(motorName);
                IEnumerable<Motorbike>? motorbikes;
                switch (roleId)
                {
                    case SD.Role_Customer_Id:
                        motorbikes = await _unitOfWork.MotorBikeService.Get(
                        expression: motor => motor.MotorName.Contains(motorName) 
                            && motor.MotorStatusId != SD.Status_Storage
                            && motor.StoreId != null,
                        includeProperties: SD.GetMotorArray
                        );
                        break;
                    default:
                        motorbikes = await _unitOfWork.MotorBikeService.Get(
                        expression: motor => motor.MotorName.Contains(motorName) 
                            && (motor.MotorStatusId == SD.Status_Consignment || motor.MotorStatusId == SD.Status_nonConsignment)
                            && motor.StoreId == null ,
                        includeProperties: SD.GetMotorArray
                       );
                        break;
                }

                var listResponse = _mapper.Map<List<MotorResponseDTO>>(motorbikes);
                // (Boosting) 
                var listPostBoosting = await _unitOfWork.PostBoostingService.Get(x => x.Status == SD.Request_Accept
                                                                                && x.StartTime < DateTime.Now && x.EndTime > DateTime.Now);
                // (Boosting) Iterate through each MotorBike in listDatabase
                foreach (var motor in listResponse)
                {
                    // Check if the MotorBike is in the list of PostBoosting
                    var matchingPostBoosting = listPostBoosting.FirstOrDefault(p => p.MotorId == motor.MotorId);

                    //If yes, assign the PostBoosting object to the MotorBike
                    if (matchingPostBoosting != null)
                    {
                        var boostingInfor = _mapper.Map<PostBoostingCreateDTO>(matchingPostBoosting);
                        motor.Boosting = boostingInfor;
                    }
                }
                // (Boosting) Sort listDatabase by Level in descending order
                listResponse = listResponse.OrderByDescending(motor => motor.Boosting?.Level ?? 0).ToList();
                //---------------------------
                foreach (var r in listResponse)
                {
                    foreach (var PostingType in SD.RequestPostingTypeArray)
                    {
                        var request = await _unitOfWork.RequestService.GetLast(
                                e => e.MotorId == r.MotorId && e.RequestTypeId == PostingType && e.Status == SD.Request_Accept
                        );
                        if (request != null) { r.PostingAt = request?.Time; }
                    }
                }
                if (motorbikes != null && motorbikes.Any())
                {
                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Result = listResponse;
                    return Ok(_response);
                }
                else
                {
                    _response.ErrorMessages.Add("Không tìm thấy xe nào!");
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages = new List<string> { ex.ToString() };
                return BadRequest(_response);
            }
        }

        [Authorize]
        [HttpGet("filter")]
        public async Task<IActionResult> FilterMotorbikes([FromQuery] MotorFilterDTO filter)
        {
            try
            {
                var roleId = int.Parse(User.FindFirst("RoleId")?.Value);
                IEnumerable<Motorbike>? motorbikes;
                switch (roleId)
                {
                    case SD.Role_Customer_Id:
                        motorbikes = await _unitOfWork.MotorBikeService.Get(
                        expression: motor => motor.MotorStatusId != SD.Status_Storage && motor.StoreId != null,
                        includeProperties: SD.GetMotorArray
                        );
                        break;
                    default:
                        motorbikes = await _unitOfWork.MotorBikeService.Get(
                        expression: motor => motor.MotorStatusId == SD.Status_Consignment || (motor.MotorStatusId == SD.Status_nonConsignment && motor.StoreId == null),
                        includeProperties: SD.GetMotorArray
                       );
                        break;
                }

                if (filter.BrandId != null && filter.BrandId.Any())
                {
                    var brandIds = filter.BrandId;
                    var modelList = new List<int>();
                    var modelIds = _unitOfWork.MotorModelService.Get(m => brandIds.Contains((int)m.BrandId));
                    if (modelIds != null)
                    {
                        foreach (var product in await modelIds)
                        {
                            modelList.Add(product.ModelId);
                        }
                        motorbikes = motorbikes.Where(m => modelList.Contains((int)m.ModelId)).ToList();
                    }
                }

                if (filter.ModelId != null && filter.ModelId.Any())
                {
                    motorbikes = motorbikes.Where(m => (filter.ModelId).Contains((int)m.ModelId)).ToList();
                }

                if (filter.minPrice.HasValue)
                {
                    motorbikes = motorbikes.Where(m => m.Price >= filter.minPrice.Value).ToList();
                }

                if (filter.maxPrice.HasValue)
                {
                    motorbikes = motorbikes.Where(m => m.Price <= filter.maxPrice.Value).ToList();
                }

                if (filter.MotorTypeId != null && filter.MotorTypeId.Any())
                {
                    motorbikes = motorbikes.Where(m => (filter.MotorTypeId).Contains((int)m.MotorTypeId)).ToList();
                }

                var listResponse = _mapper.Map<List<MotorResponseDTO>>(motorbikes);
                // (Boosting) 
                var listPostBoosting = await _unitOfWork.PostBoostingService.Get(x => x.Status == SD.Request_Accept
                                                                                && x.StartTime < DateTime.Now && x.EndTime > DateTime.Now);
                // (Boosting) Iterate through each MotorBike in listDatabase
                foreach (var motor in listResponse)
                {
                    // Check if the MotorBike is in the list of PostBoosting
                    var matchingPostBoosting = listPostBoosting.FirstOrDefault(p => p.MotorId == motor.MotorId);

                    //If yes, assign the PostBoosting object to the MotorBike
                    if (matchingPostBoosting != null)
                    {
                        var boostingInfor = _mapper.Map<PostBoostingCreateDTO>(matchingPostBoosting);
                        motor.Boosting = boostingInfor;
                    }
                }
                // (Boosting) Sort listDatabase by Level in descending order
                listResponse = listResponse.OrderByDescending(motor => motor.Boosting?.Level ?? 0).ToList();
                //---------------------------
                foreach (var r in listResponse)
                {
                    foreach (var PostingType in SD.RequestPostingTypeArray)
                    {
                        var request = await _unitOfWork.RequestService.GetLast(
                                e => e.MotorId == r.MotorId && e.RequestTypeId == PostingType && e.Status == SD.Request_Accept
                        );
                        if (request != null) { r.PostingAt = request?.Time; }
                    }
                }
                if (motorbikes != null && motorbikes.Any())
                {
                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Result = listResponse;
                    return Ok(_response);
                }
                else
                {                    
                    _response.ErrorMessages.Add("Không tìm thấy xe nào!");
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages = new List<string> { ex.ToString() };
                return BadRequest(_response);
            }
        }

        //[HttpPut]
        //[Authorize(Roles = "Store,Owner")]
        //[Route("UpdateMotor-Owner")]
        //public async Task<IActionResult> Update_MotorOwner(int MotorID, int newUserID)
        //{
        //    try
        //    {
        //        var motor = await _unitOfWork.MotorBikeService.GetFirst(e => e.MotorId == MotorID);
        //        if (motor == null)
        //        {
        //            _response.ErrorMessages.Add("Không tìm thấy xe này!");
        //            _response.IsSuccess = false;
        //            _response.StatusCode = HttpStatusCode.NotFound;
        //            return NotFound(_response);
        //        }
        //        else
        //        {
        //            var userId = int.Parse(User.FindFirst("UserId")?.Value);
        //            var Store = await _unitOfWork.StoreDescriptionService.GetFirst(e => e.UserId == userId);
        //            if (Store.UserId != motor.StoreId && userId != motor.OwnerId)
        //            {
        //                _response.StatusCode = HttpStatusCode.BadRequest;
        //                _response.IsSuccess = false;
        //                _response.ErrorMessages.Add("Xe không thuộc quyền của người dùng!");
        //                return BadRequest(_response);
        //            }
        //            else
        //            {
        //                Request request = new()
        //                {
        //                    MotorId = MotorID,
        //                    ReceiverId = motor.OwnerId,
        //                    SenderId = newUserID,
        //                    Time = DateTime.Now,
        //                    RequestTypeId = SD.Request_MotorTranfer_Id,
        //                    Status = SD.Request_Accept
        //                };
        //                await _unitOfWork.RequestService.Add(request);

        //                motor.OwnerId = newUserID;
        //                motor.StoreId = null;
        //                motor.MotorStatusId = SD.Status_Storage;

        //                await _unitOfWork.MotorBikeService.Update(motor);
        //                _response.IsSuccess = true;
        //                _response.StatusCode = HttpStatusCode.OK;
        //                _response.Result = motor;
        //            }
        //            return Ok(_response);
        //        }
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

        [HttpPut]
        [Authorize(Roles = "Store,Owner")]
        [Route("UpdateMotor-Status")]
        public async Task<IActionResult> UpdateMotorStatus(int MotorID, int StatusID)
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
                else if (StatusID == 0)
                {
                    _response.ErrorMessages.Add("Vui lòng chọn trạng thái đăng bán!");
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                else
                {
                    var userId = int.Parse(User.FindFirst("UserId")?.Value);
                    var roleId = int.Parse(User.FindFirst("RoleId")?.Value);
                    var Store = await _unitOfWork.StoreDescriptionService.GetFirst(e => e.UserId == userId);
                    int StoreID = (Store == null) ? 0 : Store.StoreId;
                    if (StoreID != obj.StoreId && userId != obj.OwnerId)
                    {
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.IsSuccess = false;
                        _response.ErrorMessages.Add("Xe không thuộc quyền của người dùng!");
                        return BadRequest(_response);
                    }
                    else if (roleId == SD.Role_Store_Id && obj.StoreId == null && SD.Posting_MotorStatusArray.Contains(StatusID))
                    {
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.IsSuccess = false;
                        _response.ErrorMessages.Add("Xe thiếu thông tin cửa hàng!");
                        return BadRequest(_response);
                    }                   
                    else
                    {
                        var rs = InputValidation.PostingTypeByRole(roleId,StatusID);
                        if (rs != "") 
                        {
                            _response.IsSuccess = false;
                            _response.StatusCode = HttpStatusCode.BadRequest;
                            _response.ErrorMessages.Add(rs);
                            return BadRequest(_response);
                        }
                        //Check MotorStatusPosting if not same with OwnerPosting
                        foreach (var PostingType in SD.Owner_RequestPostingTypeArray)
                        {
                            var request_Posting = await _unitOfWork.RequestService.GetLast(
                                    e => e.MotorId == MotorID && e.RequestTypeId == PostingType 
                                    && e.SenderId == obj.OwnerId &&  e.Status == SD.Request_Accept
                            );
                            if (request_Posting != null)
                            {
                                if ((request_Posting.RequestTypeId == SD.Request_Motor_Consignment && StatusID != SD.Status_Consignment) ||
                                    (request_Posting.RequestTypeId == SD.Request_Motor_nonConsignment && StatusID != SD.Status_nonConsignment))
                                {
                                    _response.StatusCode = HttpStatusCode.BadRequest;
                                    _response.IsSuccess = false;
                                    _response.ErrorMessages.Add("Không thể đăng xe khác với hợp đồng mua bán với chủ xe!");
                                    return BadRequest(_response);
                                }
                            }
                        }
                        //-------------------------
                        Request request = new()
                        {
                            MotorId = MotorID,
                            ReceiverId = SD.AdminID,
                            SenderId = userId,
                            Time = DateTime.Now,
                            RequestTypeId = null,
                            Status = SD.Request_Accept
                        };
                        
                        switch (StatusID)
                        {
                            case SD.Status_Posting:
                                request.RequestTypeId = SD.Request_Motor_Posting;
                                break;
                            case SD.Status_Consignment:
                                request.RequestTypeId = SD.Request_Motor_Consignment;
                                break;
                            case SD.Status_nonConsignment:
                                request.RequestTypeId = SD.Request_Motor_nonConsignment;
                                break;
                            default:
                                break;
                        }
                        if (StatusID != SD.Status_Storage) 
                        { 
                            await _unitOfWork.RequestService.Add(request);
                        }
                        obj.MotorStatusId = StatusID;
                        await _unitOfWork.MotorBikeService.Update(obj);
                        _response.IsSuccess = true;
                        _response.StatusCode = HttpStatusCode.OK;
                        _response.Result = obj;
                    }
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
        [Authorize(Roles = "Store,Owner")]
        [Route("CancelPosting")]
        public async Task<IActionResult> CancelPosting(int MotorID)
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
                    var roleId = int.Parse(User.FindFirst("RoleId")?.Value);
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
                        int check = 0;
                        foreach (var PostingType in SD.RequestPostingTypeArray) 
                        {
                            var request = await _unitOfWork.RequestService.GetFirst(
                                    e => e.MotorId == MotorID && e.RequestTypeId == PostingType && e.Status == SD.Request_Accept
                            );
                            if (request != null ) { check += 1; }
                        }
                        if (check == 0)
                        {
                            _response.StatusCode = HttpStatusCode.BadRequest;
                            _response.IsSuccess = false;
                            _response.ErrorMessages.Add("Xe chưa được đăng lên sàn!");
                            return BadRequest(_response);
                        }
                        //Cancel all request except Register and Negotiation 
                        IEnumerable<Request>? requestEdit = null;
                        switch (roleId)
                        {
                            case SD.Role_Store_Id:
                                requestEdit = await _unitOfWork.RequestService.Get(e => e.MotorId == MotorID
                                                                           && e.RequestTypeId != SD.Request_Motor_Register
                                                                           && e.RequestTypeId != SD.Request_Negotiation_Id
                                                                           && e.SenderId == userId
                                );
                                break;
                            case SD.Role_Owner_Id:
                                requestEdit = await _unitOfWork.RequestService.Get(e => e.MotorId == MotorID
                                                                           && e.RequestTypeId != SD.Request_Motor_Register
                                );
                                break;
                        }
                        if (requestEdit != null)
                        {
                            foreach (var r in requestEdit)
                            {
                                r.Status = SD.Request_Cancel;
                                await _unitOfWork.RequestService.Update(r);
                            }
                        }
                        //Update Motor
                        obj.MotorStatusId = SD.Status_Storage;
                        if (roleId == SD.Role_Owner_Id) 
                        {
                            obj.StoreId = null;
                        }
                        await _unitOfWork.MotorBikeService.Update(obj);
                        _response.IsSuccess = true;
                        _response.StatusCode = HttpStatusCode.OK;
                        _response.Result = obj;
                    }
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
        [Authorize(Roles = "Store,Owner")]
        [Route("UpdateMotor")]
        public async Task<IActionResult> UpdateMotor(int MotorID, [FromForm] MotorUpdateDTO motor, List<IFormFile> images)
        {
            try
            {
                var rs = InputValidation.MotorValidation(motor);
                if (rs != "")
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages.Add(rs);
                    return BadRequest(_response);
                }
                var obj = await _unitOfWork.MotorBikeService.GetFirst(e => e.MotorId == MotorID);
                if (obj == null)
                {
                    _response.ErrorMessages.Add("Không tìm thấy xe này!");
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                //Check if Motor is Posting
                int check = 0;
                foreach (var PostingType in SD.RequestPostingTypeArray)
                {
                    var request = await _unitOfWork.RequestService.GetLast(
                            e => e.MotorId == MotorID && e.RequestTypeId == PostingType && e.Status == SD.Request_Accept
                    );
                    
                    if (request != null) 
                    {
                        var roleId = int.Parse(User.FindFirst("RoleId")?.Value);
                        switch (roleId)
                        {
                            case SD.Role_Store_Id:
                                if (request.SenderId == obj.StoreId) check += 1;
                                break;
                            case SD.Role_Owner_Id:
                                if (request.SenderId == obj.OwnerId && obj.StoreId == null) check += 1;
                                break;
                        }
                    }
                }
                if (check > 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("Xe đang được đăng bán. Cần gỡ bài trước khi cập nhật xe!");
                    return BadRequest(_response);
                }
                //Check have any Negotiation request 
                var requestEdit = await _unitOfWork.RequestService.Get(e => e.MotorId == MotorID && e.RequestTypeId == SD.Request_Negotiation_Id && e.Status == SD.Request_Pending);
                if (requestEdit.Count() > 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("Xe đang trong quá trình thương lượng giá cả. Cần gỡ bài trước khi cập nhật xe!");
                    return BadRequest(_response);
                }
                //-----------------------------------
                if (motor.MotorStatusId == 1 && motor.StoreId == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("Xe được đăng bán cần có thông tin cửa hàng!");
                    return BadRequest(_response);
                }
                else
                {
                    bool allFilesAreImages = images.All(file => InputValidation.IsImage(file));
                    if (!allFilesAreImages || !InputValidation.IsImage(motor.RegistrationImage))
                    {
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.Result = false;
                        _response.ErrorMessages.Add("Ảnh không tồn tại hoặc định dạng không phù hợp!");
                        return BadRequest(_response);
                    }
                    else if (images.Count > 30)
                    {
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.Result = false;
                        _response.ErrorMessages.Add("Bạn chỉ được tải lên tối đa 30 hình ảnh.");
                        return BadRequest(_response);
                    }
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
                        _mapper.Map(motor, obj);
                        //Delete oldRegistration Image
                        if (motor.RegistrationImage != null && motor.RegistrationImage.Length > 0)
                        {
                            var reglink = obj.RegistrationImage.Split('/').Last();
                            await _blobService.DeleteBlob(reglink, SD.Storage_Container);
                            string rgtfileName = $"{Guid.NewGuid()}{Path.GetExtension(motor.RegistrationImage.FileName)}";
                            var rgtLink = await _blobService.UploadBlob(rgtfileName, SD.Storage_Container, motor.RegistrationImage);
                            obj.RegistrationImage = rgtLink;
                        }
                        //---------------------------                      
                        await _unitOfWork.MotorBikeService.Update(obj);

                        if (motor != null)
                        {
                            if (images != null && images.Count > 0)
                            {
                                var oldImages = await _unitOfWork.MotorImageService.Get(x => x.MotorId == MotorID);
                                foreach (var oldImg in oldImages)
                                {
                                    var link = oldImg.ImageLink.Split('/').Last();
                                    await _blobService.DeleteBlob(link, SD.Storage_Container);
                                    await _unitOfWork.MotorImageService.Delete(oldImg);
                                }
                                foreach (var p in images)
                                {
                                    string fileName = $"{Guid.NewGuid()}{Path.GetExtension(p.FileName)}";
                                    var img = await _blobService.UploadBlob(fileName, SD.Storage_Container, p);
                                    MotorbikeImage image = new()
                                    {
                                        ImageLink = img,
                                        MotorId = MotorID
                                    };
                                    await _unitOfWork.MotorImageService.Add(image);
                                }
                            }
                        }
                        

                        _response.IsSuccess = true;
                        _response.StatusCode = HttpStatusCode.OK;
                        _response.Result = obj;
                    }
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
        [Authorize(Roles = "Store,Owner")]
        [Route("MotorRegister")]
        public async Task<IActionResult> MotorRegister([FromForm] MotorRegisterDTO motor, List<IFormFile> images)
        {
            try
            {
                var rs = InputValidation.MotorValidation(_mapper.Map<MotorUpdateDTO>(motor));
                if (rs != "")
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages.Add(rs);
                    return BadRequest(_response);
                }
                var CertNum = await _unitOfWork.MotorBikeService.GetFirst(c => c.CertificateNumber == motor.CertificateNumber);
                if (CertNum != null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages.Add("Mã số chứng nhận đăng ký xe đã tồn tại!");
                    return BadRequest(_response);
                }
                else
                {
                    bool allFilesAreImages = images.All(file => InputValidation.IsImage(file));
                    if (!allFilesAreImages || !InputValidation.IsImage(motor.RegistrationImage))
                    {
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.Result = false;
                        _response.ErrorMessages.Add("Ảnh không tồn tại hoặc định dạng không phù hợp!");
                        return BadRequest(_response);
                    }
                    else if (images.Count > 30)
                    {
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.Result = false;
                        _response.ErrorMessages.Add("Bạn chỉ được tải lên tối đa 30 hình ảnh.");
                        return BadRequest(_response);
                    }
                    var newMotor = _mapper.Map<Motorbike>(motor);
                    newMotor.MotorStatusId = SD.Status_Storage;
                    //Registration-Img
                    string rgtfileName = $"{Guid.NewGuid()}{Path.GetExtension(motor.RegistrationImage.FileName)}";
                    var imgLink = await _blobService.UploadBlob(rgtfileName, SD.Storage_Container, motor.RegistrationImage);
                    newMotor.RegistrationImage = imgLink;
                    //----------------
                    var userID = int.Parse(User.FindFirst("UserId")?.Value);
                    //Add StoreID
                    var store = await _unitOfWork.StoreDescriptionService.GetFirst(c => c.UserId == userID);
                    if (store != null)
                    {
                        newMotor.StoreId = store.StoreId;
                    }
                    else
                    {
                        newMotor.StoreId = null;
                    }
                    newMotor.OwnerId = userID; 
                    await _unitOfWork.MotorBikeService.Add(newMotor);
                    //Add list Image
                    var motorInDb = await _unitOfWork.MotorBikeService.GetFirst(c => c.CertificateNumber == motor.CertificateNumber);
                    foreach (var p in images)
                    {
                        if (motor != null)
                        {
                            string fileName = $"{Guid.NewGuid()}{Path.GetExtension(p.FileName)}";
                            var img = await _blobService.UploadBlob(fileName, SD.Storage_Container, p);
                            MotorbikeImage image = new()
                            {
                                ImageLink = img,
                                MotorId = motorInDb.MotorId
                            };
                            await _unitOfWork.MotorImageService.Add(image);

                        }
                    }
                    //Add Request
                    Request request = new()
                    {
                        MotorId = motorInDb.MotorId,
                        ReceiverId = SD.AdminID,
                        SenderId = userID,
                        Time = DateTime.Now,
                        RequestTypeId = SD.Request_Motor_Register,
                        Status = SD.Request_Accept
                    };
                    await _unitOfWork.RequestService.Add(request);
                    //-----------
                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Result = newMotor;
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
