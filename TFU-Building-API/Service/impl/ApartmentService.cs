using BuildingModels;
using Constant;
using Core.Enums;
using Core.Model;
using Microsoft.EntityFrameworkCore;
using TFU_Building_API.Core.Handler;
using TFU_Building_API.Core.Infrastructure;
using TFU_Building_API.Dto;

namespace TFU_Building_API.Service.impl
{
    public class ApartmentService : BaseHandler, IApartmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserIdentity _userIdentity;

        public ApartmentService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor,
            IUserIdentity userIdentity)
            : base(unitOfWork, httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _userIdentity = userIdentity;
        }

        //public async Task<ResponseData<List<ApartmentResponseDto>>> GetApartmentsByResidentIdAsync(Guid residentId)
        //{
        //    try
        //    {
        //        // Lấy danh sách Ownerships liên quan tới ResidentId
        //        var ownerships = await _unitOfWork.OwnerShipRepository
        //            .GetQuery(o => o.ResidentId == residentId && o.IsDeleted == false)
        //            .Include(o => o.Apartment)  // Include Apartment để tránh phải join
        //            .Include(o => o.Resident)   // Include Resident để lấy thông tin liên quan
        //            .ToListAsync();

        //        // Lấy tất cả Living liên quan tới các ApartmentId đã lấy được
        //        var apartmentIds = ownerships.Select(o => o.ApartmentId).ToList();
        //        var livings = await _unitOfWork.LivingRepository
        //            .GetQuery(l => apartmentIds.Contains(l.ApartmentId) && l.IsDeleted == false)
        //            .ToListAsync();

        //        // Group Livings by ApartmentId để tính số thành viên
        //        var livingCounts = livings.GroupBy(l => l.ApartmentId)
        //                                  .ToDictionary(g => g.Key, g => g.Count());

        //        // Tạo danh sách ApartmentResponseDto
        //        var apartments = ownerships.Select(ownership => new ApartmentResponseDto
        //        {
        //            ApartmentId = ownership.ApartmentId ?? Guid.Empty, // Thêm trường ApartmentId
        //            OwnerName = ownership.Resident?.Name ?? string.Empty,
        //            RoomNumber = ownership.Apartment?.RoomNumber ?? 0,
        //            FloorNumber = ownership.Apartment?.FloorNumber ?? 0,
        //            NumberOfMembers = livingCounts.ContainsKey(ownership.ApartmentId) ? livingCounts[ownership.ApartmentId] : 0,
        //            Email = ownership.Resident?.Email ?? string.Empty,
        //            PhoneNumber = ownership.Resident?.Phone ?? string.Empty
        //        }).ToList();

        //        return new ResponseData<List<ApartmentResponseDto>>
        //        {
        //            Success = true,
        //            Message = "Apartment information retrieved successfully.",
        //            Data = apartments,
        //            Code = (int)ErrorCodeAPI.OK
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        return new ResponseData<List<ApartmentResponseDto>>
        //        {
        //            Success = false,
        //            Message = ex.Message,
        //            Code = (int)ErrorCodeAPI.SystemIsError
        //        };
        //    }
        //}

        public async Task<ResponseData<List<ApartmentResponseDto>>> GetApartmentsByResidentIdAsync(Guid residentId)
        {
            try
            {
                List<OwnerShip> ownerships = new List<OwnerShip>();
                // Lấy danh sách Ownerships liên quan tới ResidentId

                if (_userIdentity.RoleName.Equals(Constants.ROLE_Resident))
                {
                    residentId = (Guid)_userIdentity.UserId;
                    ownerships = await _unitOfWork.OwnerShipRepository
                    .GetQuery(o => o.ResidentId == residentId && o.IsDeleted == false)
                    .Include(o => o.Apartment)  // Include Apartment để tránh phải join
                    .ThenInclude(a => a.Building) // Include Building to fetch Building Name
                    .Include(o => o.Resident)   // Include Resident để lấy thông tin liên quan
                    .ToListAsync();
                }
                else
                {
                    ownerships = await _unitOfWork.OwnerShipRepository
                   .GetQuery(o => o.IsDeleted == false)
                   .Include(o => o.Apartment)  // Include Apartment để tránh phải join
                   .ThenInclude(a => a.Building) // Include Building to fetch Building Name
                   .Include(o => o.Resident)   // Include Resident để lấy thông tin liên quan
                   .ToListAsync();
                }

                //if (_userIdentity.RoleName.Equals(Constants.ROLE_HANH_CHINH))
                //{

                //}
                //else
                //{
                //    ownerships = await _unitOfWork.OwnerShipRepository
                //    .GetQuery(o => o.ResidentId == residentId && o.IsDeleted == false)
                //    .Include(o => o.Apartment)  // Include Apartment để tránh phải join
                //    .ThenInclude(a => a.Building) // Include Building to fetch Building Name
                //    .Include(o => o.Resident)   // Include Resident để lấy thông tin liên quan
                //    .ToListAsync();
                //}


                // Lấy tất cả Living liên quan tới các ApartmentId đã lấy được
                var apartmentIds = ownerships.Select(o => o.ApartmentId).ToList();
                var livings = await _unitOfWork.LivingRepository
                    .GetQuery(l => apartmentIds.Contains(l.ApartmentId) && l.IsDeleted == false)
                    .ToListAsync();

                // Group Livings by ApartmentId để tính số thành viên
                var livingCounts = livings.GroupBy(l => l.ApartmentId)
                                          .ToDictionary(g => g.Key, g => g.Count());

                // Tạo danh sách ApartmentResponseDto
                var apartments = ownerships.Select((ownership, index) => new ApartmentResponseDto
                {
                    STT = index + 1, // Serial Number
                    ApartmentId = ownership.ApartmentId ?? Guid.Empty, // Thêm trường ApartmentId
                    OwnerName = ownership.Resident?.Name ?? string.Empty,
                    RoomNumber = ownership.Apartment?.RoomNumber ?? 0,
                    FloorNumber = ownership.Apartment?.FloorNumber ?? 0,
                    NumberOfMembers = livingCounts.ContainsKey(ownership.ApartmentId) ? livingCounts[ownership.ApartmentId] : 0,
                    Email = ownership.Resident?.Email ?? string.Empty,
                    PhoneNumber = ownership.Resident?.Phone ?? string.Empty,
                    BuildingName = ownership.Apartment?.Building?.Name ?? string.Empty // Thêm tên tòa nhà
                }).ToList();

                return new ResponseData<List<ApartmentResponseDto>>
                {
                    Success = true,
                    Message = "Tìm kiếm thông tin căn hộ thành công.",
                    Data = apartments,
                    Code = (int)ErrorCodeAPI.OK
                };
            }
            catch (Exception ex)
            {
                return new ResponseData<List<ApartmentResponseDto>>
                {
                    Success = false,
                    Message = ex.Message,
                    Code = (int)ErrorCodeAPI.SystemIsError
                };
            }
        }


        //public async Task<ResponseData<List<ApartmentMemberDetailDto>>> GetApartmentDetailsByApartmentIdAsync(Guid apartmentId)
        //{
        //    try
        //    {
        //        // Lấy thông tin chủ căn hộ từ bảng Ownership
        //        var ownership = await _unitOfWork.OwnerShipRepository.GetQuery(o => o.ApartmentId == apartmentId && o.IsDeleted == false)
        //            .Include(o => o.Resident)
        //            .FirstOrDefaultAsync();

        //        if (ownership == null)
        //        {
        //            return new ResponseData<List<ApartmentMemberDetailDto>>
        //            {
        //                Success = false,
        //                Message = "Apartment owner not found.",
        //                Code = (int)ErrorCodeAPI.NotFound
        //            };
        //        }

        //        var owner = ownership.Resident;

        //        // Lấy danh sách các thành viên từ bảng Livings, trừ chủ căn hộ
        //        var members = await _unitOfWork.LivingRepository.GetQuery(l => l.ApartmentId == apartmentId && l.IsDeleted == false && l.ResidentId != owner.Id)
        //            .Include(l => l.Resident)
        //            .ToListAsync();

        //        // Chuyển đổi danh sách thành ApartmentMemberDetailDto và đánh số STT
        //        //        var response = new List<ApartmentMemberDetailDto>
        //        //{
        //        //    new ApartmentMemberDetailDto
        //        //    {
        //        //        STT = 1,
        //        //        MemberName = owner.Name,
        //        //        Role = "Chủ căn hộ",
        //        //        Email = owner.Email,
        //        //        PhoneNumber = owner.Phone
        //        //    }
        //        //};

        //        var response = new List<ApartmentMemberDetailDto>();
        //        response.AddRange(members.Select((l, index) => new ApartmentMemberDetailDto
        //        {
        //            STT = index + 2, // Đánh số STT bắt đầu từ 2 cho các thành viên
        //            MemberName = l.Resident.Name,
        //            Role = "Thành viên",
        //            Email = l.Resident.Email,
        //            PhoneNumber = l.Resident.Phone
        //        }));

        //        return new ResponseData<List<ApartmentMemberDetailDto>>
        //        {
        //            Success = true,
        //            Message = "Apartment member details retrieved successfully.",
        //            Data = response,
        //            Code = (int)ErrorCodeAPI.OK
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        return new ResponseData<List<ApartmentMemberDetailDto>>
        //        {
        //            Success = false,
        //            Message = ex.Message,
        //            Code = (int)ErrorCodeAPI.SystemIsError
        //        };
        //    }
        //}

        public async Task<ResponseData<List<ApartmentMemberDetailDto>>> GetApartmentDetailsByApartmentIdAsync(Guid apartmentId, string? memberName = null)
        {
            try
            {
                // Retrieve ownership including the resident (owner)
                var ownership = await _unitOfWork.OwnerShipRepository.GetQuery(o => o.ApartmentId == apartmentId && o.IsDeleted == false)
                    .Include(o => o.Resident)
                    .FirstOrDefaultAsync();

                if (ownership == null)
                {
                    return new ResponseData<List<ApartmentMemberDetailDto>>
                    {
                        Success = false,
                        Message = "Không tìm thấy chủ căn hộ",
                        Code = (int)ErrorCodeAPI.NotFound
                    };
                }

                var owner = ownership.Resident;

                // Retrieve members from Livings table excluding the owner
                var membersQuery = _unitOfWork.LivingRepository.GetQuery(l => l.ApartmentId == apartmentId && l.IsDeleted == false && l.ResidentId != owner.Id);

                // Ensure compatibility for Include
                var membersWithIncludes = membersQuery as IQueryable<BuildingModels.Living>;

                // Add Include for Resident
                membersWithIncludes = membersWithIncludes.Include(l => l.Resident);

                // Apply filter by MemberName if provided
                if (!string.IsNullOrEmpty(memberName))
                {
                    membersWithIncludes = membersWithIncludes.Where(l => l.Resident.Name.Contains(memberName));
                }

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
                    Message = "Tìm kiếm thông tin căn hộ thành công.",
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

        public async Task<ResponseData<AddApartmentMemberResponseDto>> AddApartmentMemberAsync(AddApartmentMemberDto request)
        {
            try
            {
                var existingResident = await _unitOfWork.ResidentRepository.GetQuery(r => r.Email == request.Email && r.IsDeleted == false)
                    .FirstOrDefaultAsync();

                Resident resident;
                if (existingResident == null)
                {
                    resident = new Resident
                    {
                        Id = Guid.NewGuid(),
                        Name = request.MemberName,
                        Phone = request.PhoneNumber,
                        Email = request.Email,
                        IsDeleted = false,
                        InsertedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    };

                    _unitOfWork.ResidentRepository.Add(resident);
                }
                else
                {
                    resident = existingResident;
                }

                var living = new Living
                {
                    Id = Guid.NewGuid(),
                    ApartmentId = request.ApartmentId,
                    ResidentId = resident.Id,
                    StartDate = DateTime.Now,
                    IsActive = true,
                    IsDeleted = false,
                    InsertedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                _unitOfWork.LivingRepository.Add(living);
                await _unitOfWork.SaveChangesAsync();

                return new ResponseData<AddApartmentMemberResponseDto>
                {
                    Success = true,
                    Message = "Thêm cư dân thành công",
                    Data = new AddApartmentMemberResponseDto { Success = true, Message = "Thêm cư dân thành công" },
                    Code = (int)ErrorCodeAPI.OK
                };
            }
            catch (Exception ex)
            {
                return new ResponseData<AddApartmentMemberResponseDto>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = new AddApartmentMemberResponseDto { Success = false, Message = ex.Message },
                    Code = (int)ErrorCodeAPI.SystemIsError
                };
            }
        }

        public async Task<ResponseData<List<ApartmentDto>>> GetApartmentsByBuildingIdAsync(Guid buildingId)
        {
            try
            {
                var apartments = await _unitOfWork.ApartmentRepository
                    .GetQuery(a => a.BuildingId == buildingId && a.IsDeleted == false)
                    .Select(a => new ApartmentDto
                    {
                        Id = a.Id,
                        RoomNumber = a.RoomNumber
                    })
                    .ToListAsync();

                if (_userIdentity.RoleName.Equals(Constants.ROLE_Resident))
                {
                    List<OwnerShip> ownerShips = _unitOfWork.OwnerShipRepository.GetQuery(x => x.ResidentId == _userIdentity.UserId).ToList();
                    if (ownerShips.Any())
                    {
                        apartments = apartments.Where(z => ownerShips.Select(x => x.ApartmentId).ToList().Contains(z.Id)).ToList();
                    }
                    else
                    {
                        apartments = new List<ApartmentDto>();
                    }
                }

                return new ResponseData<List<ApartmentDto>>
                {
                    Success = true,
                    Message = "Tìm kiếm thông tin thành công",
                    Data = apartments,
                    Code = (int)ErrorCodeAPI.OK
                };
            }
            catch (Exception ex)
            {
                return new ResponseData<List<ApartmentDto>>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null,
                    Code = (int)ErrorCodeAPI.SystemIsError
                };
            }
        }

        public async Task<ResponseData<AddApartmentResDto>> AddApartmentAsync(AddApartmentReqDto request)
        {
            try
            {
                Building building = await _unitOfWork.BuildingRepository.GetByIdAsync(request.BuildingId);
                if (building == null)
                {
                    return new ResponseData<AddApartmentResDto>
                    {
                        Success = false,
                        Message = "Vui lòng nhập lại vì trùng phòng\r\nKhông có toà nhà đã chọn",
                        Data = null,
                        Code = (int)ErrorCodeAPI.InternalError
                    };
                }

                List<Apartment> apartments = await _unitOfWork.ApartmentRepository.GetQuery(r =>
                   r.BuildingId == request.BuildingId
                   && r.IsActive
                   && r.IsDeleted == false)
                    .ToListAsync();

                if (apartments.Count >= building.NumberApartment)
                {
                    return new ResponseData<AddApartmentResDto>
                    {
                        Success = false,
                        Message = $"Vui lòng nhập lại vì trùng phòng\r\nToà nhà đã đạt số lượng phòng tối đa {apartments.Count}/{building.NumberApartment}",
                        Data = null,
                        Code = (int)ErrorCodeAPI.InternalError
                    };
                }


                if (building.NumberFloor < request.FloorNumber || request.FloorNumber < 0)
                {
                    return new ResponseData<AddApartmentResDto>
                    {
                        Success = false,
                        Message = $"Vui lòng nhập lại vì trùng phòng\r\nSố tầng đã chọn không hợp lệ. Toà nhà có {building.NumberFloor} tầng.",
                        Data = null,
                        Code = (int)ErrorCodeAPI.InternalError
                    };
                }

                if (request.RoomNumber < 0)
                {
                    return new ResponseData<AddApartmentResDto>
                    {
                        Success = false,
                        Message = $"Vui lòng nhập lại\r\nSố phồng đã chọn không hợp lệ. Không có phòng {request.RoomNumber}",
                        Data = null,
                        Code = (int)ErrorCodeAPI.InternalError
                    };
                }

                var existing = apartments.Where(r =>
                     r.FloorNumber == request.FloorNumber && r.RoomNumber == request.RoomNumber)
                        .ToList().FirstOrDefault();

                if (existing != null)
                {
                    return new ResponseData<AddApartmentResDto>
                    {
                        Success = false,
                        Message = "Vui lòng nhập lại vì trùng phòng\r\nTrùng căn hộ ! Vui lòng nhập lại",
                        Data = null,
                        Code = (int)ErrorCodeAPI.InternalError
                    };
                }

                var apartment = new Apartment
                {
                    Id = Guid.NewGuid(),
                    Price = request.Price,
                    FloorNumber = request.FloorNumber,
                    RoomNumber = request.RoomNumber,
                    ApartmentTypeId = request.ApartmentTypeId,
                    BuildingId = request.BuildingId,
                };

                _unitOfWork.ApartmentRepository.Add(apartment);

                if (request.Id != null || request.Id == Guid.Empty)
                {
                    var newOwnerShip = new OwnerShip
                    {
                        ApartmentId = apartment.Id,
                        ResidentId = request.Id,
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now.AddYears(50),
                    };
                    _unitOfWork.OwnerShipRepository.Add(newOwnerShip);
                }


                await _unitOfWork.SaveChangesAsync();

                return new ResponseData<AddApartmentResDto>
                {
                    Success = true,
                    Message = "Thêm phòng thành công",
                    Data = new AddApartmentResDto { ApartmentId = apartment.Id },
                    Code = (int)ErrorCodeAPI.OK
                };
            }
            catch (Exception ex)
            {
                return new ResponseData<AddApartmentResDto>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null,
                    Code = (int)ErrorCodeAPI.SystemIsError
                };
            }
        }
    }
}
