﻿using BuildingModels;
using Core.Enums;
using Core.Model;
using Microsoft.EntityFrameworkCore;
using TFU_Building_API.Core.Helper;
using TFU_Building_API.Core.Infrastructure;
using TFU_Building_API.Dto;

namespace TFU_Building_API.Service.impl
{
    public class NotifyService : INotifyService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _env;

        public NotifyService(IUnitOfWork unitOfWork, IWebHostEnvironment env)
        {
            _unitOfWork = unitOfWork;
            _env = env;
        }

        public async Task<ResponseData<CreateNotifyResponseDto>> CreateNotifyAsync(CreateNotifyRequestDto request)
        {
            try
            {
                string imageUrl = null;

                // Lưu ảnh vào thư mục nếu người dùng tải lên
                if (request.Image != null)
                {
                    // Định nghĩa đường dẫn thư mục lưu trữ ảnh
                    var uploadPath = Path.Combine(_env.ContentRootPath, "uploads", "images");
                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }

                    // Tạo tên file duy nhất để tránh trùng lặp
                    var fileName = $"{Guid.NewGuid()}{Path.GetExtension(request.Image.FileName)}";
                    var filePath = Path.Combine(uploadPath, fileName);

                    // Lưu file lên server
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await request.Image.CopyToAsync(stream);
                    }

                    // Tạo URL để truy cập ảnh
                    imageUrl = $"/uploads/images/{fileName}";
                }

                // Khởi tạo bản tin mới
                var newNotify = new Notify
                {
                    Id = Guid.NewGuid(),
                    Date = request.ApplyDate,
                    Time = request.ApplyTime,
                    NotifyCategoryId = request.NotifyCategoryId,
                    BuildingId = request.BuildingId,
                    RoleId = request.RoleId,
                    Title = request.Title,
                    ShortContent = request.Content,
                    LongContent = request.DetailContent,
                    UrlImg = imageUrl,
                    IsDeleted = false,
                    IsActive = true,
                    Status = request.Status

                };

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
                var query = from n in _unitOfWork.NotifyRepository.GetQuery(x => x.IsDeleted == false)
                            join nc in _unitOfWork.NotifyCategoryRepository.GetQuery(x => x.IsDeleted == false)
                                on n.NotifyCategoryId equals nc.Id
                            join b in _unitOfWork.BuildingRepository.GetQuery(x => x.IsDeleted == false)
                                on n.BuildingId equals b.Id into buildingJoin
                            from bj in buildingJoin.DefaultIfEmpty()
                            join r in _unitOfWork.RoleRepository.GetQuery(x => x.IsDeleted == false)
                                on n.RoleId equals r.Id into roleJoin
                            from rj in roleJoin.DefaultIfEmpty()
                            select new
                            {
                                n.Id,
                                n.Title,
                                n.ShortContent,
                                n.Date,
                                n.Time,
                                n.NotifyCategoryId,  // Include NotifyCategoryId here
                                n.BuildingId,
                                NotifyCategoryName = nc.Name,
                                BuildingName = bj != null ? bj.Name : "Tất cả",
                                RoleName = rj != null ? rj.Name : "Tất cả",
                                n.Status,
                                n.InsertedById,
                                n.UpdatedById
                            };

                // Apply filters
                if (!string.IsNullOrEmpty(request.Title))
                {
                    query = query.Where(x => x.Title.Contains(request.Title));
                }

                if (request.NotifyCategoryId.HasValue)
                {
                    query = query.Where(x => x.NotifyCategoryId == request.NotifyCategoryId.Value);
                }

                if (request.BuildingId.HasValue)
                {
                    query = query.Where(x => x.BuildingId == request.BuildingId.Value);
                }

                if (!string.IsNullOrEmpty(request.StatusText))
                {
                    var statusValue = request.StatusText switch
                    {
                        "Bản nháp" => NotifyStatus.Draft,
                        "Chờ phê duyệt" => NotifyStatus.Pending,
                        "Đã duyệt" => NotifyStatus.Approved,
                        "Từ chối" => NotifyStatus.Rejected,
                        _ => (int?)null
                    };
                    if (statusValue.HasValue)
                    {
                        query = query.Where(x => x.Status == statusValue.Value);
                    }
                }

                if (request.ApplyDate.HasValue)
                {
                    query = query.Where(x => x.Date == request.ApplyDate.Value);
                }

                // Sort by date descending
                query = query.OrderByDescending(x => x.Date);

                // Calculate total records before pagination
                var totalRecords = await query.CountAsync();

                // Apply pagination
                var paginatedQuery = query
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize);

                // Stage 2: Fetch the data into memory
                var dataList = await paginatedQuery.ToListAsync();

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
                        Time = item.Time,
                        NotifyCategory = item.NotifyCategoryName,
                        BuildingName = item.BuildingName,
                        RoleName = item.RoleName,
                        Status = GetNotifyStatus(new Notify { Status = item.Status }),
                        CreatedBy = item.InsertedById != null ? await GetUserNameById(item.InsertedById.Value) : "",
                        ApprovedBy = item.UpdatedById != null ? await GetUserNameById(item.UpdatedById.Value) : "",
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


        // Helper method to get the status of the notify
        private string GetNotifyStatus(Notify notify)
        {
            // Logic to determine the status of the notify
            return notify.Status switch
            {
                NotifyStatus.Draft => "Bản nháp",
                NotifyStatus.Pending => "Chờ phê duyệt",
                NotifyStatus.Approved => "Đã duyệt",
                NotifyStatus.Rejected => "Từ chối",
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