using System;
using System.Threading.Tasks;
using BuildingModels;
using Core.Enums;
using Core.Model;
using Microsoft.EntityFrameworkCore;
using TFU_Building_API.Core.Infrastructure;
using TFU_Building_API.Dto;

namespace TFU_Building_API.Service.impl
{
    public class ResidentService : IResidentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ResidentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseData<ResidentResponseDto>> AddResident(ResidentRequestDto request)
        {
            try
            {
                // Kiểm tra xem OwnershipId có tồn tại không
                var ownership = await _unitOfWork.OwnerShipRepository.GetQuery(x => x.Id == request.OwnerShipId && x.IsDeleted == false).FirstOrDefaultAsync();

                if (ownership == null)
                {
                    // Nếu không tìm thấy Ownership, trả về lỗi
                    return new ResponseData<ResidentResponseDto>
                    {
                        Success = false,
                        Message = "Ownership not found.",
                        Code = (int)ErrorCodeAPI.NotFound
                    };
                }

                // Tạo mới Resident và thiết lập các giá trị từ request
                var newResident = new Resident
                {
                    Id = Guid.NewGuid(),
                    Name = request.Name,
                    Email = request.Email,
                    Phone = request.Phone,
                    OwnerShipId = request.OwnerShipId,
                    RegistratorDate = DateTime.Now,  // Ngày đăng ký hiện tại
                    IsDeleted = false,
                    IsActive = true,
                    InsertedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                };

                // Thêm resident mới vào cơ sở dữ liệu
                _unitOfWork.ResidentRepository.Add(newResident);
                await _unitOfWork.SaveChangesAsync();

                // Trả về kết quả thành công với thông tin ResidentResponseDto
                var response = new ResidentResponseDto
                {
                    Id = newResident.Id
                };

                return new ResponseData<ResidentResponseDto>
                {
                    Success = true,
                    Message = "Resident added successfully.",
                    Data = response,
                    Code = (int)ErrorCodeAPI.OK
                };
            }
            catch (Exception ex)
            {
                // Xử lý lỗi và trả về thông báo lỗi chi tiết
                return new ResponseData<ResidentResponseDto>
                {
                    Success = false,
                    Message = ex.Message,
                    Code = (int)ErrorCodeAPI.SystemIsError
                };
            }
        }

        public async Task<ResponseData<ResidentUpdateRequestDto>> UpdateResident(ResidentUpdateRequestDto request)
        {
            try
            {
                // Kiểm tra xem Resident có tồn tại không
                var existingResident = await _unitOfWork.ResidentRepository.GetQuery(x => x.Id == request.Id && x.IsDeleted == false).FirstOrDefaultAsync();

                if (existingResident == null)
                {
                    // Nếu không tìm thấy Resident, trả về lỗi
                    return new ResponseData<ResidentUpdateRequestDto>
                    {
                        Success = false,
                        Message = "Resident not found.",
                        Code = (int)ErrorCodeAPI.NotFound
                    };
                }

                // Cập nhật thông tin Resident
                existingResident.Name = request.Name;
                existingResident.Email = request.Email;
                existingResident.Phone = request.Phone;
                existingResident.UpdatedAt = DateTime.Now;

                // Lưu thay đổi
                _unitOfWork.ResidentRepository.Update(existingResident);
                await _unitOfWork.SaveChangesAsync();

                // Trả về kết quả thành công
                var response = new ResidentUpdateRequestDto
                {
                    Id = existingResident.Id,
                    Name = existingResident.Name,
                    Email = existingResident.Email,
                    Phone = existingResident.Phone
                };

                return new ResponseData<ResidentUpdateRequestDto>
                {
                    Success = true,
                    Message = "Resident updated successfully.",
                    Data = response,
                    Code = (int)ErrorCodeAPI.OK
                };
            }
            catch (Exception ex)
            {
                // Xử lý lỗi và trả về thông báo lỗi chi tiết
                return new ResponseData<ResidentUpdateRequestDto>
                {
                    Success = false,
                    Message = ex.Message,
                    Code = (int)ErrorCodeAPI.SystemIsError
                };
            }
        }

        public async Task<ResponseData<PagedResidentListResponseDto>> GetResidentsByOwnershipId(ResidentSearchRequestDto request)
        {
            try
            {
                // Lấy danh sách Resident dựa trên OwnershipId và tìm kiếm theo tên nếu có
                var residentQuery = _unitOfWork.ResidentRepository.GetQuery(r => r.OwnerShipId == request.OwnershipId && r.IsDeleted == false);

                // Nếu có tìm kiếm theo tên
                if (!string.IsNullOrEmpty(request.Name))
                {
                    residentQuery = residentQuery.Where(r => r.Name.Contains(request.Name));
                }

                // Tính tổng số bản ghi trước khi phân trang
                var totalRecords = await residentQuery.CountAsync();

                // Áp dụng phân trang
                var residents = await residentQuery
                    .OrderBy(r => r.Name)
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .Select(r => new ResidentListResponseDto
                    {
                        Id = r.Id,
                        Name = r.Name,
                        Email = r.Email,
                        Phone = r.Phone,
                        RegistratorDate = r.RegistratorDate
                    }).ToListAsync();

                // Trả về kết quả
                return new ResponseData<PagedResidentListResponseDto>
                {
                    Success = true,
                    Message = "Residents retrieved successfully.",
                    Data = new PagedResidentListResponseDto
                    {
                        Residents = residents,
                        TotalRecords = totalRecords
                    },
                    Code = (int)ErrorCodeAPI.OK
                };
            }
            catch (Exception ex)
            {
                // Xử lý lỗi và trả về thông báo lỗi chi tiết
                return new ResponseData<PagedResidentListResponseDto>
                {
                    Success = false,
                    Message = ex.Message,  // Hoặc log lỗi chi tiết và trả về thông báo chung
                    Code = (int)ErrorCodeAPI.SystemIsError
                };
            }
        }

        public async Task<ResponseData<ResidentResponseDto>> DeleteResident(ResidentDeleteRequestDto request)
        {
            try
            {
                // Tìm kiếm Resident dựa trên ResidentId
                var resident = await _unitOfWork.ResidentRepository.GetQuery(r => r.Id == request.ResidentId && r.IsDeleted == false).FirstOrDefaultAsync();

                if (resident == null)
                {
                    // Nếu không tìm thấy Resident, trả về thông báo lỗi
                    return new ResponseData<ResidentResponseDto>
                    {
                        Success = false,
                        Message = "Resident not found.",
                        Code = (int)ErrorCodeAPI.NotFound
                    };
                }

                // Cập nhật cờ IsDeleted thành true
                resident.IsDeleted = true;
                resident.UpdatedAt = DateTime.Now;

                _unitOfWork.ResidentRepository.Update(resident);
                await _unitOfWork.SaveChangesAsync();

                // Trả về kết quả thành công
                return new ResponseData<ResidentResponseDto>
                {
                    Success = true,
                    Message = "Resident deleted successfully.",
                    Code = (int)ErrorCodeAPI.OK
                };
            }
            catch (Exception ex)
            {
                // Xử lý lỗi và trả về thông báo lỗi chi tiết
                return new ResponseData<ResidentResponseDto>
                {
                    Success = false,
                    Message = ex.Message,
                    Code = (int)ErrorCodeAPI.SystemIsError
                };
            }
        }
    }
}
