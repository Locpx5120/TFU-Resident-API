using BuildingModels;
using Core.Enums;
using Core.Model;
using Microsoft.EntityFrameworkCore;
using TFU_Building_API.Core.Handler;
using TFU_Building_API.Core.Infrastructure;
using TFU_Building_API.Dto;
using static fake_tool.Helpers.EnumVariable;

namespace TFU_Building_API.Service.impl
{
    public class ApartmentTypeService : BaseHandler,IApartmentType
    {
        private readonly IUnitOfWork _unitOfWork;
        public ApartmentTypeService(IUnitOfWork UnitOfWork, IHttpContextAccessor HttpContextAccessor) : base(UnitOfWork, HttpContextAccessor)
        {
            _unitOfWork = UnitOfWork;
        }

        //Tạo các loại căn hộ
        public async Task<ResponseData<ApartmentResponseTypeDto>> AddApartmentType(ApartmentTypeDto apartmentType)
        {
            var existingApartmentType = await _unitOfWork.ApartmentTypeRepository.GetQuery(x => x.Name.Equals(apartmentType.Name) || x.LandArea == apartmentType.LandArea ).FirstOrDefaultAsync();
            if (existingApartmentType != null) 
            {
                return new ResponseData<ApartmentResponseTypeDto>
                {
                    Success = true,
                    Message = "ApartmentType already exists.",
                    Code = (int)ErrorCodeAPI.OK
                };
            }

                var newApartmentType = new ApartmentType()
                {
                    Name = apartmentType.Name,
                    LandArea = apartmentType.LandArea,
                    IsDeleted = false,
                    IsActive = true,
                    InsertedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };
            _unitOfWork.ApartmentTypeRepository.Add(newApartmentType);
            _unitOfWork.SaveChangesAsync();

            var response = new ApartmentResponseTypeDto()
            {
               Name = newApartmentType.Name,
               LandArea = newApartmentType.LandArea
            };

            return new ResponseData<ApartmentResponseTypeDto>
            {
                Success = true,
                Message = "Add ApartmentType successfully.",
                Data = response,
                Code = (int)ErrorCodeAPI.OK,
            };
        }
        public async Task<ResponseData<List<ApartmentResponseTypeDto>>> GetApartmentType()
        {
            try
            {
                var apartmentTypes = await _unitOfWork.ApartmentTypeRepository.GetQuery(x => x.IsDeleted == false)
                .Select(apartmentType => new ApartmentResponseTypeDto
                {
                    Name = apartmentType.Name,
                    LandArea = apartmentType.LandArea
                }).ToListAsync();
                return new ResponseData<List<ApartmentResponseTypeDto>>
                {
                    Success = true,
                    Message = "ApartmentTypes retrived successfully",
                    Data = apartmentTypes,
                    Code = (int)ErrorCodeAPI.OK
                };
            }catch (Exception ex)
            {
                // Xử lý lỗi nếu có
                return new ResponseData<List<ApartmentResponseTypeDto>>
                {
                    Success = false,
                    Message = ex.Message,
                    Code = (int)ErrorCodeAPI.OK
                };
            }
        }

        
    }
}
