using Core.Model;
using TFU_Resident_API.Dto;

namespace TFU_Resident_API.Services
{
    public interface IProjectService
    {
        public Task<ResponseData<object>> CreateProject(CreateProjectDto request);
        public Task<ResponseData<object>> Update(UpdateProjectRequest request);
        public Task<ResponseData<object>> Delete(DeleteProjectRequest request);
        public Task<ResponseData<List<ViewManagerProjectResponse>>> ViewManager(ViewManagerProjectRequest request);
    }
}
