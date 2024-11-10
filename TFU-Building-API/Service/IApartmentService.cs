using Core.Model;
using TFU_Building_API.Dto;

namespace TFU_Building_API.Service
{
    public interface IApartmentService
    {
        Task<ResponseData<List<ApartmentResponseDto>>> GetApartmentsByResidentIdAsync(Guid residentId);
        Task<ResponseData<List<ApartmentMemberDetailDto>>> GetApartmentDetailsByApartmentIdAsync(Guid apartmentId);
        Task<ResponseData<AddApartmentMemberResponseDto>> AddApartmentMemberAsync(AddApartmentMemberDto request);

        Task<ResponseData<List<ApartmentDto>>> GetApartmentsByBuildingIdAsync(Guid buildingId);

    }
}

