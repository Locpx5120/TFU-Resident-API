using Microsoft.AspNetCore.Mvc;
using TFU_Building_API.Configure;
using TFU_Building_API.Dto;
using TFU_Building_API.Service;

[ApiController]
[Route("api/service-request")]
[CustomFilter]
public class ServiceRequestController : ControllerBase
{
    private readonly IServiceRequestService _serviceRequestService;

    public ServiceRequestController(IServiceRequestService serviceRequestService)
    {
        _serviceRequestService = serviceRequestService;
    }

    /// <summary>
    /// Retrieve paginated service requests with filtering options
    /// </summary>
    [HttpGet]
    [Route("get-service-requests")]
    public async Task<IActionResult> GetServiceRequests([FromQuery] ServiceRequestSearchDto searchDto)
    {
        var result = await _serviceRequestService.GetServiceRequestsAsync(searchDto);
        if (result.Success)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }
}
