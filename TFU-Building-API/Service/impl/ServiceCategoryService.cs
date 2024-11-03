using BuildingModels;
using Core.Enums;
using Core.Model;
using Microsoft.EntityFrameworkCore;
using TFU_Building_API.Core.Handler;
using TFU_Building_API.Core.Infrastructure;
using TFU_Building_API.Dto;

namespace TFU_Building_API.Service.impl
{
    public class ServiceCategoryService : BaseHandler, IServiceCategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ServiceCategoryService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseData<List<ServiceCategoryResponseDto>>> GetServiceCategoriesAsync()
        {
            try
            {
                var categories = await _unitOfWork.ServiceCategoryRepository
                    .GetQuery(sc => sc.IsDeleted == false && sc.IsActive)
                    .Select(sc => new ServiceCategoryResponseDto
                    {
                        Id = sc.Id,
                        Name = sc.Name,
                        Description = sc.Description
                    })
                    .ToListAsync();

                return new ResponseData<List<ServiceCategoryResponseDto>>
                {
                    Success = true,
                    Message = "Service categories retrieved successfully.",
                    Data = categories,
                    Code = (int)ErrorCodeAPI.OK
                };
            }
            catch (Exception ex)
            {
                return new ResponseData<List<ServiceCategoryResponseDto>>
                {
                    Success = false,
                    Message = ex.Message,
                    Code = (int)ErrorCodeAPI.SystemIsError
                };
            }
        }
    }
}
