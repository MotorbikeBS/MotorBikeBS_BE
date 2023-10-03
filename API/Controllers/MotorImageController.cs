using API.DTO;
using API.Utility;
using AutoMapper;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.BlobImageService;
using Service.UnitOfWork;
using System.Collections.Generic;
using System.Data;
using System.Net;
using static System.Net.Mime.MediaTypeNames;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MotorImageController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private ApiResponse _response;
        private readonly IMapper _mapper;
        private readonly IBlobService _blobService;

        public MotorImageController(IUnitOfWork unitOfWork, IMapper mapper, IBlobService blobService)
        {
            _unitOfWork = unitOfWork;
            _response = new ApiResponse();
            _mapper = mapper;
            _blobService = blobService;
        }

        //[Authorize]
        [HttpGet]
        public async Task<IActionResult> GetByMotorId(int MotorID)
        {
            try
            {
                var list = await _unitOfWork.MotorImageService.Get(e => e.MotorId == MotorID);
                var listResponse = _mapper.Map<List<ImageResponseDTO>>(list);
                if (list == null || list.Count() <= 0)
                {
                    _response.ErrorMessages.Add("Không tìm thấy ảnh nào!");
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
        public async Task<IActionResult> GetByImageId(int ImageID)
        {
            try
            {
                var obj = await _unitOfWork.MotorImageService.GetFirst(e => e.ImageId == ImageID);
                var objResponse = _mapper.Map<ImageResponseDTO>(obj);
                if (obj == null)
                {
                    _response.ErrorMessages.Add("Không tồn tại xe này!");
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                else
                {
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
        [HttpPut]
        //[Authorize(Roles = "Store,Owner")]
        public async Task<IActionResult> UpdateImage([FromForm] int id, List<IFormFile> images)
        {
            try
            {
                var motor = await _unitOfWork.MotorBikeService.GetFirst(c => c.MotorId == id);
                if (motor == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.ErrorMessages.Add("Không tồn tại xe này!");
                    return NotFound(_response);
                }
                else
                {
                    var userId = int.Parse(User.FindFirst("UserId")?.Value);
                    if (userId != motor.StoreId && userId != motor.OwnerId)
                    {
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.Result = false;
                        _response.ErrorMessages.Add("Xe không thuộc quyền của người dùng!");
                        return BadRequest(_response);
                    }
                    else
                    {
                        foreach (var p in images)
                        {
                            if (motor != null)
                            {
                                var oldImg = await _unitOfWork.MotorImageService.GetFirst(x => x.ImageId == id);
                                var link = oldImg.ImageLink.Split('/').Last();

                                await _blobService.DeleteBlob(link, SD.Storage_Container);
                                await _unitOfWork.MotorImageService.Delete(oldImg);

                                string fileName = $"{Guid.NewGuid()}{Path.GetExtension(p.FileName)}";
                                var img = await _blobService.UploadBlob(fileName, SD.Storage_Container, p);
                                MotorbikeImage image = new()
                                {
                                    ImageLink = img,
                                    MotorId = motor.MotorId
                                };
                                await _unitOfWork.MotorImageService.Add(image);

                            }
                        }
                        _response.IsSuccess = true;
                        _response.StatusCode = HttpStatusCode.OK;
                        _response.Result = images;
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
        //[Authorize(Roles = "Store,Owner")]
        [Route("image-register")]
        public async Task<IActionResult> ImageRegister(int id, List<IFormFile> images)
        {
            try
            {
                var motor = await _unitOfWork.MotorBikeService.GetFirst(c => c.MotorId == id);
                if (motor == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.ErrorMessages.Add("Không tồn tại xe này!");
                    return NotFound(_response);
                }
                else
                {
                    var userId = int.Parse(User.FindFirst("UserId")?.Value);
                    if (userId != motor.StoreId && userId != motor.OwnerId)
                    {
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.Result = false;
                        _response.ErrorMessages.Add("Xe không thuộc quyền của người dùng!");
                        return BadRequest(_response);
                    }
                    else
                    {
                        foreach (var p in images)
                        {
                            if (motor != null)
                            {
                                string fileName = $"{Guid.NewGuid()}{Path.GetExtension(p.FileName)}";
                                var img = await _blobService.UploadBlob(fileName, SD.Storage_Container, p);
                                MotorbikeImage image = new()
                                {
                                    ImageLink = img,
                                    MotorId = motor.MotorId
                                };
                                await _unitOfWork.MotorImageService.Add(image);

                            }
                        }
                        _response.IsSuccess = true;
                        _response.StatusCode = HttpStatusCode.OK;
                        _response.Result = images;
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
    }
}
