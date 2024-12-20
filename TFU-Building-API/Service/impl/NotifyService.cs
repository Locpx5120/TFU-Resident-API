﻿using BuildingModels;
using Constant;
using Core.Enums;
using Core.Model;
using fake_tool.Helpers;
using Microsoft.EntityFrameworkCore;
using TFU_Building_API.Core.Dapper.Noti;
using TFU_Building_API.Core.Infrastructure;
using TFU_Building_API.Dto;

namespace TFU_Building_API.Service.impl
{
    public class NotifyService : INotifyService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _env;
        private readonly INotifyRepository _notifyRepository;
        private readonly IUserIdentity _userIdentity;

        public NotifyService(IUnitOfWork unitOfWork, IWebHostEnvironment env
           , IUserIdentity userIdentity
           , INotifyRepository notifyRepository)
        {
            _unitOfWork = unitOfWork;
            _env = env;
            _notifyRepository = notifyRepository;
            _userIdentity = userIdentity;
        }

        public async Task<ResponseData<CreateNotifyResponseDto>> CreateNotifyAsync(CreateNotifyRequestDto request)
        {
            try
            {
                string imageUrl = null;
                ImgBase imgBase = new ImgBase();

                // Lưu ảnh vào thư mục nếu người dùng tải lên
                if (request.Image != null)
                {
                    //// Định nghĩa đường dẫn thư mục lưu trữ ảnh
                    //var uploadPath = Path.Combine(_env.ContentRootPath, "uploads", "images");
                    //if (!Directory.Exists(uploadPath))
                    //{
                    //    Directory.CreateDirectory(uploadPath);
                    //}

                    //// Tạo tên file duy nhất để tránh trùng lặp
                    //var fileName = $"{Guid.NewGuid()}{Path.GetExtension(request.Image.FileName)}";
                    //var filePath = Path.Combine(uploadPath, fileName);

                    //// Lưu file lên server
                    //using (var stream = new FileStream(filePath, FileMode.Create))
                    //{
                    //    await request.Image.CopyToAsync(stream);
                    //}

                    //// Tạo URL để truy cập ảnh
                    //imageUrl = $"/uploads/images/{fileName}";

                    imgBase.Id = Guid.NewGuid();
                    imgBase.Base64 = await Utill.ConvertImageToBase64(request.Image);
                    imgBase.FileName = request.Image.FileName;
                    imgBase.Name = request.Image.Name;
                    imgBase.ContentType = request.Image.ContentType;
                    imgBase.ContentDisposition = request.Image.ContentDisposition;
                    imgBase.Length = request.Image.Length;

                    _unitOfWork.ImgBaseRepository.Add(imgBase);
                }

                // Khởi tạo bản tin mới
                var newNotify = new Notify
                {
                    Id = Guid.NewGuid(),
                    ApplyDate = request.ApplyDate,
                    NotificationType = request.NotificationType,
                    BuildingId = request.BuildingId,
                    RoleId = request.RoleId,
                    Title = request.Title,
                    ShortContent = request.ShortContent,
                    LongContent = request.LongContent,
                    Status = request.Status
                };

                if (imgBase.Id != null && imgBase.Id != Guid.Empty)
                {
                    newNotify.ImgBaseId = imgBase.Id;
                }

                // Thêm vào cơ sở dữ liệu
                _unitOfWork.NotifyRepository.Add(newNotify);
                await _unitOfWork.SaveChangesAsync();

                // Tạo response
                var response = new CreateNotifyResponseDto
                {
                    Id = newNotify.Id,
                    Message = "Bản tin đã được tạo thành công"
                };

                return new ResponseData<CreateNotifyResponseDto>
                {
                    Success = true,
                    Message = "Bản tin đã được tạo thành công.",
                    Data = response,
                    Code = (int)ErrorCodeAPI.OK
                };
            }
            catch (Exception ex)
            {
                return new ResponseData<CreateNotifyResponseDto>
                {
                    Success = false,
                    Message = $"An error occurred: {ex.Message}",
                    Code = (int)ErrorCodeAPI.SystemIsError
                };
            }
        }

        public async Task<ResponseData<PaginatedResponseDto<NotifyResponseDto>>> GetNotifiesAsync(NotifyFilterRequestDto request)
        {
            try
            {
                // Stage 1: Fetch data from the database
                var query = await _notifyRepository.GetNotifies(request);



                // Calculate total records before pagination
                var totalRecords = query.ToList().Count();

                // Apply pagination
                var paginatedQuery = query.ToList()
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize);

                // Stage 2: Fetch the data into memory
                var dataList = paginatedQuery.ToList();

                // Stage 3: In-memory transformations
                var resultData = new List<NotifyResponseDto>();
                foreach (var item in dataList)
                {
                    var notifyResponse = new NotifyResponseDto
                    {
                        Id = item.Id,
                        Title = item.Title,
                        ShortContent = item.ShortContent,
                        Date = item.Date,
                        NotificationType = item.NotificationType,
                        BuildingName = item.BuildingName,
                        RoleName = item.RoleName,
                        Status = item.Status,
                        CreatedBy = item.CreatedBy,
                        ApprovedBy = item.ApprovedBy,
                        BuildingId = item.BuildingId,
                        ImgBaseId = item.ImgBaseId,
                        NoteReject = item.NoteReject,
                    };
                    resultData.Add(notifyResponse);
                }

                // Wrap the result in PaginatedResponseDto
                var response = new PaginatedResponseDto<NotifyResponseDto>
                {
                    TotalRecords = totalRecords,
                    Data = resultData
                };

                return new ResponseData<PaginatedResponseDto<NotifyResponseDto>>
                {
                    Success = true,
                    Message = "Successfully retrieved notify list.",
                    Data = response,
                    Code = (int)ErrorCodeAPI.OK
                };
            }
            catch (Exception ex)
            {
                return new ResponseData<PaginatedResponseDto<NotifyResponseDto>>
                {
                    Success = false,
                    Message = ex.Message,
                    Code = (int)ErrorCodeAPI.SystemIsError
                };
            }
        }

        public async Task<ResponseData<List<NotifyResponseDto>>> GetNotifiesByUserAsync()
        {
            try
            {
                // Stage 1: Fetch data from the database
                var query = await _notifyRepository.GetNotifiesByUser();


                // Stage 3: In-memory transformations
                var resultData = new List<NotifyResponseDto>();
                foreach (var item in query.ToList())
                {
                    var notifyResponse = new NotifyResponseDto
                    {
                        Id = item.Id,
                        Title = item.Title,
                        ShortContent = item.ShortContent,
                        Date = item.Date,
                        NotificationType = item.NotificationType,
                        BuildingName = item.BuildingName,
                        RoleName = item.RoleName,
                        Status = item.Status,
                        CreatedBy = item.CreatedBy,
                        ApprovedBy = item.ApprovedBy,
                        BuildingId = item.BuildingId,
                        ImgBaseId = item.ImgBaseId,
                        LongContent = item.LongContent,
                        NoteReject = item.NoteReject,
                    };
                    resultData.Add(notifyResponse);
                }

                return new ResponseData<List<NotifyResponseDto>>
                {
                    Success = true,
                    Message = "Successfully retrieved notify list.",
                    Data = resultData,
                    Code = (int)ErrorCodeAPI.OK
                };
            }
            catch (Exception ex)
            {
                return new ResponseData<List<NotifyResponseDto>>
                {
                    Success = false,
                    Message = ex.Message,
                    Code = (int)ErrorCodeAPI.SystemIsError
                };
            }
        }


        public async Task<ResponseData<NotifyDetailResponseDto>> GetNotifiesDetailAsync(Guid notifyId)
        {
            try
            {
                // Stage 1: Fetch data from the database
                var query = await _notifyRepository.GetNotifiesDetails(notifyId);


                return new ResponseData<NotifyDetailResponseDto>
                {
                    Success = true,
                    Message = "Successfully retrieved notify list.",
                    Data = query.First(),
                    Code = (int)ErrorCodeAPI.OK
                };
            }
            catch (Exception ex)
            {
                return new ResponseData<NotifyDetailResponseDto>
                {
                    Success = false,
                    Message = ex.Message,
                    Code = (int)ErrorCodeAPI.SystemIsError
                };
            }
        }

        public async Task<ResponseData<NotifyDetailResponseDto>> ApplyingNotifyAsync(Guid id)
        {
            try
            {
                // Stage 1: Fetch data from the database
                var data = _unitOfWork.NotifyRepository.GetById(id);
                if (data == null)
                {
                    return new ResponseData<NotifyDetailResponseDto>
                    {
                        Success = false,
                        Message = "Not found Noty.",
                        Data = null,
                        Code = (int)ErrorCodeAPI.OK
                    };
                }
                data.Status = Constants.NOTY_PENDING_APPLY;
                data.UserAccpectId = _userIdentity.UserId;
                if (await _unitOfWork.SaveChangesAsync() > 0)
                {
                    return new ResponseData<NotifyDetailResponseDto>
                    {
                        Success = true,
                        Message = "Successfully update notify.",
                        Data = null,
                        Code = (int)ErrorCodeAPI.OK
                    };
                };
            }
            catch (Exception ex)
            {
                return new ResponseData<NotifyDetailResponseDto>
                {
                    Success = false,
                    Message = ex.Message,
                    Code = (int)ErrorCodeAPI.SystemIsError
                };
            }

            return new ResponseData<NotifyDetailResponseDto>
            {
                Success = false,
                Message = "Liên hệ admin check lại code #_#",
                Code = (int)ErrorCodeAPI.SystemIsError
            };
        }

        public async Task<ResponseData<NotifyDetailResponseDto>> RejectNotifyAsync(NotifyRejectRequestDto rejectRequestDto)
        {
            try
            {
                // Stage 1: Fetch data from the database
                var data = _unitOfWork.NotifyRepository.GetById(rejectRequestDto.Id);
                if (data == null)
                {
                    return new ResponseData<NotifyDetailResponseDto>
                    {
                        Success = false,
                        Message = "Not found Noty.",
                        Data = null,
                        Code = (int)ErrorCodeAPI.OK
                    };
                }
                data.Status = Constants.NOTY_REJECT;
                data.NoteReject = rejectRequestDto.Note;
                if (await _unitOfWork.SaveChangesAsync() > 0)
                {
                    return new ResponseData<NotifyDetailResponseDto>
                    {
                        Success = true,
                        Message = "Successfully update notify.",
                        Data = null,
                        Code = (int)ErrorCodeAPI.OK
                    };
                };
            }
            catch (Exception ex)
            {
                return new ResponseData<NotifyDetailResponseDto>
                {
                    Success = false,
                    Message = ex.Message,
                    Code = (int)ErrorCodeAPI.SystemIsError
                };
            }

            return new ResponseData<NotifyDetailResponseDto>
            {
                Success = false,
                Message = "Liên hệ admin check lại code #_#",
                Code = (int)ErrorCodeAPI.SystemIsError
            };
        }


        public async Task<ResponseData<NotifyDetailResponseDto>> UpdateNotifyAsync(NotifyUpdateRequestDto requestDto)
        {
            try
            {
                // Stage 1: Fetch data from the database
                var data = _unitOfWork.NotifyRepository.GetById(requestDto.Id);
                if (data == null)
                {
                    return new ResponseData<NotifyDetailResponseDto>
                    {
                        Success = false,
                        Message = "Not found Noty.",
                        Data = null,
                        Code = (int)ErrorCodeAPI.OK
                    };
                }
                if (requestDto.Status.Equals(Constants.NOTY_APPLYING))
                {
                    if (_userIdentity.RoleName.Equals(Constants.ROLE_BAN_QUAN_LY))
                    {
                        data.Status = requestDto.Status;
                        _unitOfWork.NotifyRepository.Update(data);

                        if (await _unitOfWork.SaveChangesAsync() > 0)
                        {
                            return new ResponseData<NotifyDetailResponseDto>
                            {
                                Success = true,
                                Message = "Successfully update notify.",
                                Data = null,
                                Code = (int)ErrorCodeAPI.OK
                            };
                        };
                    }
                    else
                    {
                        return new ResponseData<NotifyDetailResponseDto>
                        {
                            Success = false,
                            Message = $"Role {_userIdentity.RoleName} not APPLYING",
                            Data = null,
                            Code = (int)ErrorCodeAPI.OK
                        };
                    }
                }

                data.Status = requestDto.Status;
                data.ApplyDate = requestDto.ApplyDate;
                data.NotificationType = requestDto.NotificationType;
                data.BuildingId = requestDto.BuildingId;
                if (requestDto.RoleId != null || requestDto.RoleId != Guid.Empty)
                {
                    data.RoleId = null;
                }
                data.Title = requestDto.Title;
                data.ShortContent = requestDto.ShortContent;
                data.LongContent = requestDto.LongContent;

                if (requestDto.Image != null)
                {
                    ImgBase imgBase = new ImgBase();
                    imgBase.Id = Guid.NewGuid();
                    imgBase.Base64 = await Utill.ConvertImageToBase64(requestDto.Image);
                    imgBase.FileName = requestDto.Image.FileName;
                    imgBase.Name = requestDto.Image.Name;
                    imgBase.ContentType = requestDto.Image.ContentType;
                    imgBase.ContentDisposition = requestDto.Image.ContentDisposition;
                    imgBase.Length = requestDto.Image.Length;

                    _unitOfWork.ImgBaseRepository.Add(imgBase);
                    data.ImgBaseId = imgBase.Id;
                }

                _unitOfWork.NotifyRepository.Update(data);

                if (await _unitOfWork.SaveChangesAsync() > 0)
                {
                    return new ResponseData<NotifyDetailResponseDto>
                    {
                        Success = true,
                        Message = "Successfully update notify.",
                        Data = null,
                        Code = (int)ErrorCodeAPI.OK
                    };
                };
            }
            catch (Exception ex)
            {
                return new ResponseData<NotifyDetailResponseDto>
                {
                    Success = false,
                    Message = ex.Message,
                    Code = (int)ErrorCodeAPI.SystemIsError
                };
            }

            return new ResponseData<NotifyDetailResponseDto>
            {
                Success = false,
                Message = "Liên hệ admin check lại code #_#",
                Code = (int)ErrorCodeAPI.SystemIsError
            };
        }


        // Helper method to get the status of the notify
        private string GetNotifyStatus(Notify notify)
        {
            // Logic to determine the status of the notify
            return notify.Status switch
            {
                //NotifyStatus.Draft => "Bản nháp",
                //NotifyStatus.Pending => "Chờ phê duyệt",
                //NotifyStatus.Approved => "Đã duyệt",
                //NotifyStatus.Rejected => "Từ chối",
                _ => "Khác"
            };
        }

        // Helper method to get the username by Id
        private async Task<string> GetUserNameById(Guid userId)
        {
            var user = await _unitOfWork.StaffRepository.GetQuery(x => x.Id == userId && x.IsDeleted == false).FirstOrDefaultAsync();
            return user != null ? user.FullName : "";
        }
    }
}
