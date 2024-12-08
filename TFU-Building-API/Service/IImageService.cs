using Core.Model;
using TFU_Building_API.Dto;

namespace TFU_Building_API.Service
{
    public interface IImageService
    {
        Task<ResponseData<ImageDto>> Get(Guid id);
    }
}
