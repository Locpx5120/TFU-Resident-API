using Microsoft.AspNetCore.Mvc;
using TFU_Building_API.Configure;
using TFU_Building_API.Service;

namespace TFU_Building_API.Controllers
{
    [Route("api/package")]
    [ApiController]
    [CustomFilter]
    public class PackageServiceController : Controller
    {
        private readonly IPackageService _packageService;

        public PackageServiceController(IPackageService packageService)
        {
            _packageService = packageService;
        }
        [HttpGet("get-all")]
        public async Task<IActionResult> GetPackageServices()
        {
            var response = await _packageService.GetPackageServices();

            if (response.Success)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

    }
}
