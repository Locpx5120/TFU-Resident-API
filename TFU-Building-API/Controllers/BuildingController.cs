using Microsoft.AspNetCore.Mvc;
using TFU_Building_API.Configure;
using TFU_Building_API.Dto;
using TFU_Building_API.Service;

namespace TFU_Building_API.Controllers
{
    [Route("api/building")]
    [ApiController]
    [CustomFilter]
    public class BuildingController : ControllerBase
    {
        private readonly IBuildingService _buildingService;

        public BuildingController(IBuildingService buildingService)
        {
            _buildingService = buildingService;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddBuilding(BuildingRequestDto request)
        {
            var response = await _buildingService.AddBuilding(request);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPut("edit")]
        public async Task<IActionResult> UpdateBuilding([FromBody] BuildingUpdateRequestDto request)
        {
            var response = await _buildingService.UpdateBuilding(request);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }


        [HttpGet("get")]
        public async Task<IActionResult> GetBuildings()
        {
            var result = await _buildingService.GetBuildingsAsync();
            return StatusCode(result.Code, result);
        }

    }
}
