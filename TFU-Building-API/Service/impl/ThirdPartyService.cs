using BuildingModels;
using Core.Enums;
using Core.Model;
using Microsoft.EntityFrameworkCore;
using TFU_Building_API.Core.Helper;
using TFU_Building_API.Core.Infrastructure;
using TFU_Building_API.Dto;

namespace TFU_Building_API.Service.impl
{
    public class ThirdPartyService : IThirdPartyService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ThirdPartyService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseData<AddThirdPartyResponseDto>> AddThirdPartyAsync(AddThirdPartyRequestDto request)
        {
            try
            {
                // Kiểm tra xem email đã tồn tại trong bảng Staff hay chưa
                var existingStaff = await _unitOfWork.StaffRepository
                    .GetQuery(s => s.Email == request.ContactInfo && s.IsDeleted == false)
                    .FirstOrDefaultAsync();

                if (existingStaff != null)
                {
                    // Nếu email đã tồn tại, trả về thông báo lỗi
                    return new ResponseData<AddThirdPartyResponseDto>
                    {
                        Success = false,
                        Message = "Email đã tồn tại. Vui lòng sử dụng một email khác.",
                        Code = (int)ErrorCodeAPI.EmailUsed
                    };
                }

                // Bước 1: Tạo mật khẩu ngẫu nhiên
                string generatedPassword = GenerateRandomPassword();

                // Bước 2: Tạo tài khoản staff cho đăng nhập bên thứ ba mà không băm mật khẩu
                var staff = new Staff
                {
                    Id = Guid.NewGuid(),
                    FullName = request.CompanyName,
                    Email = request.ContactInfo,
                    Password = generatedPassword, // Plain password, không băm mật khẩu
                    PhoneNumber = "", // Không bắt buộc, không có trong yêu cầu
                    RoleId = RoleConstants.TenantRoleId, // Gán ID role cụ thể cho bên thuê thứ ba
                    IsDeleted = false,
                    InsertedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    IsActive = true,
                    IsChangePassword = false
                };
                _unitOfWork.StaffRepository.Add(staff);

                // Bước 3: Thêm vào bảng ThirdParties
                var thirdParty = new ThirdParty
                {
                    Id = Guid.NewGuid(),
                    NameCompany = request.CompanyName,
                    Description = request.StoreType,
                    Status = ThirdPartyStatus.WithinTerm,
                    IsTenant = true,
                    StaffId = staff.Id,
                    IsDeleted = false,
                    InsertedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    IsActive = true
                };
                _unitOfWork.ThirdPartyRepository.Add(thirdParty);

                // Lưu tất cả thay đổi
                await _unitOfWork.SaveChangesAsync();

                // Trả về response với thông tin tài khoản đã tạo
                return new ResponseData<AddThirdPartyResponseDto>
                {
                    Success = true,
                    Message = "Thêm bên thuê thứ ba thành công.",
                    Data = new AddThirdPartyResponseDto
                    {
                        Success = true,
                        Message = "Tài khoản đã được tạo thành công.",
                        Username = staff.Email,
                        Password = generatedPassword // Trả về mật khẩu để thông báo cho người dùng
                    },
                    Code = (int)ErrorCodeAPI.OK
                };
            }
            catch (Exception ex)
            {
                return new ResponseData<AddThirdPartyResponseDto>
                {
                    Success = false,
                    Message = ex.Message,
                    Code = (int)ErrorCodeAPI.SystemIsError
                };
            }
        }


        private string GenerateRandomPassword()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, 8)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public async Task<ResponseData<AddThirdPartyContactResponseDto>> AddThirdPartyContactAsync(AddThirdPartyContactRequestDto request)
        {
            try
            {
                Guid? apartmentId = null;

                // Kiểm tra nếu có thông tin `BuildingId`, `FloorNumber`, `RoomNumber` thì mới lấy `ApartmentId`
                if (request.BuildingId.HasValue && request.FloorNumber.HasValue && request.RoomNumber.HasValue)
                {
                    var apartment = await _unitOfWork.ApartmentRepository
                        .GetQuery(a => a.BuildingId == request.BuildingId &&
                                       a.FloorNumber == request.FloorNumber &&
                                       a.RoomNumber == request.RoomNumber &&
                                       a.IsDeleted == false)
                        .FirstOrDefaultAsync();

                    if (apartment == null)
                    {
                        return new ResponseData<AddThirdPartyContactResponseDto>
                        {
                            Success = false,
                            Message = "Apartment not found with the specified building, floor, and room.",
                            Data = new AddThirdPartyContactResponseDto { Success = false, Message = "Invalid apartment details." },
                            Code = (int)ErrorCodeAPI.InvalidData
                        };
                    }

                    apartmentId = apartment.Id;
                }

                // Tạo đối tượng hợp đồng bên thứ ba
                var thirdPartyContact = new ThirdPartyContact
                {
                    Id = Guid.NewGuid(),
                    StartDate = request.StartDate,
                    EndDate = request.EndDate,
                    Price = request.Price,
                    NameService = request.NameService,
                    ThirdPartyId = request.ThirdPartyId,
                    ApartmentId = apartmentId, // `ApartmentId` có thể là null
                    IsDeleted = false,
                    InsertedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    IsActive = true
                };

                // Thêm hợp đồng vào cơ sở dữ liệu
                _unitOfWork.ThirdPartyContractRepository.Add(thirdPartyContact);
                await _unitOfWork.SaveChangesAsync();

                return new ResponseData<AddThirdPartyContactResponseDto>
                {
                    Success = true,
                    Message = "Third-party contract added successfully.",
                    Data = new AddThirdPartyContactResponseDto { Success = true, Message = "Contract added successfully." },
                    Code = (int)ErrorCodeAPI.OK
                };
            }
            catch (Exception ex)
            {
                return new ResponseData<AddThirdPartyContactResponseDto>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = new AddThirdPartyContactResponseDto { Success = false, Message = "Failed to add contract." },
                    Code = (int)ErrorCodeAPI.SystemIsError
                };
            }
        }


        //public async Task<ResponseData<List<ThirdPartyListResponseDto>>> GetThirdPartyListAsync(ThirdPartyListRequestDto request)
        //{
        //    try
        //    {
        //        var query = from tp in _unitOfWork.ThirdPartyRepository.GetQuery(x => x.IsDeleted == false && x.IsTenant == true)
        //                    join tpc in _unitOfWork.ThirdPartyContractRepository.GetQuery(x => x.IsDeleted == false) on tp.Id equals tpc.ThirdPartyId
        //                    join a in _unitOfWork.ApartmentRepository.GetQuery(x => x.IsDeleted == false) on tpc.ApartmentId equals a.Id into apartmentJoin
        //                    from aj in apartmentJoin.DefaultIfEmpty() // Allow null for ApartmentId
        //                    join b in _unitOfWork.BuildingRepository.GetQuery(x => x.IsDeleted == false) on aj.BuildingId equals b.Id into buildingJoin
        //                    from bj in buildingJoin.DefaultIfEmpty() // Allow null for BuildingId
        //                    join s in _unitOfWork.StaffRepository.GetQuery(x => x.IsDeleted == false) on tp.StaffId equals s.Id
        //                    select new
        //                    {
        //                        tp.Id,
        //                        tp.NameCompany,
        //                        ContactInfo = new { s.Email, s.PhoneNumber }, // Thông tin liên hệ từ bảng Staff
        //                        tp.Description,
        //                        tpc.StartDate,
        //                        tpc.EndDate,
        //                        BuildingName = bj.Name // Tên tòa nhà
        //                    };

        //        // Tìm kiếm theo tên công ty
        //        if (!string.IsNullOrEmpty(request.CompanyName))
        //        {
        //            query = query.Where(x => x.NameCompany.Contains(request.CompanyName));
        //        }

        //        // Lọc theo trạng thái
        //        DateTime oneMonthFromNow = DateTime.Now.AddMonths(1);
        //        if (request.Status == "Trong thời hạn")
        //        {
        //            query = query.Where(x => x.EndDate > DateTime.Now);
        //        }
        //        else if (request.Status == "Chuẩn bị hết hạn")
        //        {
        //            query = query.Where(x => x.EndDate <= oneMonthFromNow && x.EndDate > DateTime.Now);
        //        }

        //        // Lấy dữ liệu và chuyển đổi thành danh sách response
        //        var result = await query.Select(item => new ThirdPartyListResponseDto
        //        {
        //            ThirdPartyId = item.Id,
        //            CompanyName = item.NameCompany,
        //            ContactInfo = $"{item.ContactInfo.Email}, {item.ContactInfo.PhoneNumber}", // Định dạng thông tin liên hệ
        //            StoreType = item.Description,
        //            StartDate = item.StartDate,
        //            EndDate = item.EndDate,
        //            BuildingName = item.BuildingName
        //        }).ToListAsync();

        //        return new ResponseData<List<ThirdPartyListResponseDto>>
        //        {
        //            Success = true,
        //            Message = "Successfully retrieved third-party list.",
        //            Data = result,
        //            Code = (int)ErrorCodeAPI.OK
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        return new ResponseData<List<ThirdPartyListResponseDto>>
        //        {
        //            Success = false,
        //            Message = ex.Message,
        //            Code = (int)ErrorCodeAPI.SystemIsError
        //        };
        //    }
        //}

        public async Task<ResponseData<List<ThirdPartyListResponseDto>>> GetThirdPartyListAsync(ThirdPartyListRequestDto request)
        {
            try
            {
                DateTime now = DateTime.Now;
                DateTime oneMonthFromNow = now.AddMonths(1);

                // Step 1: Build the query with optional joins
                var query = from tp in _unitOfWork.ThirdPartyRepository.GetQuery(x => x.IsDeleted == false && x.IsTenant == request.IsTenant)
                            join tpc in _unitOfWork.ThirdPartyContractRepository.GetQuery(x => x.IsDeleted == false)
                                on tp.Id equals tpc.ThirdPartyId into tpcJoin
                            from tpc in tpcJoin.DefaultIfEmpty() // Left join for ThirdPartyContract
                            join a in _unitOfWork.ApartmentRepository.GetQuery(x => x.IsDeleted == false)
                                on tpc.ApartmentId equals a.Id into apartmentJoin
                            from aj in apartmentJoin.DefaultIfEmpty() // Left join for Apartment
                            join b in _unitOfWork.BuildingRepository.GetQuery(x => x.IsDeleted == false)
                                on aj.BuildingId equals b.Id into buildingJoin
                            from bj in buildingJoin.DefaultIfEmpty() // Left join for Building
                            join s in _unitOfWork.StaffRepository.GetQuery(x => x.IsDeleted == false)
                                on tp.StaffId equals s.Id
                            select new
                            {
                                tp.Id,
                                tp.NameCompany,
                                ContactInfo = new { s.Email, s.PhoneNumber }, // Contact info from Staff table
                                tp.Description,
                                StartDate = tpc == null ? (DateTime?)null : tpc.StartDate, // Handle null for ThirdPartyContract
                                EndDate = tpc == null ? (DateTime?)null : tpc.EndDate, // Handle null for ThirdPartyContract
                                BuildingName = bj == null ? null : bj.Name, // Handle null for Building
                                //ContractStatus = tpc == null || tpc.EndDate == null || tpc.EndDate <= now
                                //    ? "Hết hạn" // Expired or missing EndDate
                                //    : (tpc.EndDate <= oneMonthFromNow && tpc.EndDate > now)
                                //        ? "Chuẩn bị hết hạn" // Expiring within one month
                                //        : "Trong thời hạn" // Valid and not expiring soon
                                ContractStatus = tpc == null
                                ? "Chưa thanh toán" // If there is no contract
                                : (tpc.EndDate == null && tp.Status == false)
                                    ? "Chưa thanh toán" // If EndDate is null and Status is false
                                    : (tpc.EndDate <= now)
                                        ? "Hết hạn" // If EndDate is in the past
                                        : (tpc.EndDate <= oneMonthFromNow && tpc.EndDate > now)
                                            ? "Chuẩn bị hết hạn" // If EndDate is within one month
                                            : "Trong thời hạn" // If EndDate is in the future and more than one month away

                            };

                // Step 2: Apply search filter
                if (!string.IsNullOrEmpty(request.CompanyName))
                {
                    query = query.Where(x => x.NameCompany.Contains(request.CompanyName));
                }

                // Step 3: Apply status filter
                if (!string.IsNullOrEmpty(request.Status))
                {
                    switch (request.Status.ToLower())
                    {
                        case "hết hạn":
                            query = query.Where(x => x.ContractStatus == "Hết hạn");
                            break;
                        case "chuẩn bị hết hạn":
                            query = query.Where(x => x.ContractStatus == "Chuẩn bị hết hạn");
                            break;
                        case "trong thời hạn":
                            query = query.Where(x => x.ContractStatus == "Trong thời hạn");
                            break;
                    }
                }

                // Step 4: Retrieve and transform data into response DTO
                var result = await query.Select(item => new ThirdPartyListResponseDto
                {
                    ThirdPartyId = item.Id,
                    CompanyName = item.NameCompany,
                    ContactInfo = $"{item.ContactInfo.Email}, {item.ContactInfo.PhoneNumber}", // Format contact info
                    StoreType = item.Description,
                    StartDate = item.StartDate,
                    EndDate = item.EndDate,
                    BuildingName = item.BuildingName,
                    Status = item.ContractStatus // Add the contract status to the response
                }).ToListAsync();

                // Step 5: Return successful response
                return new ResponseData<List<ThirdPartyListResponseDto>>
                {
                    Success = true,
                    Message = "Successfully retrieved third-party list.",
                    Data = result,
                    Code = (int)ErrorCodeAPI.OK
                };
            }
            catch (Exception ex)
            {
                // Return error response in case of exception
                return new ResponseData<List<ThirdPartyListResponseDto>>
                {
                    Success = false,
                    Message = $"An error occurred: {ex.Message}",
                    Code = (int)ErrorCodeAPI.SystemIsError
                };
            }
        }

        //public async Task<ResponseData<List<ContractDetailResponseDto>>> GetContractDetailsByThirdPartyIdAsync(ContractDetailRequestDto request)
        //{
        //    try
        //    {
        //        // Validate input
        //        if (request.ThirdPartyId == Guid.Empty)
        //        {
        //            return new ResponseData<List<ContractDetailResponseDto>>
        //            {
        //                Success = false,
        //                Message = "Invalid ThirdPartyId.",
        //                Code = (int)ErrorCodeAPI.BadRequest
        //            };
        //        }

        //        // Query the details
        //        var query = from tpc in _unitOfWork.ThirdPartyContractRepository.GetQuery(x => x.IsDeleted == false && x.ThirdPartyId == request.ThirdPartyId)
        //                    join a in _unitOfWork.ApartmentRepository.GetQuery(x => x.IsDeleted == false)
        //                        on tpc.ApartmentId equals a.Id
        //                    join b in _unitOfWork.BuildingRepository.GetQuery(x => x.IsDeleted == false)
        //                        on a.BuildingId equals b.Id
        //                    join tp in _unitOfWork.ThirdPartyRepository.GetQuery(x => x.IsDeleted == false)
        //                        on tpc.ThirdPartyId equals tp.Id
        //                    select new ContractDetailResponseDto
        //                    {
        //                        CompanyName = tp.NameCompany,
        //                        Floor = a.FloorNumber,
        //                        Room = a.RoomNumber,
        //                        Area = a.ApartmentType.LandArea, // Assume this represents the area in square meters
        //                        StartDate = tpc.StartDate ?? DateTime.MinValue,
        //                        EndDate = tpc.EndDate ?? DateTime.MinValue,
        //                        ServicePrice = tpc.Price
        //                    };

        //        var result = await query.ToListAsync();

        //        return new ResponseData<List<ContractDetailResponseDto>>
        //        {
        //            Success = true,
        //            Message = "Successfully retrieved contract details.",
        //            Data = result,
        //            Code = (int)ErrorCodeAPI.OK
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        return new ResponseData<List<ContractDetailResponseDto>>
        //        {
        //            Success = false,
        //            Message = $"An error occurred: {ex.Message}",
        //            Code = (int)ErrorCodeAPI.SystemIsError
        //        };
        //    }
        //}

        public async Task<ResponseData<List<ContractDetailResponseDto>>> GetContractDetailsByThirdPartyIdAsync(ContractDetailRequestDto request)
        {
            try
            {
                // Validate input
                if (request.ThirdPartyId == Guid.Empty)
                {
                    return new ResponseData<List<ContractDetailResponseDto>>
                    {
                        Success = false,
                        Message = "Invalid ThirdPartyId.",
                        Code = (int)ErrorCodeAPI.BadRequest
                    };
                }

                // Query the details
                //var query = from tpc in _unitOfWork.ThirdPartyContractRepository.GetQuery(x => x.IsDeleted == false && x.ThirdPartyId == request.ThirdPartyId)
                //            join a in _unitOfWork.ApartmentRepository.GetQuery(x => x.IsDeleted == false)
                //                on tpc.ApartmentId equals a.Id into apartmentJoin
                //            from aj in apartmentJoin.DefaultIfEmpty() // Allow null for ApartmentId
                //            join b in _unitOfWork.BuildingRepository.GetQuery(x => x.IsDeleted == false)
                //                on aj.BuildingId equals b.Id into buildingJoin
                //            from bj in buildingJoin.DefaultIfEmpty() // Allow null for BuildingId
                //            join tp in _unitOfWork.ThirdPartyRepository.GetQuery(x => x.IsDeleted == false)
                //                on tpc.ThirdPartyId equals tp.Id
                //            select new ContractDetailResponseDto
                //            {
                //                CompanyName = tp.NameCompany,
                //                Floor = aj == null ? 0 : aj.FloorNumber, // Handle null apartment
                //                Room = aj == null ? 0 : aj.RoomNumber, // Handle null apartment
                //                Area = aj == null ? 0 : aj.ApartmentType.LandArea, // Handle null apartment type
                //                NameService = tpc.NameService, // Include NameService
                //                StartDate = (DateTime)((tp.Status == false && tp.IsTenant == false) ? null : tpc.StartDate), // Null if Status is false and IsTenant is false
                //                EndDate = (DateTime)((tp.Status == false && tp.IsTenant == false) ? null : tpc.EndDate), // Null if Status is false and IsTenant is false
                //                ServicePrice = tpc.Price
                //            };
                var query = from tpc in _unitOfWork.ThirdPartyContractRepository.GetQuery(x => x.IsDeleted == false && x.ThirdPartyId == request.ThirdPartyId)
                            join a in _unitOfWork.ApartmentRepository.GetQuery(x => x.IsDeleted == false)
                                on tpc.ApartmentId equals a.Id into apartmentJoin
                            from aj in apartmentJoin.DefaultIfEmpty() // Allow null for ApartmentId
                            join b in _unitOfWork.BuildingRepository.GetQuery(x => x.IsDeleted == false)
                                on aj.BuildingId equals b.Id into buildingJoin
                            from bj in buildingJoin.DefaultIfEmpty() // Allow null for BuildingId
                            join tp in _unitOfWork.ThirdPartyRepository.GetQuery(x => x.IsDeleted == false)
                                on tpc.ThirdPartyId equals tp.Id
                            select new ContractDetailResponseDto
                            {
                                CompanyName = tp.NameCompany,
                                Floor = aj == null ? 0 : aj.FloorNumber,
                                                                                         Room = aj == null ? 0 : aj.RoomNumber, // Handle null apartment
                                                                                        Area = aj == null ? 0 : aj.ApartmentType.LandArea, // Handle null apartment type
                                NameService = tpc.NameService, // Include NameService
                                StartDate = (tp.Status == false && tp.IsTenant == false) ? null : tpc.StartDate, // Null if Status is false and IsTenant is false
                                EndDate = (tp.Status == false && tp.IsTenant == false) ? null : tpc.EndDate, // Null if Status is false and IsTenant is false
                                ServicePrice = tpc.Price
                            };


                var result = await query.ToListAsync();

                return new ResponseData<List<ContractDetailResponseDto>>
                {
                    Success = true,
                    Message = "Successfully retrieved contract details.",
                    Data = result,
                    Code = (int)ErrorCodeAPI.OK
                };
            }
            catch (Exception ex)
            {
                return new ResponseData<List<ContractDetailResponseDto>>
                {
                    Success = false,
                    Message = $"An error occurred: {ex.Message}",
                    Code = (int)ErrorCodeAPI.SystemIsError
                };
            }
        }


        public async Task<ResponseData<ThirdPartyContractDetailDto>> GetThirdPartyContractDetailAsync(Guid thirdPartyId)
        {
            try
            {
                var contractDetail = await (from tp in _unitOfWork.ThirdPartyRepository.GetQuery(x => x.IsDeleted == false && x.Id == thirdPartyId)
                                            join tpc in _unitOfWork.ThirdPartyContractRepository.GetQuery(x => x.IsDeleted == false)
                                                on tp.Id equals tpc.ThirdPartyId
                                            join a in _unitOfWork.ApartmentRepository.GetQuery(x => x.IsDeleted == false)
                                                on tpc.ApartmentId equals a.Id into apartmentJoin
                                            from a in apartmentJoin.DefaultIfEmpty()
                                            join b in _unitOfWork.BuildingRepository.GetQuery(x => x.IsDeleted == false)
                                                on a.BuildingId equals b.Id into buildingJoin
                                            from b in buildingJoin.DefaultIfEmpty()
                                            select new ThirdPartyContractDetailDto
                                            {
                                                ApartmentId = a.Id,
                                                CompanyName = tp.NameCompany,
                                                BuildingName = b != null ? b.Name : null,
                                                FloorNumber = a != null ? a.FloorNumber : null,
                                                RoomNumber = a != null ? a.RoomNumber : null,
                                                StartDate = tpc.StartDate,
                                                EndDate = tpc.EndDate,
                                                Price = tpc.Price
                                            }).FirstOrDefaultAsync();

                if (contractDetail == null)
                {
                    return new ResponseData<ThirdPartyContractDetailDto>
                    {
                        Success = false,
                        Message = "Không tìm thấy hợp đồng.",
                        Code = (int)ErrorCodeAPI.NotFound
                    };
                }

                return new ResponseData<ThirdPartyContractDetailDto>
                {
                    Success = true,
                    Message = "Lấy chi tiết hợp đồng thành công.",
                    Data = contractDetail,
                    Code = (int)ErrorCodeAPI.OK
                };
            }
            catch (Exception ex)
            {
                return new ResponseData<ThirdPartyContractDetailDto>
                {
                    Success = false,
                    Message = ex.Message,
                    Code = (int)ErrorCodeAPI.SystemIsError
                };
            }
        }

        public async Task<ResponseData<List<ThirdPartyContractInfoDto>>> GetContractDetailsForThirdPartyAsync(Guid staffId)
        {
            try
            {
                // Tìm bên thứ ba dựa trên staffId
                var thirdParty = await _unitOfWork.ThirdPartyRepository
                    .GetQuery(tp => tp.StaffId == staffId && tp.IsDeleted == false)
                    .FirstOrDefaultAsync();

                if (thirdParty == null)
                {
                    return new ResponseData<List<ThirdPartyContractInfoDto>>
                    {
                        Success = false,
                        Message = "Không tìm thấy bên thứ ba.",
                        Code = (int)ErrorCodeAPI.NotFound
                    };
                }

                // Lấy danh sách hợp đồng
                var contractDetails = await (from tpc in _unitOfWork.ThirdPartyContractRepository.GetQuery(c => c.IsDeleted == false && c.ThirdPartyId == thirdParty.Id)
                                             join a in _unitOfWork.ApartmentRepository.GetQuery(a => a.IsDeleted == false)
                                                 on tpc.ApartmentId equals a.Id into apartmentJoin
                                             from a in apartmentJoin.DefaultIfEmpty()
                                             join at in _unitOfWork.ApartmentTypeRepository.GetQuery(at => at.IsDeleted == false)
                                                 on a.ApartmentTypeId equals at.Id into apartmentTypeJoin
                                             from at in apartmentTypeJoin.DefaultIfEmpty()
                                             join b in _unitOfWork.BuildingRepository.GetQuery(b => b.IsDeleted == false)
                                                 on a.BuildingId equals b.Id into buildingJoin
                                             from b in buildingJoin.DefaultIfEmpty()
                                             select new ThirdPartyContractInfoDto
                                             {
                                                 BuildingName = b != null ? b.Name : null,
                                                 ApartmentId = a.Id,
                                                 FloorNumber = a != null ? a.FloorNumber : null,
                                                 RoomNumber = a != null ? a.RoomNumber : null,
                                                 Area = at != null ? at.LandArea : 0, // Diện tích từ bảng ApartmentType
                                                 StartDate = tpc.StartDate,
                                                 EndDate = tpc.EndDate,
                                                 Price = tpc.Price,
                                                 Status = tpc.EndDate <= DateTime.Now.AddMonths(1) ? "Chuẩn bị hết hạn" : "Trong thời hạn"
                                             }).ToListAsync();

                return new ResponseData<List<ThirdPartyContractInfoDto>>
                {
                    Success = true,
                    Message = "Lấy thông tin hợp đồng thành công.",
                    Data = contractDetails,
                    Code = (int)ErrorCodeAPI.OK
                };
            }
            catch (Exception ex)
            {
                return new ResponseData<List<ThirdPartyContractInfoDto>>
                {
                    Success = false,
                    Message = ex.Message,
                    Code = (int)ErrorCodeAPI.SystemIsError
                };
            }
        }

        public async Task<ResponseData<string>> AddThirdPartyHireAsync(AddThirdPartyHireRequestDto request)
        {
            try
            {
                // Step 1: Add staff record with the company name
                var newStaff = new Staff
                {
                    Id = Guid.NewGuid(),
                    FullName = request.NameCompany,
                    Email = request.ContactInfo,
                    Password = GenerateRandomPassword(), // No password needed
                    PhoneNumber = null,
                    RoleId = RoleConstants.TenantRoleId, // Define the role for staff if required
                    IsChangePassword = false,
                    IsDeleted = false,
                    IsActive = true,
                    InsertedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                 _unitOfWork.StaffRepository.Add(newStaff);

                // Step 2: Add third party record with the StaffId
                var newThirdParty = new ThirdParty
                {
                    Id = Guid.NewGuid(),
                    NameCompany = request.NameCompany,
                    Status = false, // Assuming false means chưa thanh toán
                    IsTenant = false, // Not a tenant, you're hiring them
                    StaffId = newStaff.Id,
                    IsDeleted = false,
                    IsActive = true,
                    Description = "Thuê dịch vụ bên thứ 3",
                    InsertedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                 _unitOfWork.ThirdPartyRepository.Add(newThirdParty);
                await _unitOfWork.SaveChangesAsync();

                return new ResponseData<string>
                {
                    Success = true,
                    Message = "Third party and staff added successfully.",
                    Data = $"Third party {request.NameCompany} added successfully.",
                    Code = (int)ErrorCodeAPI.OK
                };
            }
            catch (Exception ex)
            {
                return new ResponseData<string>
                {
                    Success = false,
                    Message = ex.Message,
                    Code = (int)ErrorCodeAPI.SystemIsError
                };
            }
        }

        //public async Task<ResponseData<PaginatedResponseDto<TenantRentResponseDto>>> GetTenantRentHistoryAsync(GetTenantRentRequestDto request)
        //{
        //    try
        //    {
        //        // Step 1: Query ThirdParties with IsTenant = true
        //        var query = from tp in _unitOfWork.ThirdPartyRepository.GetQuery(t => (t.IsTenant == request.Istenant) && (t.IsDeleted == false))
        //                    join tpc in _unitOfWork.ThirdPartyContractRepository.GetQuery(c => (c.IsDeleted == false))
        //                        on tp.Id equals tpc.ThirdPartyId
        //                    join a in _unitOfWork.ApartmentRepository.GetQuery(a => (a.IsDeleted == false))
        //                        on tpc.ApartmentId equals a.Id
        //                    select new TenantRentResponseDto
        //                    {
        //                        CompanyName = tp.NameCompany,
        //                        Area = (decimal)a.Price, // Assuming Area is based on Apartment price or size
        //                        EndDate = (DateTime)tpc.EndDate,
        //                        RentAmount = tpc.Price,
        //                        Description = tp.Description
        //                    };

        //        // Step 2: Apply search filter
        //        if (!string.IsNullOrEmpty(request.CompanyName))
        //        {
        //            query = query.Where(x => x.CompanyName.Contains(request.CompanyName));
        //        }

        //        // Step 3: Apply end date filter
        //        if (request.EndDateFilter.HasValue)
        //        {
        //            query = query.Where(x => x.EndDate.Date == request.EndDateFilter.Value.Date);
        //        }

        //        // Step 4: Pagination
        //        var totalRecords = await query.CountAsync();
        //        var data = await query
        //            .Skip((request.PageNumber - 1) * request.PageSize)
        //            .Take(request.PageSize)
        //            .ToListAsync();

        //        // Step 5: Prepare response
        //        var response = new PaginatedResponseDto<TenantRentResponseDto>
        //        {
        //            TotalRecords = totalRecords,
        //            Data = data
        //        };

        //        return new ResponseData<PaginatedResponseDto<TenantRentResponseDto>>
        //        {
        //            Success = true,
        //            Message = "Tenant rent history retrieved successfully.",
        //            Data = response,
        //            Code = (int)ErrorCodeAPI.OK
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        return new ResponseData<PaginatedResponseDto<TenantRentResponseDto>>
        //        {
        //            Success = false,
        //            Message = ex.Message,
        //            Code = (int)ErrorCodeAPI.SystemIsError
        //        };
        //    }
        //}

        public async Task<ResponseData<PaginatedResponseDto<TenantRentResponseDto>>> GetTenantRentHistoryAsync(GetTenantRentRequestDto request)
        {
            try
            {
                // Step 1: Query ThirdParties and ThirdPartyContracts based on IsTenant filter
                var query = from tp in _unitOfWork.ThirdPartyRepository.GetQuery(t => t.IsTenant == request.IsTenant && t.IsDeleted == false)
                            join tpc in _unitOfWork.ThirdPartyContractRepository.GetQuery(c => c.IsDeleted == false)
                            on tp.Id equals tpc.ThirdPartyId
                            select new
                            {
                                tp,
                                tpc,
                                ApartmentArea = (decimal?)null // Default for non-tenant
                            };

                // Step 2: If IsTenant is true, join with Apartments and include ApartmentArea
                if (request.IsTenant)
                {
                    query = from combined in query
                            join a in _unitOfWork.ApartmentRepository.GetQuery(a => a.IsDeleted == false)
                            on combined.tpc.ApartmentId equals a.Id
                            select new
                            {
                                combined.tp,
                                combined.tpc,
                                ApartmentArea = (decimal?)a.Price // Replace null with Apartment price
                            };
                }

                // Step 3: Apply search filter
                if (!string.IsNullOrEmpty(request.CompanyName))
                {
                    query = query.Where(x => x.tp.NameCompany.Contains(request.CompanyName));
                }

                // Step 4: Apply end date filter
                if (request.EndDateFilter.HasValue)
                {
                    query = query.Where(x => x.tpc.EndDate.HasValue && x.tpc.EndDate.Value.Date == request.EndDateFilter.Value.Date);
                }

                // Step 5: Select required fields for the result
                var resultQuery = query.Select(x => new TenantRentResponseDto
                {
                    CompanyName = x.tp.NameCompany,
                    Area = x.tpc.Apartment.ApartmentType.LandArea, // Nullable for non-tenant
                    EndDate = x.tpc.EndDate.Value, // Assume EndDate is always non-null at this point
                    RentAmount = x.tpc.Price,
                    Description = x.tp.Description
                });

                // Step 6: Apply pagination
                var totalRecords = await resultQuery.CountAsync();
                var data = await resultQuery
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToListAsync();

                // Step 7: Prepare response
                var response = new PaginatedResponseDto<TenantRentResponseDto>
                {
                    TotalRecords = totalRecords,
                    Data = data
                };

                return new ResponseData<PaginatedResponseDto<TenantRentResponseDto>>
                {
                    Success = true,
                    Message = "Tenant rent history retrieved successfully.",
                    Data = response,
                    Code = (int)ErrorCodeAPI.OK
                };
            }
            catch (Exception ex)
            {
                // Return a meaningful error message
                return new ResponseData<PaginatedResponseDto<TenantRentResponseDto>>
                {
                    Success = false,
                    Message = $"An error occurred while retrieving tenant rent history: {ex.Message}",
                    Code = (int)ErrorCodeAPI.SystemIsError
                };
            }
        }

        public async Task<ResponseData<PaginatedResponseDto<ThirdPartyHireResponseDto>>> GetThirdPartiesAsync(GetThirdPartyHireRequestDto request)
        {
            try
            {
                var currentDate = DateTime.Now;
                var oneMonthLater = currentDate.AddMonths(1);

                // Step 1: Query ThirdParties
                var query = _unitOfWork.ThirdPartyRepository.GetQuery(tp => tp.IsTenant == false && tp.IsDeleted == false);

                // Step 2: Apply search filter
                if (!string.IsNullOrEmpty(request.CompanyName))
                {
                    query = query.Where(tp => tp.NameCompany.Contains(request.CompanyName));
                }

                // Step 3: Apply status filter
                if (!string.IsNullOrEmpty(request.StatusFilter))
                {
                    switch (request.StatusFilter.ToLower())
                    {
                        case "chưa thanh toán":
                            query = query.Where(tp => tp.Status == false);
                            break;
                        case "chuẩn bị hết hạn":
                            query = query.Where(tp => tp.Status == true && tp.UpdatedAt >= currentDate && tp.UpdatedAt <= oneMonthLater);
                            break;
                        case "trong thời hạn":
                            query = query.Where(tp => tp.Status == true && tp.UpdatedAt > oneMonthLater);
                            break;
                    }
                }

                // Step 4: Select required fields
                var resultQuery = query.Select(tp => new ThirdPartyHireResponseDto
                {
                    Id = tp.Id,
                    NameCompany = tp.NameCompany,
                    ContactInfo = tp.Description, // Replace with actual contact info field if available
                    Status = tp.Status == false
                        ? "Chưa thanh toán"
                        : (tp.UpdatedAt >= currentDate && tp.UpdatedAt <= oneMonthLater)
                            ? "Chuẩn bị hết hạn"
                            : "Trong thời hạn"
                });

                // Step 5: Apply pagination
                var totalRecords = await resultQuery.CountAsync();
                var data = await resultQuery
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToListAsync();

                // Step 6: Prepare response
                var response = new PaginatedResponseDto<ThirdPartyHireResponseDto>
                {
                    TotalRecords = totalRecords,
                    Data = data
                };

                return new ResponseData<PaginatedResponseDto<ThirdPartyHireResponseDto>>
                {
                    Success = true,
                    Message = "Third-party data retrieved successfully.",
                    Data = response,
                    Code = (int)ErrorCodeAPI.OK
                };
            }
            catch (Exception ex)
            {
                return new ResponseData<PaginatedResponseDto<ThirdPartyHireResponseDto>>
                {
                    Success = false,
                    Message = $"An error occurred while retrieving third-party data: {ex.Message}",
                    Code = (int)ErrorCodeAPI.SystemIsError
                };
            }
        }

        public async Task<ResponseData<AddThirdPartyContractHireResponseDto>> AddThirdPartyContractHireAsync(AddThirdPartyContractHireRequestDto request)
        {
            try
            {
                // Validate the ThirdPartyId
                var thirdParty = await _unitOfWork.ThirdPartyRepository.GetQuery(x => x.Id == request.ThirdPartyId && x.IsDeleted == false).FirstOrDefaultAsync();
                if (thirdParty == null)
                {
                    return new ResponseData<AddThirdPartyContractHireResponseDto>
                    {
                        Success = false,
                        Message = "Third party not found.",
                        Code = (int)ErrorCodeAPI.NotFound
                    };
                }

                // Validate start and end dates
                if (request.StartDate >= request.EndDate)
                {
                    return new ResponseData<AddThirdPartyContractHireResponseDto>
                    {
                        Success = false,
                        Message = "End date must be after start date.",
                        Code = (int)ErrorCodeAPI.BadRequest
                    };
                }

                // Create a new ThirdPartyContact entity
                var newContract = new ThirdPartyContact
                {
                    Id = Guid.NewGuid(),
                    ThirdPartyId = request.ThirdPartyId,
                    NameService = request.NameService,
                    StartDate = request.StartDate,
                    EndDate = request.EndDate,
                    Price = request.Price,
                    IsDeleted = false,
                    InsertedById = Guid.NewGuid(), // Replace with the actual user ID if available
                    InsertedAt = DateTime.Now,
                    IsActive = true
                };

                // Insert the new contract
                _unitOfWork.ThirdPartyContractRepository.Add(newContract);
                await _unitOfWork.SaveChangesAsync();

                return new ResponseData<AddThirdPartyContractHireResponseDto>
                {
                    Success = true,
                    Message = "Third-party contract added successfully.",
                    Data = new AddThirdPartyContractHireResponseDto
                    {
                        ContractId = newContract.Id,
                        Message = "Contract created successfully."
                    },
                    Code = (int)ErrorCodeAPI.OK
                };
            }
            catch (Exception ex)
            {
                return new ResponseData<AddThirdPartyContractHireResponseDto>
                {
                    Success = false,
                    Message = $"An error occurred: {ex.Message}",
                    Code = (int)ErrorCodeAPI.SystemIsError
                };
            }
        }
    }


}
