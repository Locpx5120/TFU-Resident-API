using BuildingModels;
using Core.Enums;
using Core.Model;
using Microsoft.EntityFrameworkCore;
using TFU_Building_API.Core.Helper;
using TFU_Building_API.Core.Infrastructure;
using TFU_Building_API.Dto;

namespace TFU_Building_API.Service.impl
{
    public class NotifyCategoryService : INotifyCategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public NotifyCategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseData<List<NotifyCategoryDto>>> GetNotifyCategoriesAsync()
        {
            try
            {
                // Query the NotifyCategories where IsDeleted is false
                var categories = await _unitOfWork.NotifyCategoryRepository
                    .GetQuery(x => x.IsDeleted == false)
                    .Select(x => new NotifyCategoryDto
                    {
                        Id = x.Id,
                        Name = x.Name
                    })
                    .ToListAsync();

                return new ResponseData<List<NotifyCategoryDto>>
                {
                    Success = true,
                    Message = "Successfully retrieved notify categories.",
                    Data = categories,
                    Code = (int)ErrorCodeAPI.OK
                };
            }
            catch (Exception ex)
            {
                return new ResponseData<List<NotifyCategoryDto>>
                {
                    Success = false,
                    Message = ex.Message,
                    Code = (int)ErrorCodeAPI.SystemIsError
                };
            }
        }
    }
}
