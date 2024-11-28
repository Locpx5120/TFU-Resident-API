using BuildingModels;
using Core.Enums;
using Core.Model;
using fake_tool.Helpers;
using Microsoft.EntityFrameworkCore;
using TFU_Building_API.Core.Handler;
using TFU_Building_API.Core.Helper;
using TFU_Building_API.Core.Infrastructure;
using TFU_Building_API.Dto;

namespace TFU_Building_API.Service.impl
{
    public class StaffService : BaseHandler, IStaffService
    {
        private readonly IConfiguration _config;
        private readonly IUnitOfWork _unitOfWork;
        private readonly EmailService emailService;

        public StaffService(IUnitOfWork UnitOfWork,
            IHttpContextAccessor HttpContextAccessor,
             IConfiguration config)
            : base(UnitOfWork, HttpContextAccessor)
        {
            _config = config;
            _unitOfWork = UnitOfWork;
            emailService = new EmailService(_config);
        }

        public async Task<ResponseData<StaffResponseDto>> AddStaff(StaffRequestDto request)
        {
            try
            {
                // Tìm kiếm user dựa trên email (nếu không tìm thấy thì báo lỗi)
                var existingUser = await _unitOfWork.StaffRepository.GetQuery(x => x.Email == request.Email && x.IsDeleted == false).FirstOrDefaultAsync();

                if (existingUser == null)
                {
                    // Nếu không tìm thấy user, trả về thông báo lỗi
                    return new ResponseData<StaffResponseDto>
                    {
                        Success = false,
                        Message = "User with this email does not exist.",
                        Code = (int)ErrorCodeAPI.UserNotFound
                    };
                }

                // Tạo mới staff và liên kết với user đã tìm thấy qua email
                Staff newStaff = new Staff
                {
                    //Id = Guid.NewGuid(),              
                    HireDate = DateTime.Now,
                    //UserId = existingUser.Id,          
                    RoleId = request.RoleId,
                    InsertedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    IsDeleted = false,
                    IsActive = true,
                    IsChangePassword = true,
                    Password = Utill.GeneratePassword()
                };

                await emailService.SendEmailAsync(newStaff.Email, "TB Dki tai khoan", BodyMaillRegister(newStaff));

                // Thêm staff mới vào cơ sở dữ liệu
                UnitOfWork.StaffRepository.Add(newStaff);
                await UnitOfWork.SaveChangesAsync();    // Lưu thay đổi vào cơ sở dữ liệu

                // Trả về kết quả thành công với thông tin StaffResponseDto
                var response = new StaffResponseDto
                {
                    Id = newStaff.Id
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
                    Message = ex.Message,  // Hoặc có thể log lỗi chi tiết và trả về thông báo chung
                    Code = (int)ErrorCodeAPI.SystemIsError
                };
            }
        }


        private string BodyMaillRegister(Staff customer)
        {
            string emailBody = $@"
<!DOCTYPE html>
<html lang='en'>
<head>
    <meta charset='UTF-8'>
    <meta http-equiv='X-UA-Compatible' content='IE=edge'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <style>
        body {{
            font-family: Arial, sans-serif;
            background-color: #f4f4f4;
            margin: 0;
            padding: 0;
        }}
        .container {{
            background-color: #ffffff;
            width: 100%;
            max-width: 600px;
            margin: 20px auto;
            padding: 20px;
            border-radius: 10px;
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
        }}
        .header {{
            background-color: #4CAF50;
            padding: 10px;
            text-align: center;
            border-radius: 10px 10px 0 0;
            color: white;
        }}
        .content {{
            margin: 20px 0;
            text-align: left;
        }}
        .content p {{
            font-size: 16px;
            color: #333333;
        }}
        .content .highlight {{
            color: #4CAF50;
            font-weight: bold;
        }}
        .footer {{
            text-align: center;
            margin-top: 20px;
            font-size: 12px;
            color: #aaaaaa;
        }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>Welcome to Our Service!</h1>
        </div>
        <div class='content'>
            <p>Dear <span class='highlight'>{customer.FullName}</span>,</p>
            <p>Thank you for registering with our service. Below are your account details:</p>
            <p><strong>Username:</strong> <span class='highlight'>{customer.Email}</span></p>
            <p><strong>Password:</strong> <span class='highlight'>{customer.Password}</span></p>
            <p>Please keep this information safe and do not share it with anyone. You can log in to your account using the credentials provided.</p>
            <p>If you have any questions, feel free to contact us at any time.</p>
            <p>Best regards,<br>The Support Team</p>
        </div>
        <div class='footer'>
            <p>&copy; 2024 Our Company. All rights reserved.</p>
        </div>
    </div>
</body>
</html>";

            return emailBody;
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
                        RoleName = x.Role.Name,
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
