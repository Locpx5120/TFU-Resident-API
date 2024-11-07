
using Core.Model;
using TFU_Building_API.Dto;

namespace TFU_Building_API.Service
{
    public interface IApartmentType
    {
        Task<ResponseData<List<ApartmentResponseTypeDto>>> GetApartmentType();
        Task<ResponseData<ApartmentResponseTypeDto>> AddApartmentType(ApartmentTypeDto apartmentType);
    }
}
