using AutoMapper;
using Entity;
using SuperOwnerModels;
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
            this.CreateMap<UserCreateRequest, User>().ReverseMap();
            this.CreateMap<UserDeleteRequest, User>().ReverseMap();
            this.CreateMap<UserUpdateRequest, User>().ReverseMap();
            this.CreateMap<UserDto, User>()
                .ForMember(dest => dest.Gender,
                       opt => opt.MapFrom(src => src.Genders.Equals("Male", StringComparison.OrdinalIgnoreCase))) // Ánh xạ Gender
                .ReverseMap();
            this.CreateMap<ViewManagerUserRequest, User>().ReverseMap();
            this.CreateMap<ViewManagerBuildingResponse, Building>().ReverseMap();
        }
    }
}
