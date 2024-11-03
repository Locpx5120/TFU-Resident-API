using BuildingModels;
using Core.Enums;
using Core.Model;
using Microsoft.EntityFrameworkCore;
using TFU_Building_API.Core.Handler;
using TFU_Building_API.Core.Infrastructure;
using TFU_Building_API.Dto;

namespace TFU_Building_API.Service.impl
{
    public class StaffService : BaseHandler, IStaffService
    {
        private readonly IUnitOfWork _unitOfWork;
        public StaffService(IUnitOfWork UnitOfWork, IHttpContextAccessor HttpContextAccessor) : base(UnitOfWork, HttpContextAccessor)
        {
            _unitOfWork = UnitOfWork;
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
                    IsActive = true                   
                };

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


    }
}
