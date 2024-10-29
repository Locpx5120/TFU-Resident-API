using Core.Enums;
using Core.Model;
using Microsoft.EntityFrameworkCore;
using TFU_Building_API.Core.Handler;
using TFU_Building_API.Core.Infrastructure;
using TFU_Building_API.Dto;

namespace TFU_Building_API.Service.impl
{
    public class ServiceImplement : BaseHandler, IService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ServiceImplement(IUnitOfWork UnitOfWork, IHttpContextAccessor HttpContextAccessor) : base(UnitOfWork, HttpContextAccessor)
        {
            _unitOfWork = UnitOfWork;
        }

        public async Task<ResponseData<List<ServiceDto>>> GetServices()
        {
            try
            {
                var services = await _unitOfWork.ServiceRepository
                    .GetQuery(x => x.IsDeleted == false && x.IsActive == true)
                    .Select(s => new ServiceDto
                    {
                        Id = s.Id,
                        ServiceName = s.ServiceName
                    })
                    .ToListAsync();

                return new ResponseData<List<ServiceDto>>
                {
                    Success = true,
                    Message = "Successfully retrieved services.",
                    Data = services,
                    Code = (int)ErrorCodeAPI.OK
                };
            }
            catch (Exception ex)
            {
                return new ResponseData<List<ServiceDto>>
                {
                    Success = false,
                    Message = ex.Message,
                    Code = (int)ErrorCodeAPI.SystemIsError
                };
            }
        }

    }
}
