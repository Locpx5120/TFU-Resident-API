using System;
using System.Threading.Tasks;
using Core.Enums;
using Core.Model;
using Microsoft.EntityFrameworkCore;
using TFU_Building_API.Dto;
using TFU_Building_API.Core.Infrastructure;
using BuildingModels;

namespace TFU_Building_API.Service.impl
{
    public class OwnerShipService : IOwnerShipService
    {
        private readonly IUnitOfWork _unitOfWork;

        public OwnerShipService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseData<OwnerShipResponseDto>> AddOwnerShip(OwnerShipRequestDto request)
        {
            try
            {
                // Tìm kiếm Apartment theo roomnumber và floor
                var apartment = await _unitOfWork.ApartmentRepository.GetQuery(a => a.RoomNumber == request.RoomNumber && a.Floor == request.Floor && a.IsDeleted == false).FirstOrDefaultAsync();
                if (apartment == null)
                {
                    return new ResponseData<OwnerShipResponseDto>
                    {
                        Success = false,
                        Message = "Apartment not found.",
                        Code = (int)ErrorCodeAPI.NotFound
                    };
                }

                // Kiểm tra xem ApartmentId đã tồn tại trong OwnerShip chưa
                var existingOwnerShip = await _unitOfWork.OwnerShipRepository.GetQuery(x => x.ApartmentId == apartment.Id && x.IsDeleted == false).FirstOrDefaultAsync();
                if (existingOwnerShip != null)
                {
                    return new ResponseData<OwnerShipResponseDto>
                    {
                        Success = false,
                        Message = "This apartment already has an ownership.",
                        Code = (int)ErrorCodeAPI.DuplicateEntry
                    };
                }

                // Tìm kiếm User theo email
                var user = await _unitOfWork.UserRepository.GetQuery(u => u.Email == request.Email && u.IsDeleted == false).FirstOrDefaultAsync();
                if (user == null)
                {
                    return new ResponseData<OwnerShipResponseDto>
                    {
                        Success = false,
                        Message = "User not found.",
                        Code = (int)ErrorCodeAPI.UserNotFound
                    };
                }

                // Tạo mới quyền sở hữu
                var newOwnerShip = new OwnerShip
                {
                    ApartmentId = apartment.Id,
                    UserId = user.Id,
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddYears(50),
                    InsertedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    IsDeleted = false,
                    IsActive = true
                };

                _unitOfWork.OwnerShipRepository.Add(newOwnerShip);
                await _unitOfWork.SaveChangesAsync();

                return new ResponseData<OwnerShipResponseDto>
                {
                    Success = true,
                    Message = "Ownership created successfully.",
                    Data = new OwnerShipResponseDto { Id = newOwnerShip.Id },
                    Code = (int)ErrorCodeAPI.OK
                };
            }
            catch (Exception ex)
            {
                return new ResponseData<OwnerShipResponseDto>
                {
                    Success = false,
                    Message = ex.Message,
                    Code = (int)ErrorCodeAPI.SystemIsError
                };
            }
        }


        public async Task<ResponseData<OwnerShipResponseDto>> UpdateOwnerShip(OwnerShipUpdateRequestDto request)
        {
            try
            {
                // Tìm quyền sở hữu theo ID
                var ownerShip = await _unitOfWork.OwnerShipRepository.GetQuery(x => x.Id == request.Id && x.IsDeleted == false).FirstOrDefaultAsync();
                if (ownerShip == null)
                {
                    return new ResponseData<OwnerShipResponseDto>
                    {
                        Success = false,
                        Message = "Ownership not found.",
                        Code = (int)ErrorCodeAPI.NotFound
                    };
                }

                // Tìm kiếm Apartment theo roomnumber và floor
                var apartment = await _unitOfWork.ApartmentRepository.GetQuery(a => a.RoomNumber == request.RoomNumber && a.Floor == request.Floor && a.IsDeleted == false).FirstOrDefaultAsync();
                if (apartment == null)
                {
                    return new ResponseData<OwnerShipResponseDto>
                    {
                        Success = false,
                        Message = "Apartment not found.",
                        Code = (int)ErrorCodeAPI.NotFound
                    };
                }

                // Kiểm tra xem ApartmentId đã tồn tại trong OwnerShip chưa (ngoại trừ OwnerShip hiện tại)
                var existingOwnerShip = await _unitOfWork.OwnerShipRepository.GetQuery(x => x.ApartmentId == apartment.Id && x.Id != ownerShip.Id && x.IsDeleted == false).FirstOrDefaultAsync();
                if (existingOwnerShip != null)
                {
                    return new ResponseData<OwnerShipResponseDto>
                    {
                        Success = false,
                        Message = "This apartment already has an ownership.",
                        Code = (int)ErrorCodeAPI.DuplicateEntry
                    };
                }

                // Tìm kiếm User theo email
                var user = await _unitOfWork.UserRepository.GetQuery(u => u.Email == request.Email && u.IsDeleted == false).FirstOrDefaultAsync();
                if (user == null)
                {
                    return new ResponseData<OwnerShipResponseDto>
                    {
                        Success = false,
                        Message = "User not found.",
                        Code = (int)ErrorCodeAPI.UserNotFound
                    };
                }

                // Cập nhật các thông tin của OwnerShip
                ownerShip.ApartmentId = apartment.Id;
                ownerShip.UserId = user.Id;
                ownerShip.UpdatedAt = DateTime.Now; // Cập nhật thời gian update
                ownerShip.EndDate = DateTime.Now.AddYears(50); // Update EndDate thêm 50 năm

                // Lưu thay đổi
                await _unitOfWork.SaveChangesAsync();

                return new ResponseData<OwnerShipResponseDto>
                {
                    Success = true,
                    Message = "Ownership updated successfully.",
                    Data = new OwnerShipResponseDto { Id = ownerShip.Id },
                    Code = (int)ErrorCodeAPI.OK
                };
            }
            catch (Exception ex)
            {
                return new ResponseData<OwnerShipResponseDto>
                {
                    Success = false,
                    Message = ex.Message,
                    Code = (int)ErrorCodeAPI.SystemIsError
                };
            }
        }

        public async Task<ResponseData<OwnerShipResponseDto>> DeleteOwnerShip(Guid ownerShipId)
        {
            try
            {
                // Tìm quyền sở hữu theo ID
                var ownerShip = await _unitOfWork.OwnerShipRepository.GetQuery(x => x.Id == ownerShipId && x.IsDeleted == false).FirstOrDefaultAsync();
                if (ownerShip == null)
                {
                    return new ResponseData<OwnerShipResponseDto>
                    {
                        Success = false,
                        Message = "Ownership not found.",
                        Code = (int)ErrorCodeAPI.NotFound
                    };
                }

                // Đánh dấu là đã xóa
                ownerShip.IsDeleted = true;
                ownerShip.UpdatedAt = DateTime.Now;

                // Lưu thay đổi vào database
                await _unitOfWork.SaveChangesAsync();

                return new ResponseData<OwnerShipResponseDto>
                {
                    Success = true,
                    Message = "Ownership deleted successfully.",
                    Data = new OwnerShipResponseDto { Id = ownerShip.Id },
                    Code = (int)ErrorCodeAPI.OK
                };
            }
            catch (Exception ex)
            {
                return new ResponseData<OwnerShipResponseDto>
                {
                    Success = false,
                    Message = ex.Message,
                    Code = (int)ErrorCodeAPI.SystemIsError
                };
            }
        }

        public async Task<ResponseData<PaginatedResponseDto<OwnerShipListResponseDto>>> GetOwnerShips(OwnerShipSearchRequestDto request)
        {
            try
            {
                // Lấy danh sách ownership kết hợp với bảng User và Apartment
                var ownerShipQuery = from ownership in _unitOfWork.OwnerShipRepository.GetQuery(x => x.IsDeleted == false)
                                     join user in _unitOfWork.UserRepository.GetQuery(u => u.IsDeleted == false)
                                        on ownership.UserId equals user.Id
                                     join apartment in _unitOfWork.ApartmentRepository.GetQuery(a => a.IsDeleted == false)
                                        on ownership.ApartmentId equals apartment.Id
                                     select new OwnerShipListResponseDto
                                     {
                                         FullName = user.FullName,
                                         Floor = apartment.Floor.GetValueOrDefault(),
                                         RoomNumber = apartment.RoomNumber.GetValueOrDefault(),
                                         PhoneNumber = user.PhoneNumber,
                                         Email = user.Email,
                                         Id = ownership.Id
                                     };

                // Nếu có tìm kiếm theo tên
                if (!string.IsNullOrEmpty(request.Name))
                {
                    ownerShipQuery = ownerShipQuery.Where(x => x.FullName.Contains(request.Name));
                }

                // Tính tổng số bản ghi trước khi phân trang
                var totalRecords = await ownerShipQuery.CountAsync();

                // Áp dụng phân trang
                var ownerShipList = await ownerShipQuery
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToListAsync();

                var response = new PaginatedResponseDto<OwnerShipListResponseDto>
                {
                    TotalRecords = totalRecords,
                    Data = ownerShipList
                };

                return new ResponseData<PaginatedResponseDto<OwnerShipListResponseDto>>
                {
                    Success = true,
                    Message = "Successfully retrieved owner ships.",
                    Data = response,
                    Code = (int)ErrorCodeAPI.OK
                };
            }
            catch (Exception ex)
            {
                return new ResponseData<PaginatedResponseDto<OwnerShipListResponseDto>>
                {
                    Success = false,
                    Message = ex.Message,
                    Code = (int)ErrorCodeAPI.SystemIsError
                };
            }
        }
    }
}
