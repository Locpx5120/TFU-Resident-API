using Core.Model;
using TFU_Building_API.Dto;

namespace TFU_Building_API.Service
{
    public interface IThirdPartyService
    {
        Task<ResponseData<AddThirdPartyResponseDto>> AddThirdPartyAsync(AddThirdPartyRequestDto request);
        Task<ResponseData<AddThirdPartyContactResponseDto>> AddThirdPartyContactAsync(AddThirdPartyContactRequestDto request);

        Task<ResponseData<List<ThirdPartyListResponseDto>>> GetThirdPartyListAsync(ThirdPartyListRequestDto request);

        Task<ResponseData<ThirdPartyContractDetailDto>> GetThirdPartyContractDetailAsync(Guid thirdPartyId);

        Task<ResponseData<List<ThirdPartyContractInfoDto>>> GetContractDetailsForThirdPartyAsync(Guid staffId);

        Task<ResponseData<string>> AddThirdPartyHireAsync(AddThirdPartyHireRequestDto request);
    }

}
