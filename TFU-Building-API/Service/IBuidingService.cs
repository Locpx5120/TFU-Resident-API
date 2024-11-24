using Core.Model;
using TFU_Building_API.Dto;

namespace TFU_Building_API.Service
{
    public interface IBuildingService
    {
        Task<ResponseData<BuildingResponseDto>> AddBuilding(BuildingRequestDto request);
        Task<ResponseData<BuildingUpdateResponseDto>> UpdateBuilding(BuildingUpdateRequestDto request);

        Task<ResponseData<List<BuildingResponseDto>>> GetBuildingsAsync();
    }
}
