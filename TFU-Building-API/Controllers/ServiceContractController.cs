using Microsoft.AspNetCore.Mvc;
using TFU_Building_API.Configure;
using TFU_Building_API.Dto;
using TFU_Building_API.Service;

namespace TFU_Building_API.Controllers
{
    [ApiController]
    [Route("api/service-contract")]
    [CustomFilter]
    public class ServiceContractController : ControllerBase
    {
        private readonly IServiceContractService _serviceContractService;

        public ServiceContractController(IServiceContractService serviceContractService)
        {
            _serviceContractService = serviceContractService;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddServiceContract(CreateServiceContractRequestDto request)
        {
            var response = await _serviceContractService.AddServiceContract(request);

            if (response.Success)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [HttpGet("get/{apartmentId}")]
        public async Task<IActionResult> GetServiceContractDetails(Guid apartmentId)
        {
            var response = await _serviceContractService.GetServiceContractDetails(apartmentId);

            if (response.Success)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [HttpPost("get-all-service")]
        public async Task<IActionResult> GetServiceContractDetails(ServiceContractDetailRequestDto request)
        {
            var response = await _serviceContractService.GetServiceContractDetails(request);

            if (response.Success)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

    }
}
