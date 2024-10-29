using Core.Model;
using TFU_Building_API.Dto;

namespace TFU_Building_API.Service
{
    public interface IService
    {
        Task<ResponseData<List<ServiceDto>>> GetServices();
    }
}
