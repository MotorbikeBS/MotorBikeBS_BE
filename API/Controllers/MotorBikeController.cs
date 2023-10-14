using API.DTO;
using API.DTO.FilterDTO;
using API.DTO.MotorbikeDTO;
using API.DTO.UserDTO;
using API.Utility;
using API.Validation;
using AutoMapper;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.BlobImageService;
using Service.UnitOfWork;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
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

        public MotorBikeController(IUnitOfWork unitOfWork, IMapper mapper, IBlobService blobService)
        {
            _unitOfWork = unitOfWork;
            _response = new ApiResponse();
            _mapper = mapper;
            _blobService = blobService;
        }

       
        [HttpGet("GetAllWithSpecificStatus")]
        [Authorize(Roles = "Store,Owner")]
        public async Task<IActionResult> GetAllWithSpecificStatus(int StatusID )
        {
            try
            {
                var listDatabase = await _unitOfWork.MotorBikeService.Get(e => e.MotorStatusId == StatusID, SD.GetMotorArray);
                var listResponse = _mapper.Map<List<MotorResponseDTO>>(listDatabase);
                if (listDatabase == null || listDatabase.Count() <= 0)
                {
                    _response.ErrorMessages.Add("Không tìm thấy xe nào!");
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
        [HttpGet("GetAllOnExchange")]
        public async Task<IActionResult> GetAllOnExchange()
        {
            try
            {  
                var listDatabase = await _unitOfWork.MotorBikeService.Get(e => e.MotorStatusId ==SD.Status_Posting, SD.GetMotorArray);
                var listResponse = _mapper.Map<List<MotorResponseDTO>>(listDatabase);
                listResponse.ForEach(item => item.Owner = null);
                if (listDatabase == null || listDatabase.Count() <= 0)
                {
                    _response.ErrorMessages.Add("Không tìm thấy xe nào!");
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
        [HttpGet("GetAllOnStoreExchange")]
        public async Task<IActionResult> GetAllOnStoreExchange()
        {
            try
            {
                var listDatabase = await _unitOfWork.MotorBikeService.Get(e => e.MotorStatusId == SD.Status_Consignment || e.MotorStatusId == SD.Status_Livelihood, SD.GetMotorArray);
                var listResponse = _mapper.Map<List<MotorResponseDTO>>(listDatabase);
                listResponse.ForEach(item => item.Store = null);
                if (listDatabase == null || listDatabase.Count() <= 0)
                {
                    _response.ErrorMessages.Add("Không tìm thấy xe nào!");
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
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
        [HttpGet("GetMotorByStoreId")]
        public async Task<IActionResult> GetByStoreId(int StoreID)
        {
            try
            {
                var listDatabase = await _unitOfWork.MotorBikeService.Get(e => e.StoreId == StoreID,SD.GetMotorArray);
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
                var listResponse = _mapper.Map<MotorResponseDTO>(listDatabase);
                if (listDatabase == null)
                {
                    _response.ErrorMessages.Add("Không tìm thấy xe nào!");
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
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
        [HttpGet("search-MotorName")]
        public async Task<IActionResult> SearchByMotorName(string motorName)
        {
            try
            {
                var roleId = int.Parse(User.FindFirst("RoleId")?.Value);
                IEnumerable<Motorbike>? results;
                switch (roleId)
                {
                    case SD.Role_Customer_Id:
                        results = await _unitOfWork.MotorBikeService.Get(
                        expression: motor => motor.MotorName.Contains(motorName) && motor.MotorStatusId == SD.Status_Posting,
                        includeProperties: SD.GetMotorArray
                        );
                        break;
                    default:
                        results = await _unitOfWork.MotorBikeService.Get(
                       expression: motor => motor.MotorName.Contains(motorName) && (motor.MotorStatusId == SD.Status_Consignment || motor.MotorStatusId == SD.Status_Livelihood),
                       includeProperties: SD.GetMotorArray
                       );
                        break;
                }

                var listResponse = _mapper.Map<List<MotorResponseDTO>>(results);
                if (results != null && results.Any())
                {
                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Result = listResponse;
                    return Ok(_response);
                }
                else
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.ErrorMessages.Add("Không tìm thấy xe nào.");
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
        public async Task<IActionResult> FilterMotorbikes([FromForm] MotorFilterDTO filter)
        {
            try
            {
                var roleId = int.Parse(User.FindFirst("RoleId")?.Value);
                IEnumerable<Motorbike>? motorbikes;
                switch (roleId)
                {
                    case SD.Role_Customer_Id:
                        motorbikes = await _unitOfWork.MotorBikeService.Get(
                        expression: motor => motor.MotorStatusId == SD.Status_Posting,
                        includeProperties: SD.GetMotorArray
                        );
                        break;
                    default:
                         motorbikes = await _unitOfWork.MotorBikeService.Get(
                        expression: motor => motor.MotorStatusId == SD.Status_Consignment || motor.MotorStatusId == SD.Status_Livelihood,
                        includeProperties: SD.GetMotorArray
                        );
                        break;
                }

                if (filter.ModelId != null && filter.ModelId.Any())
                {
                    motorbikes = motorbikes.Where(m => (filter.ModelId).Contains((int) m.ModelId)).ToList();
                }

                if (filter.minPrice.HasValue)
                {
                    motorbikes = motorbikes.Where(m => m.Price >= filter.minPrice.Value).ToList();
                }

                if (filter.maxPrice.HasValue)
                {
                    motorbikes = motorbikes.Where(m => m.Price <= filter.maxPrice.Value).ToList();
                }

                if (filter.MotorTypeId.HasValue)
                {
                    motorbikes = motorbikes.Where(m => m.MotorTypeId == filter.MotorTypeId).ToList();
                }

                var listResponse = _mapper.Map<List<MotorResponseDTO>>(motorbikes);
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
                else
                {
                    var userId = int.Parse(User.FindFirst("UserId")?.Value);
                    if (userId != obj.StoreId && userId != obj.OwnerId)
                    {
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.Result = false;
                        _response.ErrorMessages.Add("Xe không thuộc quyền của người dùng!");
                        return BadRequest(_response);
                    }
                    else
                    {
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
        [Route("UpdateMotor")]
        public async Task<IActionResult> UpdateMotor(int MotorID, [FromForm] MotorUpdateDTO motor, List<IFormFile> images)
        {
            try
            {
                var rs = InputValidation.MotorValidation(motor);
                if (rs != "")
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Result = false;
                    _response.ErrorMessages.Add(rs);
                    return BadRequest(_response);
                }
                if (motor.MotorStatusId == 1 && motor.StoreId == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Result = false;
                    _response.ErrorMessages.Add("Xe được đăng bán cần có thông tin cửa hàng!");
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
                else
                {
                    var userId = int.Parse(User.FindFirst("UserId")?.Value);
                    if (userId != obj.StoreId && userId != obj.OwnerId)
                    {
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.Result = false;
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
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Result = false;
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
                    var newMotor = _mapper.Map<Motorbike>(motor);
                    newMotor.MotorStatusId = SD.Status_Storage;
                    //Registration-Img
                    string rgtfileName = $"{Guid.NewGuid()}{Path.GetExtension(motor.RegistrationImage.FileName)}";
                    var imgLink = await _blobService.UploadBlob(rgtfileName, SD.Storage_Container, motor.RegistrationImage);
                    newMotor.RegistrationImage = imgLink;
                    //----------------
                    newMotor.OwnerId = int.Parse(User.FindFirst("UserId")?.Value); 
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
