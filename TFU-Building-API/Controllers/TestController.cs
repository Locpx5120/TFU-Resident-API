using BuildingModels;
using Microsoft.AspNetCore.Mvc;
using TFU_Building_API.Configure;
using Status = BuildingModels.Status;

namespace TFU_Building_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [CustomFilter]
    public class TestController : ControllerBase
    {
        private readonly BuildingContext buildingContext;
        public TestController()
        {
            this.buildingContext = buildingContext;
        }

        //[HttpGet("Test")]
        //public IActionResult Test()
        //{
        //    var a = this.HttpContext.RequestServices.GetService<BuildingContext>();
        //    return Ok(a.Statuses.FirstOrDefault().StatusName);
        //}

        //[HttpPost("TestPost")]
        //public IActionResult TestPost(String a)
        //{
        //    var aa = this.HttpContext.RequestServices.GetService<BuildingContext>();
        //    Status status = new BuildingModels.Status();
        //    status.StatusName = a;
        //    aa.Statuses.Add(status);
        //    aa.SaveChanges();

        //    return Ok(status);
        //}
    }
}
