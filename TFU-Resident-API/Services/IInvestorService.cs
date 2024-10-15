using Core.Model;
using TFU_Resident_API.Dto;

namespace TFU_Resident_API.Services
{
    public interface IInvestorService
    {
        public Task<ResponseData<object>> CreateInvestor(CreateInvestorDto request);
        public Task<ResponseData<ViewManagerResponse>> ViewManager();
    }
}
