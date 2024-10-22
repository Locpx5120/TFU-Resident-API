using BuildingModels;
using Microsoft.AspNetCore.Mvc;
using TFU_Building_API.Configure;
using TFU_Building_API.Dto;
using TFU_Building_API.Service;
using Status = BuildingModels.Status;

namespace TFU_Building_API.Controllers
{
    [Route("api/ceo")]
    [ApiController]
    [CustomFilter]
    public class StaffController : ControllerBase
    {
        private readonly IStaffService _staffService; // Thêm service để xử lý logic của Staff

        // Inject StaffService thông qua constructor
        public StaffController(IStaffService staffService)
        {
            _staffService = staffService;
        }

        [HttpPost("listEmployee")]
        public async Task<IActionResult> AddStaff([FromBody] StaffRequestDto request)
        {
            var response = await _staffService.AddStaff(request);

            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpDelete("listEmployee")]
        public async Task<IActionResult> DeleteStaff([FromBody] StaffDeleteRequestDto request)
        {
            var response = await _staffService.DeleteStaff(request);

            if (response.Success)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [HttpPut("listEmployee")]
        public async Task<IActionResult> UpdateStaff([FromBody] StaffUpdateRequestDto request)
        {
            var response = await _staffService.UpdateStaff(request);

            if (response.Success)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [HttpGet("listEmployee")]
        public async Task<IActionResult> GetStaffList([FromQuery] StaffSearchRequestDto request)
        {
            var response = await _staffService.GetStaffList(request);

            if (response.Success)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }
    }
}
