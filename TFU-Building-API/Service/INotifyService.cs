using Core.Model;
using TFU_Building_API.Dto;

namespace TFU_Building_API.Service
{
    public interface INotifyService
    {
        Task<ResponseData<CreateNotifyResponseDto>> CreateNotifyAsync(CreateNotifyRequestDto request);

        Task<ResponseData<PaginatedResponseDto<NotifyResponseDto>>> GetNotifiesAsync(NotifyFilterRequestDto request);
        Task<ResponseData<NotifyDetailResponseDto>> GetNotifiesDetailAsync(Guid notifyId);
    }

}
