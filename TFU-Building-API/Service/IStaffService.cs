using Core.Model;
using TFU_Building_API.Dto;

namespace TFU_Building_API.Service
{
    public interface IStaffService
    {
        public Task<ResponseData<StaffResponseDto>> AddStaff(StaffRequestDto request);
        Task<ResponseData<StaffResponseDto>> DeleteStaff(StaffDeleteRequestDto request);
        Task<ResponseData<StaffResponseDto>> UpdateStaff(StaffUpdateRequestDto request);
        //Task<ResponseData<List<StaffListResponseDto>>> GetStaffList(StaffSearchRequestDto request);
        //Task<ResponseData<StaffInfoResponseDto>> GetStaffById(Guid staffId);

        Task<ResponseData<List<GetStaffResponseDto>>> GetStaffListAsync(string searchName);
        Task<ResponseData<List<GetStaffAssigmentResponseDto>>> GetStaffListAssigment(string searchName);
    }
}
