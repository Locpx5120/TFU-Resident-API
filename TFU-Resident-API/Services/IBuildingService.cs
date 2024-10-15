using Core.Model;
using TFU_Resident_API.Dto;

namespace TFU_Resident_API.Services
{
    public interface IBuildingService
    {
        public Task<ResponseData<object>> Create(CreateBuildingDto request);
    }
}
