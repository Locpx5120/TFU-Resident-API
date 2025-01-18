using BuildingModels;
using Constant;
using Core.Enums;
using Core.Model;
using Microsoft.EntityFrameworkCore;
using TFU_Building_API.Core.Infrastructure;
using TFU_Building_API.Dto;

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
                var apartment = await _unitOfWork.ApartmentRepository.GetQuery(a => a.RoomNumber == request.RoomNumber
                && a.FloorNumber == request.FloorNumber
                && a.BuildingId == request.BuildingId && a.IsDeleted == false).FirstOrDefaultAsync();
                if (apartment == null)
                {
                    return new ResponseData<OwnerShipResponseDto>
                    {
                        Success = false,
                        Message = MessConstant.ApartmentFoundZero,
                        Code = (int)ErrorCodeAPI.NotFound
                    };
                }

                var user = await _unitOfWork.ResidentRepository.GetQuery(u => u.Id == request.Id && u.IsDeleted == false).FirstOrDefaultAsync();
                if (user == null)
                {
                    return new ResponseData<OwnerShipResponseDto>
                    {
                        Success = false,
                        Message = MessConstant.ResidentFoundZero,
                        Code = (int)ErrorCodeAPI.UserNotFound
                    };
                }
                user.IsOwner = true;
                _unitOfWork.ResidentRepository.Update(user);
                // Kiểm tra xem ApartmentId đã tồn tại trong OwnerShip chưa
                var existingOwnerShip = await _unitOfWork.OwnerShipRepository.GetQuery(x => x.ApartmentId == apartment.Id && x.IsDeleted == false).FirstOrDefaultAsync();
                if (existingOwnerShip != null)
                {
                    //return new ResponseData<OwnerShipResponseDto>
                    //{
                    //    Success = false,
                    //    Message = "This apartment already has an ownership.",
                    //    Code = (int)ErrorCodeAPI.DuplicateEntry
                    //};

                    if (existingOwnerShip.ResidentId == request.Id)
                    {
                        return new ResponseData<OwnerShipResponseDto>
                        {
                            Success = false,
                            Message = MessConstant.OwnerShipHaveExist,
                            Code = (int)ErrorCodeAPI.UserNotFound
                        };
                    }

                    existingOwnerShip.IsDeleted = true;
                    existingOwnerShip.IsActive = false;
                    _unitOfWork.OwnerShipRepository.Update(existingOwnerShip);
                }

                var newOwnerShip = new OwnerShip
                {
                    ApartmentId = apartment.Id,
                    ResidentId = request.Id,
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddYears(50),
                };
                _unitOfWork.OwnerShipRepository.Add(newOwnerShip);

                await _unitOfWork.SaveChangesAsync();

                return new ResponseData<OwnerShipResponseDto>
                {
                    Success = true,
                    Message = MessConstant.OwnerShipUpdateSuccessfully,
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
                Building building = await _unitOfWork.BuildingRepository.GetByIdAsync(request.BuildingId);
                if (building == null)
                {
                    return new ResponseData<OwnerShipResponseDto>
                    {
                        Success = false,
                        Message = MessConstant.BuildingFoundZero,
                        Data = null,
                        Code = (int)ErrorCodeAPI.InternalError
                    };
                }

                if (building.NumberFloor < request.FloorNumber)
                {
                    return new ResponseData<OwnerShipResponseDto>
                    {
                        Success = false,
                        Message = MessConstant.BuildingOverFloor + $" {request.FloorNumber}/{building.NumberFloor}",
                        Data = null,
                        Code = (int)ErrorCodeAPI.InternalError
                    };
                }



                // Tìm kiếm Apartment theo roomnumber và floor
                var apartment = await _unitOfWork.ApartmentRepository.GetQuery(a =>
                a.RoomNumber == request.RoomNumber
                && a.FloorNumber == request.FloorNumber
                && a.BuildingId == request.BuildingId
                && a.IsDeleted == false)
                    .FirstOrDefaultAsync();
                if (apartment == null)
                {
                    return new ResponseData<OwnerShipResponseDto>
                    {
                        Success = false,
                        Message = MessConstant.ApartmentFoundZero,
                        Code = (int)ErrorCodeAPI.NotFound
                    };
                }

                //// Tìm quyền sở hữu theo ID
                var ownerShip = await _unitOfWork.OwnerShipRepository
                    .GetQuery(x => x.ResidentId == request.Id && x.ApartmentId == apartment.Id
                && x.IsDeleted == false).FirstOrDefaultAsync();
                if (ownerShip == null)
                {
                    OwnerShip ownerShipOld = _unitOfWork.OwnerShipRepository.GetQuery(x => x.ApartmentId == apartment.Id).FirstOrDefault();
                    if (ownerShipOld != null)
                    {
                        ownerShipOld.IsDeleted = true;
                        ownerShipOld.IsActive = false;
                        _unitOfWork.OwnerShipRepository.Update(ownerShipOld);
                    }

                    var newOwnerShip = new OwnerShip
                    {
                        ApartmentId = apartment.Id,
                        ResidentId = request.Id,
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now.AddYears(50),
                    };
                    _unitOfWork.OwnerShipRepository.Add(newOwnerShip);
                    ownerShip = newOwnerShip;
                }

                apartment.ApartmentTypeId = request.ApartmentTypeId;

                _unitOfWork.ApartmentRepository.Update(apartment);

                // Lưu thay đổi
                await _unitOfWork.SaveChangesAsync();

                return new ResponseData<OwnerShipResponseDto>
                {
                    Success = true,
                    Message = MessConstant.UpdateSuccessfully,
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
                        Message = MessConstant.OwnerShipFoundZero,
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
                    Message = MessConstant.OwnerShipUpdateSuccessfully,
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
                                     join user in _unitOfWork.ResidentRepository.GetQuery(u => u.IsDeleted == false)
                                        on ownership.ResidentId equals user.Id
                                     join apartment in _unitOfWork.ApartmentRepository.GetQuery(a => a.IsDeleted == false)
                                       on ownership.ApartmentId equals apartment.Id
                                     select new OwnerShipListResponseDto
                                     {
                                         FullName = user.Name,
                                         FloorNumber = apartment.FloorNumber,
                                         RoomNumber = apartment.RoomNumber,
                                         PhoneNumber = user.Phone,
                                         Email = user.Email,
                                         Id = ownership.ResidentId ?? Guid.Empty,
                                         ApartmentId = apartment.Id,
                                         BuildingId = apartment.BuildingId,
                                         ApartmentTypeId = apartment.ApartmentTypeId,
                                     };

                var apartmentIds = ownerShipQuery.Select(x => x.ApartmentId).ToList();

                var apartmentNotOwnerShip = _unitOfWork.ApartmentRepository.GetQuery(a => a.IsDeleted == false && !apartmentIds.Contains(a.Id));
                List<OwnerShipListResponseDto> ownerShipListResponseDtos = new List<OwnerShipListResponseDto>();
                ownerShipListResponseDtos.AddRange(ownerShipQuery);

                foreach (var apartment in apartmentNotOwnerShip)
                {
                    ownerShipListResponseDtos.Add(
                    new OwnerShipListResponseDto
                    {
                        Id = Guid.Empty,
                        FloorNumber = apartment.FloorNumber,
                        RoomNumber = apartment.RoomNumber,
                        ApartmentId = (Guid)apartment.Id,
                        BuildingId = (Guid)apartment.BuildingId,
                        ApartmentTypeId = apartment.ApartmentTypeId,
                    });
                }

                // Nếu có tìm kiếm theo tên
                if (!string.IsNullOrEmpty(request.Name))
                {
                    ownerShipListResponseDtos = ownerShipListResponseDtos.Where(x => x.FullName != null && x.FullName.Contains(request.Name)).ToList();
                }

                if (request.BuildingId != null && request.BuildingId != Guid.Empty)
                {
                    ownerShipListResponseDtos = ownerShipListResponseDtos.Where(x => x.BuildingId == request.BuildingId).ToList();
                }

                // Tính tổng số bản ghi trước khi phân trang
                var totalRecords = ownerShipListResponseDtos.ToList().Count();

                // Áp dụng phân trang
                var ownerShipList = ownerShipListResponseDtos.ToList()
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToList();

                var response = new PaginatedResponseDto<OwnerShipListResponseDto>
                {
                    TotalRecords = totalRecords,
                    Data = ownerShipList
                };

                return new ResponseData<PaginatedResponseDto<OwnerShipListResponseDto>>
                {
                    Success = true,
                    Message = MessConstant.Successfully,
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

        public async Task<ResponseData<OwnershipInfoResponseDto>> GetOwnershipById(Guid ownershipId)
        {
            try
            {
                // Tìm Ownership dựa trên Id
                var ownership = await _unitOfWork.OwnerShipRepository.GetQuery(x => x.Id == ownershipId && x.IsDeleted == false)
                    .FirstOrDefaultAsync();

                if (ownership == null)
                {
                    // Nếu không tìm thấy Ownership, trả về thông báo lỗi
                    return new ResponseData<OwnershipInfoResponseDto>
                    {
                        Success = false,
                        Message = MessConstant.OwnerShipFoundZero,
                        Code = (int)ErrorCodeAPI.NotFound
                    };
                }

                // Lấy thông tin từ bảng User dựa trên UserId
                var user = await _unitOfWork.ResidentRepository.GetQuery(u => u.Id == ownership.ResidentId && u.IsDeleted == false)
                    .FirstOrDefaultAsync();

                if (user == null)
                {
                    return new ResponseData<OwnershipInfoResponseDto>
                    {
                        Success = false,
                        Message = MessConstant.ResidentFoundZero,
                        Code = (int)ErrorCodeAPI.UserNotFound
                    };
                }

                // Lấy thông tin từ bảng Apartment dựa trên ApartmentId
                var apartment = await _unitOfWork.ApartmentRepository.GetQuery(a => a.Id == ownership.ApartmentId && a.IsDeleted == false)
                    .FirstOrDefaultAsync();

                if (apartment == null)
                {
                    return new ResponseData<OwnershipInfoResponseDto>
                    {
                        Success = false,
                        Message = MessConstant.ApartmentFoundZero,
                        Code = (int)ErrorCodeAPI.NotFound
                    };
                }

                // Tạo đối tượng response với các trường cần thiết
                var response = new OwnershipInfoResponseDto
                {
                    Id = ownership.Id,
                    Email = user.Email,
                    FloorNumber = apartment.FloorNumber,
                    RoomNumber = apartment.RoomNumber
                };

                // Trả về kết quả thành công
                return new ResponseData<OwnershipInfoResponseDto>
                {
                    Success = true,
                    Message = MessConstant.FindSuccessfully,
                    Data = response,
                    Code = (int)ErrorCodeAPI.OK
                };
            }
            catch (Exception ex)
            {
                // Xử lý lỗi và trả về thông báo lỗi chi tiết
                return new ResponseData<OwnershipInfoResponseDto>
                {
                    Success = false,
                    Message = ex.Message,
                    Code = (int)ErrorCodeAPI.SystemIsError
                };
            }
        }

        public async Task<ResponseData<List<ApartmentMemberDetailDto>>> GetUserByOwnerShipId(Guid ownershipId)
        {
            try
            {
                // Tìm Ownership dựa trên Id
                var ownership = await _unitOfWork.OwnerShipRepository.GetQuery(x => x.Id == ownershipId && x.IsDeleted == false)
                .FirstOrDefaultAsync();

                if (ownership == null)
                {
                    // Nếu không tìm thấy Ownership, trả về thông báo lỗi
                    return new ResponseData<List<ApartmentMemberDetailDto>>
                    {
                        Success = false,
                        Message = MessConstant.OwnerShipFoundZero,
                        Code = (int)ErrorCodeAPI.NotFound
                    };
                }

                // Lấy thông tin từ bảng User dựa trên UserId
                var user = await _unitOfWork.ResidentRepository.GetQuery(u => u.Id == ownership.ResidentId && u.IsDeleted == false)
                    .FirstOrDefaultAsync();

                if (user == null)
                {
                    return new ResponseData<List<ApartmentMemberDetailDto>>
                    {
                        Success = false,
                        Message = MessConstant.ResidentFoundZero,
                        Code = (int)ErrorCodeAPI.UserNotFound
                    };
                }

                // Lấy thông tin từ bảng Apartment dựa trên ApartmentId
                var apartment = await _unitOfWork.ApartmentRepository.GetQuery(a => a.Id == ownership.ApartmentId && a.IsDeleted == false)
                    .FirstOrDefaultAsync();

                if (apartment == null)
                {
                    return new ResponseData<List<ApartmentMemberDetailDto>>
                    {
                        Success = false,
                        Message = MessConstant.ApartmentFoundZero,
                        Code = (int)ErrorCodeAPI.NotFound
                    };
                }

                // Retrieve members from Livings table excluding the owner
                var membersQuery = _unitOfWork.LivingRepository.GetQuery(l => l.ApartmentId == apartment.Id && l.IsDeleted == false && l.ResidentId != user.Id);

                // Ensure compatibility for Include
                var membersWithIncludes = membersQuery as IQueryable<BuildingModels.Living>;

                // Add Include for Resident
                membersWithIncludes = membersWithIncludes.Include(l => l.Resident);


                var members = await membersWithIncludes.ToListAsync();

                // Prepare the response
                var response = new List<ApartmentMemberDetailDto>();


                response.AddRange(members.Select((l, index) => new ApartmentMemberDetailDto
                {
                    Id = l.ResidentId,
                    STT = index + 1,
                    MemberName = l.Resident.Name,
                    Role = "Thành viên",
                    Email = l.Resident.Email,
                    PhoneNumber = l.Resident.Phone
                }));

                return new ResponseData<List<ApartmentMemberDetailDto>>
                {
                    Success = true,
                    Message = MessConstant.FindSuccessfully,
                    Data = response,
                    Code = (int)ErrorCodeAPI.OK
                };
            }
            catch (Exception ex)
            {
                return new ResponseData<List<ApartmentMemberDetailDto>>
                {
                    Success = false,
                    Message = ex.Message,
                    Code = (int)ErrorCodeAPI.SystemIsError
                };
            }
        }
    }
}
