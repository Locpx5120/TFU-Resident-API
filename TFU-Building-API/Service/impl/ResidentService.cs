using System;
using System.Threading.Tasks;
using BuildingModels;
using Core.Enums;
using Core.Model;
using Microsoft.EntityFrameworkCore;
using TFU_Building_API.Core.Helper;
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

        //public async Task<ResponseData<PagedResidentListResponseDto>> GetResidentsByOwnershipId(ResidentSearchRequestDto request)
        //{
        //    try
        //    {
        //        // Lấy danh sách Resident dựa trên OwnershipId và tìm kiếm theo tên nếu có
        //        var residentQuery = _unitOfWork.ResidentRepository.GetQuery(r => r.OwnerShipId == request.OwnershipId && r.IsDeleted == false);

        //        // Nếu có tìm kiếm theo tên
        //        if (!string.IsNullOrEmpty(request.Name))
        //        {
        //            residentQuery = residentQuery.Where(r => r.Name.Contains(request.Name));
        //        }

        //        // Tính tổng số bản ghi trước khi phân trang
        //        var totalRecords = await residentQuery.CountAsync();

        //        // Áp dụng phân trang
        //        var residents = await residentQuery
        //            .OrderBy(r => r.Name)
        //            .Skip((request.PageNumber - 1) * request.PageSize)
        //            .Take(request.PageSize)
        //            .Select(r => new ResidentListResponseDto
        //            {
        //                Id = r.Id,
        //                Name = r.Name,
        //                Email = r.Email,
        //                Phone = r.Phone,
        //                RegistratorDate = r.RegistratorDate
        //            }).ToListAsync();

        //        // Trả về kết quả
        //        return new ResponseData<PagedResidentListResponseDto>
        //        {
        //            Success = true,
        //            Message = "Residents retrieved successfully.",
        //            Data = new PagedResidentListResponseDto
        //            {
        //                Residents = residents,
        //                TotalRecords = totalRecords
        //            },
        //            Code = (int)ErrorCodeAPI.OK
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        // Xử lý lỗi và trả về thông báo lỗi chi tiết
        //        return new ResponseData<PagedResidentListResponseDto>
        //        {
        //            Success = false,
        //            Message = ex.Message,  // Hoặc log lỗi chi tiết và trả về thông báo chung
        //            Code = (int)ErrorCodeAPI.SystemIsError
        //        };
        //    }
        //}

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

                var living = await _unitOfWork.LivingRepository.GetQuery(r => r.ResidentId == resident.Id).FirstOrDefaultAsync();

                if (living == null)
                {
                    // Nếu không tìm thấy Resident, trả về thông báo lỗi
                    return new ResponseData<ResidentResponseDto>
                    {
                        Success = false,
                        Message = "Living not found.",
                        Code = (int)ErrorCodeAPI.NotFound
                    };
                }

                // Cập nhật cờ IsDeleted thành true
                living.IsDeleted = true;
                living.UpdatedAt = DateTime.Now;

                _unitOfWork.LivingRepository.Update(living);
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

        public async Task<ResponseData<ResidentInfoResponseDto>> GetResidentById(Guid residentId)
        {
            try
            {
                // Tìm kiếm Resident dựa trên residentId
                var resident = await _unitOfWork.ResidentRepository.GetQuery(r => r.Id == residentId && r.IsDeleted == false && r.IsActive)
                    .FirstOrDefaultAsync();

                if (resident == null)
                {
                    // Nếu không tìm thấy Resident, trả về thông báo lỗi
                    return new ResponseData<ResidentInfoResponseDto>
                    {
                        Success = false,
                        Message = "Resident not found.",
                        Code = (int)ErrorCodeAPI.NotFound
                    };
                }

                // Tạo đối tượng response với các trường cần thiết
                var response = new ResidentInfoResponseDto
                {
                    Id = resident.Id,
                    Name = resident.Name,
                    Email = resident.Email,
                    Phone = resident.Phone
                };

                // Trả về kết quả thành công
                return new ResponseData<ResidentInfoResponseDto>
                {
                    Success = true,
                    Message = "Resident found successfully.",
                    Data = response,
                    Code = (int)ErrorCodeAPI.OK
                };
            }
            catch (Exception ex)
            {
                // Xử lý lỗi và trả về thông báo lỗi chi tiết
                return new ResponseData<ResidentInfoResponseDto>
                {
                    Success = false,
                    Message = ex.Message,
                    Code = (int)ErrorCodeAPI.SystemIsError
                };
            }
        }

        public async Task<ResponseData<List<AddMemberResponseDto>>> AddMembersAsync(AddMemberRequestDto request)
        {
            try
            {
                var responseList = new List<AddMemberResponseDto>();

                foreach (var member in request.Members)
                {
                    // Step 1: Insert into Residents table
                    var resident = new Resident
                    {
                        Id = Guid.NewGuid(),
                        Name = member.Name,
                        Email = member.Email,
                        Birthday = member.Birthday,
                        Phone = member.Phone,
                        IsOwner = false,
                        IsDeleted = false,
                        InsertedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        IsActive = true
                    };
                    _unitOfWork.ResidentRepository.Add(resident);

                    // Step 2: Insert into Livings table
                    var living = new Living
                    {
                        Id = Guid.NewGuid(),
                        StartDate = DateTime.Now,
                        ResidentId = resident.Id,
                        ApartmentId = request.ApartmentId,
                        IsDeleted = true,
                        InsertedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        IsActive = false
                    };
                    _unitOfWork.LivingRepository.Add(living);

                    // Step 3: Insert into ServiceContracts table
                    var serviceContract = new ServiceContract
                    {
                        Id = Guid.NewGuid(),
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now.AddMonths(12), // Assuming a default 12-month contract duration
                        Status = ServiceContractStatus.Pending, // Assuming 0 is the default status for a new contract (e.g., pending)
                        Quantity = 1,
                        Note = member.Note,
                        ApartmentId = request.ApartmentId,
                        ServiceId = request.ServiceId, // Pass the service ID from the request
                        //PackageServiceId = request.PackageServiceId, // Pass the package ID from the request
                        LivingId = living.Id, // Link to the Living record
                        IsDeleted = false,
                        InsertedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        IsActive = true
                    };
                    _unitOfWork.ServiceContractRepository.Add(serviceContract);

                    // Add response for each member
                    responseList.Add(new AddMemberResponseDto
                    {
                        Success = true,
                        Message = $"Member {member.Name} added successfully with service contract."
                    });
                }

                // Commit all changes at once
                await _unitOfWork.SaveChangesAsync();

                return new ResponseData<List<AddMemberResponseDto>>
                {
                    Success = true,
                    Message = "All members and service contracts added successfully.",
                    Data = responseList,
                    Code = (int)ErrorCodeAPI.OK
                };
            }
            catch (Exception ex)
            {
                return new ResponseData<List<AddMemberResponseDto>>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = new List<AddMemberResponseDto>
            {
                new AddMemberResponseDto { Success = false, Message = ex.Message }
            },
                    Code = (int)ErrorCodeAPI.SystemIsError
                };
            }
        }


        public async Task<ResponseData<MemberServiceDetailDto>> GetMemberServiceDetailAsync(Guid serviceContractId)
        {
            try
            {
                // Step 1: Retrieve the service contract details and associated apartment
                var serviceContract = await _unitOfWork.ServiceContractRepository
                    .GetQuery(sc => sc.Id == serviceContractId && sc.IsDeleted == false)
                    .Include(sc => sc.Apartment)
                        .ThenInclude(a => a.Building)
                    .FirstOrDefaultAsync();

                if (serviceContract == null)
                {
                    return new ResponseData<MemberServiceDetailDto>
                    {
                        Success = false,
                        Message = "Service contract not found.",
                        Code = (int)ErrorCodeAPI.NotFound
                    };
                }

                // Step 2: Retrieve the resident details using the Livings table associated with this service contract
                var living = await _unitOfWork.LivingRepository
                    .GetQueryWithInactive(l => l.Id == serviceContract.LivingId && l.IsDeleted == false)
                    .Include(l => l.Resident)
                    .FirstOrDefaultAsync();

                if (living == null || living.Resident == null)
                {
                    return new ResponseData<MemberServiceDetailDto>
                    {
                        Success = false,
                        Message = "Resident not found for this service contract.",
                        Code = (int)ErrorCodeAPI.NotFound
                    };
                }

                // Step 3: Prepare response data
                var response = new MemberServiceDetailDto
                {
                    ContractId = serviceContractId,
                    BuildingName = serviceContract.Apartment.Building.Name,
                    ApartmentNumber = serviceContract.Apartment.RoomNumber,
                    ServiceName = "Thêm thành viên", // Assuming "Add Member" is the fixed service type for this contract
                    MemberName = living.Resident.Name,
                    DateOfBirth = living.Resident.Birthday ?? DateTime.MinValue,
                    Email = living.Resident.Email,
                    PhoneNumber = living.Resident.Phone,
                    Note = serviceContract.Note,
                    Status = serviceContract.Status
                };

                return new ResponseData<MemberServiceDetailDto>
                {
                    Success = true,
                    Message = "Member service details retrieved successfully",
                    Data = response,
                    Code = (int)ErrorCodeAPI.OK
                };
            }
            catch (Exception ex)
            {
                return new ResponseData<MemberServiceDetailDto>
                {
                    Success = false,
                    Message = ex.Message,
                    Code = (int)ErrorCodeAPI.SystemIsError
                };
            }
        }


    }
}
