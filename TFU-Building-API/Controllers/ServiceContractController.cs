using Core.Enums;
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

        [HttpPost("add-vehicle-service")]
        /// <summary>
        /// Thêm dịch vụ gửi xe cho cư dân.
        /// </summary>
        /// <param name="request">Yêu cầu thêm dịch vụ gửi xe.</param>
        /// <returns>Kết quả thêm dịch vụ.</returns>
        public async Task<IActionResult> AddVehicleService([FromBody] AddVehicleServiceRequestDto request)
        {
            var result = await _serviceContractService.AddVehicleServiceAsync(request);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("vehicle-service-details/{serviceContractId}")]
        public async Task<IActionResult> GetVehicleServiceDetails(Guid serviceContractId)
        {
            var result = await _serviceContractService.GetVehicleServiceDetailAsync(serviceContractId);

            if (!result.Success)
            {
                return StatusCode(result.Code, new { result.Success, result.Message });
            }

            return Ok(new { result.Success, result.Message, Data = result.Data });
        }

        /// <summary>
        /// Cập nhật trạng thái và ghi chú cho yêu cầu dịch vụ
        /// </summary>
        /// <param name="request">Thông tin yêu cầu cập nhật</param>
        /// <returns>Kết quả cập nhật yêu cầu dịch vụ</returns>
        [HttpPut("update-service")]
        public async Task<IActionResult> UpdateVehicleServiceRequest([FromBody] UpdateVehicleServiceRequestDto request)
        {
            if (request == null || request.ServiceContractId == Guid.Empty)
            {
                return BadRequest(new { Success = false, Message = "Invalid request data." });
            }

            var result = await _serviceContractService.UpdateVehicleServiceRequestAsync(request);

            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return StatusCode(result.Code == (int)ErrorCodeAPI.NotFound ? 404 : 500, result);
            }
        }

        /// <summary>
        /// Add a new service contract for a third party.
        /// </summary>
        /// <param name="request">Request data for adding a service contract.</param>
        /// <returns>Response indicating the result of the operation.</returns>
        [HttpPost]
        [Route("extend-contract")]
        public async Task<IActionResult> AddServiceContract([FromBody] AddServiceContractThirdPartyRequestDto request)
        {
            var result = await _serviceContractService.AddServiceContractThirdPartyAsync(request);
            return StatusCode(result.Code, result);
        }
    }
}
