using Core.Enums;
using Core.Model;
using Microsoft.EntityFrameworkCore;
using TFU_Building_API.Core.Handler;
using TFU_Building_API.Core.Infrastructure;
using TFU_Building_API.Dto;

namespace TFU_Building_API.Service.impl
{
    public class PackageService : BaseHandler, IPackageService
    {
        private readonly IUnitOfWork _unitOfWork;
        public PackageService(IUnitOfWork UnitOfWork, IHttpContextAccessor HttpContextAccessor) : base(UnitOfWork, HttpContextAccessor)
        {
            _unitOfWork = UnitOfWork;
        }

        public async Task<ResponseData<List<PackageServiceBasicDto>>> GetPackageServices()
        {
            try
            {
                var packageServices = await _unitOfWork.PackageServiceRepository
                    .GetQuery(x => x.IsDeleted == false && x.IsActive == true)
                    .Select(ps => new PackageServiceBasicDto
                    {
                        Id = ps.Id,
                        Name = ps.Name,
                        Discount = ps.Discount,
                        DurationInMonth = ps.DurationInMonth
                    })
                    .ToListAsync();

                return new ResponseData<List<PackageServiceBasicDto>>
                {
                    Success = true,
                    Message = "Successfully retrieved package services.",
                    Data = packageServices,
                    Code = (int)ErrorCodeAPI.OK
                };
            }
            catch (Exception ex)
            {
                return new ResponseData<List<PackageServiceBasicDto>>
                {
                    Success = false,
                    Message = ex.Message,
                    Code = (int)ErrorCodeAPI.SystemIsError
                };
            }
        }

    }
}
