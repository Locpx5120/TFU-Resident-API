using Microsoft.AspNetCore.Mvc;
using TFU_Building_API.Configure;
using TFU_Building_API.Dto;
using TFU_Building_API.Service;
using TFU_Building_API.Service.impl;

namespace TFU_Building_API.Controllers
{
    [Route("api/ceo")]
    [ApiController]
    [CustomFilter]
    public class ResidentController : ControllerBase
    {
        private readonly IResidentService _residentService;
        private readonly IResidentPayment _residentPayment;

        public ResidentController(
            IResidentService residentService,
            IResidentPayment residentPayment)
        {
            _residentService = residentService;
            _residentPayment = residentPayment;
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

        //[HttpPost("GetByOwnershipId")]
        //public async Task<IActionResult> GetResidentsByOwnershipId(ResidentSearchRequestDto request)
        //{
        //    var response = await _residentService.GetResidentsByOwnershipId(request);

        //    if (!response.Success)
        //    {
        //        return BadRequest(response);
        //    }

        //    return Ok(response);
        //}

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

        [HttpGet("get-resident-payments")]
        public async Task<IActionResult> GetResidentPayments([FromQuery] ResidentPaymentRequestDto request)
        {
            var response = await _residentPayment.GetResidentPayments(request);

            if (response.Success)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        /// <summary>
        /// Adds multiple members to an apartment.
        /// </summary>
        /// <param name="request">Request containing apartment ID and member details.</param>
        /// <returns>Response indicating success or failure of the operation.</returns>
        [HttpPost("add-members")]
        public async Task<IActionResult> AddMembers([FromBody] AddMemberRequestDto request)
        {
            var result = await _residentService.AddMembersAsync(request);
            return StatusCode(result.Code, result);
        }

        [HttpGet("member-service-details/{serviceContractId}")]
        public async Task<IActionResult> GetMemberServiceDetails(Guid serviceContractId)
        {
            var result = await _residentService.GetMemberServiceDetailAsync(serviceContractId);

            if (!result.Success)
            {
                return StatusCode(result.Code, new { result.Success, result.Message });
            }

            return Ok(new { result.Success, result.Message, Data = result.Data });
        }
    }
}
