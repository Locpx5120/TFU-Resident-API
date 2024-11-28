//using Microsoft.AspNetCore.Mvc;
//using TFU_Resident_API.Dto;
//using TFU_Resident_API.Services;

//namespace TFU_Resident_API.Controllers
//{
//    [Route("api/building")]
//    [ApiController]
//    //[Authorize]
//    public class BuildingController : ControllerBase
//    {
//        private readonly IBuildingService buildingService;

//        public BuildingController(IBuildingService buildingService)
//        {
//            this.buildingService = buildingService;
//        }

//        [HttpPost("Create")]
//        public async Task<IActionResult> Create(CreateBuildingDto createBuildingDto)
//        {
//            var result = await buildingService.Create(createBuildingDto);
//            return Ok(result);
//        }

//        [HttpPost("UpdateDBMig")]
//        public async Task<IActionResult> UpdateDBMig()
//        {
//            var result = await buildingService.UpdateDBMig();
//            return Ok(result);
//        }

//        [HttpPost("GetBuildings")]
//        public async Task<IActionResult> GetBuildings(ViewManagerBuildingRequest request)
//        {
//            var result = await buildingService.ViewManager(request);
//            return Ok(result);
//        }
//    }
//}
