using Constant;
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
        private readonly IUserIdentity _userIdentity;

        public ServiceImplement(IUnitOfWork UnitOfWork, IHttpContextAccessor HttpContextAccessor,
             IUserIdentity userIdentity) : base(UnitOfWork, HttpContextAccessor)
        {
            _unitOfWork = UnitOfWork;
            _userIdentity = userIdentity;
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
                        ServiceName = s.ServiceName,
                        UnitPrice = s.UnitPrice,
                        Unit = s.Unit,

                    })
                    .ToListAsync();

                if (_userIdentity.RoleName.Equals(Constants.ROLE_Resident))
                {
                    if (services.Any())
                    {
                        services = services.ToList().Where(x =>
                        x.Id != Guid.Parse("f517bef7-d325-487b-9f76-e66d20413634") // bỏ gia hạn hợp đồng cho cư dân
                        ).ToList();
                    }
                }

                return new ResponseData<List<ServiceDto>>
                {
                    Success = true,
                    Message = MessConstant.UpdateSuccessfully,
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

        public async Task<ResponseData<List<ServiceResponseDto>>> GetServicesByCategoryIdAsync(Guid serviceCategoryId)
        {
            try
            {
                var services = await _unitOfWork.ServiceRepository
                    .GetQuery(s => s.ServiceCategoryID == serviceCategoryId && s.IsDeleted == false && s.IsActive)
                    .Select(s => new ServiceResponseDto
                    {
                        Id = s.Id,
                        ServiceName = s.ServiceName,
                        Description = s.Description,
                        UnitPrice = s.UnitPrice,
                        Unit = s.Unit,
                        IsPackageAllowed = s.IsPackageAllowed
                    })
                    .ToListAsync();

                return new ResponseData<List<ServiceResponseDto>>
                {
                    Success = true,
                    Message = MessConstant.UpdateSuccessfully,
                    Data = services,
                    Code = (int)ErrorCodeAPI.OK
                };
            }
            catch (Exception ex)
            {
                return new ResponseData<List<ServiceResponseDto>>
                {
                    Success = false,
                    Message = ex.Message,
                    Code = (int)ErrorCodeAPI.SystemIsError
                };
            }
        }

    }
}
