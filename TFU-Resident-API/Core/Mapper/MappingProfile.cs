using AutoMapper;
using TFU_Resident_API.Dto;

namespace Core.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //this.CreateMap<SuperOwnerModels.Investor, InvestorDto>();
            this.CreateMap<CreateInvestorDto, SuperOwnerModels.Investor>();
            //this.CreateMap<SuperOwnerModels.Project, ProjectDto>();
            this.CreateMap<CreateProjectDto, SuperOwnerModels.Project>();
            //this.CreateMap<SuperOwnerModels.Building, BuildingDto>();
            this.CreateMap<CreateBuildingDto, SuperOwnerModels.Building>();
        }
    }
}
