using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TFU_Resident_API.Dto;
using TFU_Resident_API.Services;

namespace TFU_Resident_API.Controllers
{
    [Route("api/project")]
    [ApiController]
    [Authorize]
    public class ProjectController : ControllerBase
    {
        readonly IProjectService _projectService;

        public ProjectController(IProjectService projectService)
        {
            this._projectService = projectService;
        }

        [HttpPost("viewManager")]
        public async Task<IActionResult> ViewManager(ViewManagerProjectRequest request)
        {
            var result = await _projectService.ViewManager(request);
            return Ok(result);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateProject(CreateProjectDto request)
        {
            var result = await _projectService.CreateProject(request);
            return Ok(result);
        }

        [HttpPost("update")]
        public async Task<IActionResult> Update(UpdateProjectRequest request)
        {
            var result = await _projectService.Update(request);
            return Ok(result);
        }

        [HttpPost("delete")]
        public async Task<IActionResult> Delete(DeleteProjectRequest request)
        {
            var result = await _projectService.Delete(request);
            return Ok(result);
        }

    }
}
