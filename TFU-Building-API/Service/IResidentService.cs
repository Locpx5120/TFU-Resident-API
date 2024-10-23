using Core.Model;
using TFU_Building_API.Dto;

namespace TFU_Building_API.Service
{
    public interface IResidentService
    {
        Task<ResponseData<ResidentResponseDto>> AddResident(ResidentRequestDto request);
        Task<ResponseData<ResidentUpdateRequestDto>> UpdateResident(ResidentUpdateRequestDto request);
        Task<ResponseData<PagedResidentListResponseDto>> GetResidentsByOwnershipId(ResidentSearchRequestDto request);
        Task<ResponseData<ResidentResponseDto>> DeleteResident(ResidentDeleteRequestDto request);
        Task<ResponseData<ResidentInfoResponseDto>> GetResidentById(Guid residentId);
    }
}
