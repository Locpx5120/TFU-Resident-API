using AutoMapper;
using BuildingModels;
using TFU_Building_API.Model;

namespace TFU_Building_API.Core.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //this.CreateMap<SuperOwnerModels.Investor, InvestorDto>();
            this.CreateMap<User, UserInfoResponse>();

        }
    }
}
