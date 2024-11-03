using BuildingModels;
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

        public ApartmentService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseData<List<ApartmentResponseDto>>> GetApartmentsByResidentIdAsync(Guid residentId)
        {
            try
            {
                // Lấy danh sách Ownerships liên quan tới ResidentId
                var ownerships = await _unitOfWork.OwnerShipRepository
                    .GetQuery(o => o.ResidentId == residentId && o.IsDeleted == false)
                    .Include(o => o.Apartment)  // Include Apartment để tránh phải join
                    .Include(o => o.Resident)   // Include Resident để lấy thông tin liên quan
                    .ToListAsync();

                // Lấy tất cả Living liên quan tới các ApartmentId đã lấy được
                var apartmentIds = ownerships.Select(o => o.ApartmentId).ToList();
                var livings = await _unitOfWork.LivingRepository
                    .GetQuery(l => apartmentIds.Contains(l.ApartmentId) && l.IsDeleted == false)
                    .ToListAsync();

                // Group Livings by ApartmentId để tính số thành viên
                var livingCounts = livings.GroupBy(l => l.ApartmentId)
                                          .ToDictionary(g => g.Key, g => g.Count());

                // Tạo danh sách ApartmentResponseDto
                var apartments = ownerships.Select(ownership => new ApartmentResponseDto
                {
                    ApartmentId = ownership.ApartmentId ?? Guid.Empty, // Thêm trường ApartmentId
                    OwnerName = ownership.Resident?.Name ?? string.Empty,
                    RoomNumber = ownership.Apartment?.RoomNumber ?? 0,
                    FloorNumber = ownership.Apartment?.FloorNumber ?? 0,
                    NumberOfMembers = livingCounts.ContainsKey(ownership.ApartmentId) ? livingCounts[ownership.ApartmentId] : 0,
                    Email = ownership.Resident?.Email ?? string.Empty,
                    PhoneNumber = ownership.Resident?.Phone ?? string.Empty
                }).ToList();

                return new ResponseData<List<ApartmentResponseDto>>
                {
                    Success = true,
                    Message = "Apartment information retrieved successfully.",
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

        public async Task<ResponseData<List<ApartmentMemberDetailDto>>> GetApartmentDetailsByApartmentIdAsync(Guid apartmentId)
        {
            try
            {
                // Lấy thông tin chủ căn hộ từ bảng Ownership
                var ownership = await _unitOfWork.OwnerShipRepository.GetQuery(o => o.ApartmentId == apartmentId && o.IsDeleted == false)
                    .Include(o => o.Resident)
                    .FirstOrDefaultAsync();

                if (ownership == null)
                {
                    return new ResponseData<List<ApartmentMemberDetailDto>>
                    {
                        Success = false,
                        Message = "Apartment owner not found.",
                        Code = (int)ErrorCodeAPI.NotFound
                    };
                }

                var owner = ownership.Resident;

                // Lấy danh sách các thành viên từ bảng Livings, trừ chủ căn hộ
                var members = await _unitOfWork.LivingRepository.GetQuery(l => l.ApartmentId == apartmentId && l.IsDeleted == false && l.ResidentId != owner.Id)
                    .Include(l => l.Resident)
                    .ToListAsync();

                // Chuyển đổi danh sách thành ApartmentMemberDetailDto và đánh số STT
                var response = new List<ApartmentMemberDetailDto>
        {
            new ApartmentMemberDetailDto
            {
                STT = 1,
                MemberName = owner.Name,
                Role = "Chủ căn hộ",
                Email = owner.Email,
                PhoneNumber = owner.Phone
            }
        };

                response.AddRange(members.Select((l, index) => new ApartmentMemberDetailDto
                {
                    STT = index + 2, // Đánh số STT bắt đầu từ 2 cho các thành viên
                    MemberName = l.Resident.Name,
                    Role = "Thành viên",
                    Email = l.Resident.Email,
                    PhoneNumber = l.Resident.Phone
                }));

                return new ResponseData<List<ApartmentMemberDetailDto>>
                {
                    Success = true,
                    Message = "Apartment member details retrieved successfully.",
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
                    Message = "Apartment member added successfully.",
                    Data = new AddApartmentMemberResponseDto { Success = true, Message = "Member added successfully" },
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




    }
}
