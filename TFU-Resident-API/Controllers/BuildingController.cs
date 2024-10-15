using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TFU_Resident_API.Dto;
using TFU_Resident_API.Services;

namespace TFU_Resident_API.Controllers
{
    [Route("api/building")]
    [ApiController]
    [Authorize]
    public class BuildingController : ControllerBase
    {
        private readonly IBuildingService buildingService;

        public BuildingController(IBuildingService buildingService)
        {
            this.buildingService = buildingService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateBuildingDto createBuildingDto)
        {
            var result = await buildingService.Create(createBuildingDto);
            return Ok(result);
        }
    }
}
