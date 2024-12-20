﻿using Microsoft.AspNetCore.Mvc;
using TFU_Building_API.Configure;
using TFU_Building_API.Dto;
using TFU_Building_API.Service;

namespace TFU_Building_API.Controllers
{
    [Route("api/staff")]
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

        [HttpPost("addStaff")]
        public async Task<IActionResult> AddStaff([FromBody] StaffRequestDto request)
        {
            var response = await _staffService.AddStaff(request);

            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPost("deleteStaff")]
        public async Task<IActionResult> DeleteStaff([FromBody] StaffDeleteRequestDto request)
        {
            var response = await _staffService.DeleteStaff(request);

            if (response.Success)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [HttpPost("updateStaff")]
        public async Task<IActionResult> UpdateStaff([FromBody] StaffUpdateRequestDto request)
        {
            var response = await _staffService.UpdateStaff(request);

            if (response.Success)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetStaffList([FromQuery] string? searchName)
        {
            var result = await _staffService.GetStaffListAsync(searchName);
            return StatusCode(result.Code, result);
        }

        [HttpPost("listStaffAssigment")]
        public async Task<IActionResult> GetStaffListAssigment([FromQuery] string? searchName)
        {
            var result = await _staffService.GetStaffListAssigment(searchName);
            return StatusCode(result.Code, result);
        }


        //[HttpGet("listEmployee")]
        //public async Task<IActionResult> GetStaffList([FromQuery] StaffSearchRequestDto request)
        //{
        //    var response = await _staffService.GetStaffList(request);

        //    if (response.Success)
        //    {
        //        return Ok(response);
        //    }

        //    return BadRequest(response);
        //}

        //[HttpGet("GetById/{staffId}")]
        //public async Task<IActionResult> GetStaffById(Guid staffId)
        //{
        //    var response = await _staffService.GetStaffById(staffId);

        //    if (!response.Success)
        //    {
        //        return NotFound(response.Message);
        //    }

        //    return Ok(response.Data);
        //}

    }
}
