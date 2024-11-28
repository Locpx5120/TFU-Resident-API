using BuildingModels;
using Core.Enums;
using Core.Model;
using Microsoft.EntityFrameworkCore;
using TFU_Building_API.Core.Handler;
using TFU_Building_API.Core.Helper;
using TFU_Building_API.Core.Infrastructure;
using TFU_Building_API.Dto;

namespace TFU_Building_API.Service.impl
{
    public class StaffService : BaseHandler, IStaffService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;
        public StaffService(IUnitOfWork UnitOfWork, IHttpContextAccessor HttpContextAccessor, IConfiguration config) : base(UnitOfWork, HttpContextAccessor)
        {
            _unitOfWork = UnitOfWork;
            _config = config;
        }

        public async Task<ResponseData<StaffResponseDto>> AddStaff(StaffRequestDto request)
        {
            try
            {
                // Kiểm tra xem nhân viên với email này đã tồn tại hay chưa
                var existingStaff = await _unitOfWork.StaffRepository
                    .GetQuery(x => x.Email == request.Email && x.IsDeleted == false)
                    .FirstOrDefaultAsync();

                if (existingStaff != null)
                {
                    // Nếu nhân viên đã tồn tại, trả về thông báo lỗi
                    return new ResponseData<StaffResponseDto>
                    {
                        Success = false,
                        Message = "A staff with this email already exists.",
                        Code = (int)ErrorCodeAPI.DuplicateEntry
                    };
                }

                // Tạo mật khẩu ngẫu nhiên cho nhân viên mới
                var generatedPassword = GenerateRandomPassword();

                // Tạo mới staff và thêm thông tin tài khoản
                Staff newStaff = new Staff
                {
                    Id = Guid.NewGuid(),
                    HireDate = DateTime.Now,
                    FullName = request.Email.Split('@')[0],
                    Email = request.Email,
                    Password = generatedPassword, // Lưu mật khẩu dạng plain text
                    //PhoneNumber = request.PhoneNumber,
                    RoleId = request.RoleId,
                    InsertedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    IsDeleted = false,
                    IsActive = true,
                    IsChangePassword = false // Đánh dấu là người dùng cần đổi mật khẩu sau lần đăng nhập đầu tiên
                };

                // Thêm staff mới vào cơ sở dữ liệu
                _unitOfWork.StaffRepository.Add(newStaff);
                await _unitOfWork.SaveChangesAsync(); // Lưu thay đổi vào cơ sở dữ liệu

                // Gửi email thông tin tài khoản và mật khẩu cho nhân viên
                var emailService = new EmailService(_config);
                string subject = "Your New Account Details";
                string body = $@"
            <p>Dear {newStaff.FullName},</p>
            <p>Your new account has been created successfully. Below are your login details:</p>
            <p><b>Email:</b> {newStaff.Email}</p>
            <p><b>Password:</b> {generatedPassword}</p>
            <p>Please log in and change your password as soon as possible.</p>
            <br/>
            <p>Best Regards,<br/>TFU Building Management Team</p>";

                await emailService.SendEmailAsync(newStaff.Email, subject, body);

                // Trả về kết quả thành công với thông tin StaffResponseDto
                var response = new StaffResponseDto
                {
                    Id = newStaff.Id,
                    Email = newStaff.Email,
                    Password = generatedPassword // Trả về mật khẩu cho người dùng (chỉ trả về trong response này)
                };

                return new ResponseData<StaffResponseDto>
                {
                    Success = true,
                    Message = "Staff created successfully.",
                    Data = response,
                    Code = (int)ErrorCodeAPI.OK
                };
            }
            catch (Exception ex)
            {
                // Xử lý lỗi và trả về thông báo lỗi chi tiết
                return new ResponseData<StaffResponseDto>
                {
                    Success = false,
                    Message = ex.Message, // Hoặc có thể log lỗi chi tiết và trả về thông báo chung
                    Code = (int)ErrorCodeAPI.SystemIsError
                };
            }
        }

        // Hàm tạo mật khẩu ngẫu nhiên
        private string GenerateRandomPassword()
        {
            // Bạn có thể tùy chỉnh độ dài và kiểu ký tự của mật khẩu tại đây
            const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*";
            var random = new Random();
            return new string(Enumerable.Repeat(validChars, 12)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }


        public async Task<ResponseData<StaffResponseDto>> DeleteStaff(StaffDeleteRequestDto request)
        {
            try
            {
                // Tìm kiếm nhân viên dựa trên StaffId
                var staff = await _unitOfWork.StaffRepository.GetQuery(x => x.Id == request.StaffId && x.IsDeleted == false).FirstOrDefaultAsync();

                // Nếu không tìm thấy nhân viên, trả về thông báo lỗi
                if (staff == null)
                {
                    return new ResponseData<StaffResponseDto>
                    {
                        Success = false,
                        Message = "Staff not found.",
                        Code = (int)ErrorCodeAPI.UserNotFound
                    };
                }

                // Đánh dấu nhân viên là đã bị xóa (soft delete)
                staff.IsDeleted = true;
                staff.UpdatedAt = DateTime.Now;

                // Lưu thay đổi vào cơ sở dữ liệu
                UnitOfWork.StaffRepository.Update(staff);
                await UnitOfWork.SaveChangesAsync();

                return new ResponseData<StaffResponseDto>
                {
                    Success = true,
                    Message = "Staff deleted successfully.",
                    Code = (int)ErrorCodeAPI.OK
                };
            }
            catch (Exception ex)
            {
                // Xử lý lỗi và trả về thông báo lỗi
                return new ResponseData<StaffResponseDto>
                {
                    Success = false,
                    Message = ex.Message,
                    Code = (int)ErrorCodeAPI.SystemIsError
                };
            }
        }

        public async Task<ResponseData<StaffResponseDto>> UpdateStaff(StaffUpdateRequestDto request)
        {
            try
            {
                // Tìm kiếm nhân viên dựa trên StaffId
                var staff = await _unitOfWork.StaffRepository.GetQuery(x => x.Id == request.StaffId && x.IsDeleted == false).FirstOrDefaultAsync();

                // Nếu không tìm thấy nhân viên, trả về thông báo lỗi
                if (staff == null)
                {
                    return new ResponseData<StaffResponseDto>
                    {
                        Success = false,
                        Message = "Staff not found.",
                        Code = (int)ErrorCodeAPI.UserNotFound
                    };
                }

                // Cập nhật RoleId của nhân viên
                staff.RoleId = request.RoleId;
                staff.UpdatedAt = DateTime.Now;  // Cập nhật thời gian chỉnh sửa

                // Lưu thay đổi vào cơ sở dữ liệu
                UnitOfWork.StaffRepository.Update(staff);
                await UnitOfWork.SaveChangesAsync();

                return new ResponseData<StaffResponseDto>
                {
                    Success = true,
                    Message = "Role updated successfully.",
                    Code = (int)ErrorCodeAPI.OK
                };
            }
            catch (Exception ex)
            {
                // Xử lý lỗi và trả về thông báo lỗi chi tiết
                return new ResponseData<StaffResponseDto>
                {
                    Success = false,
                    Message = ex.Message,
                    Code = (int)ErrorCodeAPI.SystemIsError
                };
            }
        }

        //public async Task<ResponseData<List<StaffListResponseDto>>> GetStaffList(StaffSearchRequestDto request)
        //{
        //    try
        //    {
        //        // Lấy danh sách nhân viên và join với bảng User và Role để lấy thông tin cần thiết
        //        var staffQuery = from staff in UnitOfWork.StaffRepository.GetQuery(x => x.IsDeleted == false)
        //                         join user in UnitOfWork.UserRepository.GetQuery(u => u.IsDeleted == false)
        //                            on staff.UserId equals user.Id
        //                         join role in UnitOfWork.RoleRepository.GetQuery(r => r.IsDeleted == false)
        //                            on staff.RoleId equals role.Id
        //                         select new StaffListResponseDto
        //                         {
        //                             FullName = user.FullName,
        //                             Department = role.Name,
        //                             Email = user.Email,
        //                             Phone = user.PhoneNumber,
        //                             HireDate = staff.HireDate.GetValueOrDefault(),
        //                             Id = staff.Id
        //                         };

        //        // Thêm điều kiện tìm kiếm nếu có
        //        if (!string.IsNullOrEmpty(request.Name))
        //        {
        //            staffQuery = staffQuery.Where(x => x.FullName.Contains(request.Name)); // Tìm kiếm theo tên chứa chuỗi 'SearchTerm'
        //        }

        //        // Tính tổng số bản ghi trước khi phân trang
        //        var totalRecords = await staffQuery.CountAsync();

        //        // Áp dụng phân trang
        //        var staffList = await staffQuery
        //            .Skip((request.PageNumber - 1) * request.PageSize)
        //            .Take(request.PageSize)
        //            .ToListAsync();


        //        // Gán giá trị TotalRecords cho mỗi StaffListResponseDto
        //        foreach (var staff in staffList)
        //        {
        //            staff.TotalRecords = totalRecords; // Gán tổng số bản ghi cho mỗi item trong danh sách
        //        }

        //        return new ResponseData<List<StaffListResponseDto>>
        //        {
        //            Success = true,
        //            Message = "Staff list retrieved successfully",
        //            Data = staffList,
        //            Code = (int)ErrorCodeAPI.OK
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        return new ResponseData<List<StaffListResponseDto>>
        //        {
        //            Success = false,
        //            Message = ex.Message,
        //            Code = (int)ErrorCodeAPI.SystemIsError
        //        };
        //    }
        //}

        //public async Task<ResponseData<StaffInfoResponseDto>> GetStaffById(Guid staffId)
        //{
        //    try
        //    {
        //        // Lấy thông tin nhân viên dựa trên Id
        //        var staff = await _unitOfWork.StaffRepository.GetQuery(x => x.Id == staffId && x.IsDeleted == false)
        //            .FirstOrDefaultAsync();

        //        if (staff == null)
        //        {
        //            return new ResponseData<StaffInfoResponseDto>
        //            {
        //                Success = false,
        //                Message = "Staff not found.",
        //                Code = (int)ErrorCodeAPI.NotFound
        //            };
        //        }

        //        // Lấy thông tin từ bảng User dựa trên UserId của nhân viên
        //        var user = await _unitOfWork.UserRepository.GetQuery(u => u.Id == staff.UserId && u.IsDeleted == false)
        //            .FirstOrDefaultAsync();

        //        if (user == null)
        //        {
        //            return new ResponseData<StaffInfoResponseDto>
        //            {
        //                Success = false,
        //                Message = "User not found.",
        //                Code = (int)ErrorCodeAPI.UserNotFound
        //            };
        //        }

        //        // Lấy thông tin từ bảng Role (bộ phận) dựa trên RoleId của nhân viên
        //        var role = await _unitOfWork.RoleRepository.GetQuery(r => r.Id == staff.RoleId && r.IsDeleted == false)
        //            .FirstOrDefaultAsync();

        //        if (role == null)
        //        {
        //            return new ResponseData<StaffInfoResponseDto>
        //            {
        //                Success = false,
        //                Message = "Role not found.",
        //                Code = (int)ErrorCodeAPI.RoleNotFound
        //            };
        //        }

        //        // Tạo đối tượng response với các trường cần thiết
        //        var response = new StaffInfoResponseDto
        //        {
        //            Id = staff.Id,
        //            Email = user.Email,
        //            Department = role.Name
        //        };

        //        // Trả về kết quả thành công
        //        return new ResponseData<StaffInfoResponseDto>
        //        {
        //            Success = true,
        //            Message = "Staff found successfully.",
        //            Data = response,
        //            Code = (int)ErrorCodeAPI.OK
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        // Xử lý lỗi và trả về thông báo lỗi chi tiết
        //        return new ResponseData<StaffInfoResponseDto>
        //        {
        //            Success = false,
        //            Message = ex.Message,
        //            Code = (int)ErrorCodeAPI.SystemIsError
        //        };
        //    }
        //}

        public async Task<ResponseData<List<GetStaffResponseDto>>> GetStaffListAsync(string searchName)
        {
            try
            {
                // Query để lấy danh sách Staff không bị xóa
                var query = _unitOfWork.StaffRepository.GetQuery(x => x.IsDeleted == false);

                // Lọc theo tên nếu có
                if (!string.IsNullOrEmpty(searchName))
                {
                    query = query.Where(x => x.FullName.Contains(searchName));
                }

                // Lấy danh sách Staff
                var data = await query
                    .OrderBy(x => x.FullName) // Sắp xếp theo tên
                    .Select(x => new GetStaffResponseDto
                    {
                        Id = x.Id,
                        FullName = x.FullName,
                        Email = x.Email,
                        PhoneNumber = x.PhoneNumber,
                        HireDate = x.HireDate,
                        Birthday = x.Birthday,
                        IsActive = x.IsActive,
                        RoleId = x.RoleId,
                        InsertedAt = x.InsertedAt,
                        UpdatedAt = x.UpdatedAt
                    })
                    .ToListAsync();

                return new ResponseData<List<GetStaffResponseDto>>
                {
                    Success = true,
                    Message = "Successfully retrieved staff.",
                    Data = data,
                    Code = (int)ErrorCodeAPI.OK
                };
            }
            catch (Exception ex)
            {
                return new ResponseData<List<GetStaffResponseDto>>
                {
                    Success = false,
                    Message = $"An error occurred: {ex.Message}",
                    Code = (int)ErrorCodeAPI.SystemIsError
                };
            }
        }

    }
}
