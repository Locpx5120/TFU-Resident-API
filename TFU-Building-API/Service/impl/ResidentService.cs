using BuildingModels;
using Constant;
using Core.Enums;
using Core.Model;
using fake_tool.Helpers;
using Microsoft.EntityFrameworkCore;
using TFU_Building_API.Core.Dapper.User;
using TFU_Building_API.Core.Handler;
using TFU_Building_API.Core.Helper;
using TFU_Building_API.Core.Infrastructure;
using TFU_Building_API.Dto;

namespace TFU_Building_API.Service.impl
{
    public class ResidentService : BaseHandler, IResidentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;
        private readonly IUserRepository _userRepository;

        public ResidentService(IUnitOfWork UnitOfWork, IHttpContextAccessor HttpContextAccessor, IConfiguration config,
            IUserRepository userRepository) : base(UnitOfWork, HttpContextAccessor)
        {
            _unitOfWork = UnitOfWork;
            _config = config;
            _userRepository = userRepository;
        }


        public async Task<ResponseData<ResidentResponseDto>> AddResident(ResidentRequestDto request)
        {
            try
            {
                var resident = _unitOfWork.ResidentRepository.GetQuery(x => x.Email == request.Email).FirstOrDefault();

                if (resident != null)
                {
                    return new ResponseData<ResidentResponseDto>
                    {
                        Success = false,
                        Message = MessConstant.ResidentHaveExist,
                        Data = null,
                        Code = (int)ErrorCodeAPI.DuplicateEntry
                    };
                }

                if (request.Birthday != null)
                {
                    DateTime birthday = request.Birthday.Value;
                    DateTime today = DateTime.Today;
                    int age = today.Year - birthday.Year;

                    if (today.Month < birthday.Month || (today.Month == birthday.Month && today.Day < birthday.Day))
                    {
                        age--;
                    }

                    // Kiểm tra nếu tuổi nhỏ hơn 1 thì không được đăng ký
                    if (age < 1)
                    {
                        return new ResponseData<ResidentResponseDto>
                        {
                            Success = false,
                            Message = "Thành viên không đủ tuổi đăng ký ít nhất 1 tuổi",
                            Data = null,
                            Code = (int)ErrorCodeAPI.DuplicateEntry
                        };
                    }
                }

                Resident residenCheck = _unitOfWork.ResidentRepository.GetQuery(x => x.Email.ToLower().Equals(request.Email.ToLower().Trim())).FirstOrDefault();
                if (residenCheck != null)
                {
                    return new ResponseData<ResidentResponseDto>
                    {
                        Success = false,
                        Message = "Email đã tồn tại trong bản cư dân",
                        Data = null,
                        Code = (int)ErrorCodeAPI.DuplicateEntry
                    };
                }

                Staff staff = _unitOfWork.StaffRepository.GetQuery(x => x.Email.ToLower().Equals(request.Email.ToLower().Trim())).FirstOrDefault();
                if (staff != null)
                {
                    return new ResponseData<ResidentResponseDto>
                    {
                        Success = false,
                        Message = "Email đã tồn tại trong bản nhân viên",
                        Data = null,
                        Code = (int)ErrorCodeAPI.DuplicateEntry
                    };
                }
                // Tạo mới Resident và thiết lập các giá trị từ request
                var newResident = new Resident
                {
                    Id = Guid.NewGuid(),
                    Name = request.Name,
                    Email = request.Email,
                    Phone = request.Phone,
                    RegistratorDate = request.RegistratorDate ?? DateTime.Now,
                    Birthday = request.Birthday ?? DateTime.Now,
                    IsOwner = false,
                    IsDeleted = false,
                    IsActive = true,
                    Password = Utill.GenerateRandomPassword(),
                };

                _unitOfWork.ResidentRepository.Add(newResident);
                await _unitOfWork.SaveChangesAsync();

                var emailService = new EmailService(_config);

                string subject = "Your New Account Details";
                string body = $@"
            <p>Dear {newResident.Name},</p>
            <p>Your new account has been created successfully. Below are your login details:</p>
            <p><b>Email:</b> {newResident.Email}</p>
            <p><b>Password:</b> {newResident.Password}</p>
            <p>Please log in and change your password as soon as possible.</p>
            <br/>
            <p>Best Regards,<br/>TFU Building Management Team</p>";

                await emailService.SendEmailAsync(newResident.Email, subject, body);

                // Trả về kết quả thành công với thông tin ResidentResponseDto
                var response = new ResidentResponseDto
                {
                    Id = newResident.Id
                };

                return new ResponseData<ResidentResponseDto>
                {
                    Success = true,
                    Message = MessConstant.Successfully,
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
                        Message = MessConstant.ResidentFoundZero,
                        Code = (int)ErrorCodeAPI.NotFound
                    };
                }

                // Cập nhật thông tin Resident
                existingResident.Name = request.Name;
                existingResident.Email = request.Email;
                existingResident.Phone = request.Phone;
                existingResident.UpdatedAt = DateTime.Now;
                existingResident.Birthday = request.DateOfBirth;

                // Lưu thay đổi
                _unitOfWork.ResidentRepository.Update(existingResident);
                await _unitOfWork.SaveChangesAsync();

                // Trả về kết quả thành công
                var response = new ResidentUpdateRequestDto
                {
                    Id = existingResident.Id,
                    Name = existingResident.Name,
                    Email = existingResident.Email,
                    Phone = existingResident.Phone,
                    DateOfBirth = existingResident.Birthday,
                };

                return new ResponseData<ResidentUpdateRequestDto>
                {
                    Success = true,
                    Message = MessConstant.UpdateSuccessfully,
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
                        Message = MessConstant.ResidentFoundZero,
                        Code = (int)ErrorCodeAPI.NotFound
                    };
                }

                var living = await _unitOfWork.LivingRepository.GetQuery(r => r.ResidentId == resident.Id && r.IsDeleted == false).FirstOrDefaultAsync();

                if (living == null)
                {
                    // Nếu không tìm thấy Resident, trả về thông báo lỗi
                    return new ResponseData<ResidentResponseDto>
                    {
                        Success = false,
                        Message = MessConstant.ResidentFoundZeroLiving,
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
                    Message = MessConstant.UpdateSuccessfully,
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
                        Message = MessConstant.ResidentFoundZero,
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
                    Message = MessConstant.FindSuccessfully,
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
                    var resident = _unitOfWork.ResidentRepository.GetQuery(false).Where(x => x.Email == member.Email).FirstOrDefault();
                    if (resident == null)
                    {
                        var residentNew = new Resident
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
                            IsActive = true,
                            Password = Utill.GenerateRandomPassword(),
                        };
                        _unitOfWork.ResidentRepository.Add(residentNew);

                        var emailService = new EmailService(_config);

                        string subject = "Your New Account Details";
                        string body = $@"
            <p>Dear {residentNew.Name},</p>
            <p>Your new account has been created successfully. Below are your login details:</p>
            <p><b>Email:</b> {residentNew.Email}</p>
            <p><b>Password:</b> {residentNew.Password}</p>
            <p>Please log in and change your password as soon as possible.</p>
            <br/>
            <p>Best Regards,<br/>TFU Building Management Team</p>";

                        await emailService.SendEmailAsync(residentNew.Email, subject, body);

                        resident = residentNew;
                    }
                    var checkLiving = _unitOfWork.LivingRepository
                        .GetQuery(false).Where(x =>
                         x.ResidentId.Equals(resident.Id) &&
                         x.ApartmentId.Equals(request.ApartmentId)
                        && x.IsDeleted == false
                        ).FirstOrDefault();
                    if (checkLiving != null)
                    {
                        return new ResponseData<List<AddMemberResponseDto>>
                        {
                            Success = false,
                            Message = MessConstant.ResidentHaveExist,
                            Data = null,
                            Code = (int)ErrorCodeAPI.DuplicateEntry
                        };
                    }

                    // Step 2: Insert into Livings table
                    var living = new Living
                    {
                        Id = Guid.NewGuid(),
                        StartDate = DateTime.Now,
                        ResidentId = resident.Id,
                        ApartmentId = request.ApartmentId,
                        IsDeleted = false,
                        IsActive = false,
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
                        Message = $"Thành viên {member.Name} thêm hợp đồng thành công."
                    });
                }

                // Commit all changes at once
                await _unitOfWork.SaveChangesAsync();

                return new ResponseData<List<AddMemberResponseDto>>
                {
                    Success = true,
                    Message = MessConstant.Successfully,
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
                        Message = MessConstant.ServiceContractFoundZero,
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
                        Message = MessConstant.ResidentFoundZeroLiving,
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
                    Message = MessConstant.UpdateSuccessfully,
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

        public async Task<ResponseData<List<GetResidentResponseDto>>> GetResidentsAsync()
        {
            try
            {
                List<GetResidentResponseDto> residentResponseDtos = new List<GetResidentResponseDto>();
                var responseList = _unitOfWork.ResidentRepository.GetQuery(x => x.IsDeleted == false).ToList();
                if (responseList.Any())
                {
                    responseList = responseList.ToList().OrderByDescending(x => x.InsertedAt).ToList();
                }
                foreach (var item in responseList)
                {
                    // Step 1: Insert into Residents table
                    var resident = new GetResidentResponseDto
                    {
                        Id = item.Id,
                        RegistratorDate = item.RegistratorDate,
                        Name = item.Name,
                        Email = item.Email,
                        Birthday = item.Birthday,
                        Phone = item.Phone,
                        IsOwner = item.IsOwner,
                    };
                    residentResponseDtos.Add(resident);
                }

                return new ResponseData<List<GetResidentResponseDto>>
                {
                    Success = true,
                    Message = MessConstant.FindSuccessfully,
                    Data = residentResponseDtos,
                    Code = (int)ErrorCodeAPI.OK
                };
            }
            catch (Exception ex)
            {
                return new ResponseData<List<GetResidentResponseDto>>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = new List<GetResidentResponseDto>
            {
               null
            },
                    Code = (int)ErrorCodeAPI.SystemIsError
                };
            }
        }
    }
}
