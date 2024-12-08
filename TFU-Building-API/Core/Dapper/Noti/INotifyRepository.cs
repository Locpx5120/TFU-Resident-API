using TFU_Building_API.Dto;

namespace TFU_Building_API.Core.Dapper.Noti
{
    public interface INotifyRepository
    {
        public Task<IEnumerable<NotifyResponseDto>> GetNotifies(NotifyFilterRequestDto request);
        public Task<IEnumerable<NotifyDetailResponseDto>> GetNotifiesDetails(Guid notifyId);
    }
}
