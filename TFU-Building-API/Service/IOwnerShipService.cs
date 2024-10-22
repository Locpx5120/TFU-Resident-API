using Core.Model;
using TFU_Building_API.Dto;

namespace TFU_Building_API.Service
{
    public interface IOwnerShipService
    {
        Task<ResponseData<OwnerShipResponseDto>> AddOwnerShip(OwnerShipRequestDto request);
        Task<ResponseData<OwnerShipResponseDto>> UpdateOwnerShip(OwnerShipUpdateRequestDto request);
        Task<ResponseData<OwnerShipResponseDto>> DeleteOwnerShip(Guid ownerShipId);
        Task<ResponseData<PaginatedResponseDto<OwnerShipListResponseDto>>> GetOwnerShips(OwnerShipSearchRequestDto request);
    }

}
