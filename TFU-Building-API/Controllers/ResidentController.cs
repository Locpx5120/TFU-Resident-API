using Microsoft.AspNetCore.Mvc;
using TFU_Building_API.Configure;
using TFU_Building_API.Dto;
using TFU_Building_API.Service;

namespace TFU_Building_API.Controllers
{
    [Route("api/ceo")]
    [ApiController]
    [CustomFilter]
    public class ResidentController : ControllerBase
    {
        private readonly IResidentService _residentService;

        public ResidentController(IResidentService residentService)
        {
            _residentService = residentService;
        }

        [HttpPost("addResident")]
        public async Task<IActionResult> AddResident(ResidentRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _residentService.AddResident(request);
            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPut("updateResident")]
        public async Task<IActionResult> UpdateResident(ResidentUpdateRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _residentService.UpdateResident(request);
            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPost("GetByOwnershipId")]
        public async Task<IActionResult> GetResidentsByOwnershipId(ResidentSearchRequestDto request)
        {
            var response = await _residentService.GetResidentsByOwnershipId(request);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPost("deleteResident")]
        public async Task<IActionResult> DeleteResident(ResidentDeleteRequestDto request)
        {
            var response = await _residentService.DeleteResident(request);

            if (!response.Success)
            {
                return BadRequest(response.Message);
            }

            return Ok(response.Data);
        }

        [HttpGet("GetById/{residentId}")]
        public async Task<IActionResult> GetResidentById(Guid residentId)
        {
            var response = await _residentService.GetResidentById(residentId);

            if (!response.Success)
            {
                return NotFound(response.Message);
            }

            return Ok(response.Data);
        }

    }
}
