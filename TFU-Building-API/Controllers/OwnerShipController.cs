using Microsoft.AspNetCore.Mvc;
using TFU_Building_API.Configure;
using TFU_Building_API.Dto;
using TFU_Building_API.Service;

namespace TFU_Building_API.Controllers
{
    [ApiController]
    [Route("api/ceo")]
    [CustomFilter]
    public class OwnerShipController : ControllerBase
    {
        private readonly IOwnerShipService _ownerShipService;

        public OwnerShipController(IOwnerShipService ownerShipService)
        {
            _ownerShipService = ownerShipService;
        }

        [HttpPost("addOwnerShip")]
        public async Task<IActionResult> AddOwnerShip([FromBody] OwnerShipRequestDto request)
        {
            var response = await _ownerShipService.AddOwnerShip(request);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPut("UpdateOwnerShip")]
        public async Task<IActionResult> UpdateOwnerShip([FromBody] OwnerShipUpdateRequestDto request)
        {
            var response = await _ownerShipService.UpdateOwnerShip(request);
            if (!response.Success)
            {
                return BadRequest(response.Message);
            }

            return Ok(response);
        }

        [HttpDelete("deleteOwnerShip")]
        public async Task<IActionResult> DeleteOwnerShip([FromBody] OwnerShipDeleteRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _ownerShipService.DeleteOwnerShip(request.OwnerShipId);
            if (!response.Success)
            {
                return BadRequest(response.Message);
            }

            return Ok(response);
        }

        [HttpGet("getOwnerShips")]
        public async Task<IActionResult> GetOwnerShips([FromQuery] OwnerShipSearchRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _ownerShipService.GetOwnerShips(request);
            if (!response.Success)
            {
                return BadRequest(response.Message);
            }

            return Ok(response.Data);
        }
    }

}
