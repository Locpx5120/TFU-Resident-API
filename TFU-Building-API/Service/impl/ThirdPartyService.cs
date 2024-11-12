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


        public async Task<ResponseData<List<ThirdPartyListResponseDto>>> GetThirdPartyListAsync(ThirdPartyListRequestDto request)
        {
            try
            {
                var query = from tp in _unitOfWork.ThirdPartyRepository.GetQuery(x => x.IsDeleted == false && x.IsTenant == true)
                            join tpc in _unitOfWork.ThirdPartyContractRepository.GetQuery(x => x.IsDeleted == false) on tp.Id equals tpc.ThirdPartyId
                            join a in _unitOfWork.ApartmentRepository.GetQuery(x => x.IsDeleted == false) on tpc.ApartmentId equals a.Id into apartmentJoin
                            from aj in apartmentJoin.DefaultIfEmpty() // Allow null for ApartmentId
                            join b in _unitOfWork.BuildingRepository.GetQuery(x => x.IsDeleted == false) on aj.BuildingId equals b.Id into buildingJoin
                            from bj in buildingJoin.DefaultIfEmpty() // Allow null for BuildingId
                            join s in _unitOfWork.StaffRepository.GetQuery(x => x.IsDeleted == false) on tp.StaffId equals s.Id
                            select new
                            {
                                tp.Id,
                                tp.NameCompany,
                                ContactInfo = new { s.Email, s.PhoneNumber }, // Thông tin liên hệ từ bảng Staff
                                tp.Description,
                                tpc.StartDate,
                                tpc.EndDate,
                                BuildingName = bj.Name // Tên tòa nhà
                            };

                // Tìm kiếm theo tên công ty
                if (!string.IsNullOrEmpty(request.CompanyName))
                {
                    query = query.Where(x => x.NameCompany.Contains(request.CompanyName));
                }

                // Lọc theo trạng thái
                DateTime oneMonthFromNow = DateTime.Now.AddMonths(1);
                if (request.Status == "Trong thời hạn")
                {
                    query = query.Where(x => x.EndDate > DateTime.Now);
                }
                else if (request.Status == "Chuẩn bị hết hạn")
                {
                    query = query.Where(x => x.EndDate <= oneMonthFromNow && x.EndDate > DateTime.Now);
                }

                // Lấy dữ liệu và chuyển đổi thành danh sách response
                var result = await query.Select(item => new ThirdPartyListResponseDto
                {
                    ThirdPartyId = item.Id,
                    CompanyName = item.NameCompany,
                    ContactInfo = $"{item.ContactInfo.Email}, {item.ContactInfo.PhoneNumber}", // Định dạng thông tin liên hệ
                    StoreType = item.Description,
                    StartDate = item.StartDate,
                    EndDate = item.EndDate,
                    BuildingName = item.BuildingName
                }).ToListAsync();

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
                return new ResponseData<List<ThirdPartyListResponseDto>>
                {
                    Success = false,
                    Message = ex.Message,
                    Code = (int)ErrorCodeAPI.SystemIsError
                };
            }
        }

    }

}
