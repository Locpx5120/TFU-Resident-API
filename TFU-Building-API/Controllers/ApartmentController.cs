﻿using Core.Enums;
using Microsoft.AspNetCore.Mvc;
using TFU_Building_API.Configure;
using TFU_Building_API.Dto;
using TFU_Building_API.Service;

[ApiController]
[Route("api/apartment")]
[CustomFilter]
public class ApartmentController : ControllerBase
{
    private readonly IApartmentService _apartmentService;

    public ApartmentController(IApartmentService apartmentService)
    {
        _apartmentService = apartmentService;
    }

    /// <summary>
    /// API này dùng để lấy danh sách thông tin căn hộ mà người dùng đang sở hữu dựa trên ResidentId.
    /// </summary>
    /// <param name="residentId">ID của người cư trú (resident) để tìm thông tin căn hộ sở hữu.</param>
    /// <returns>Danh sách các căn hộ kèm theo thông tin chi tiết như số phòng, số tầng, thành viên, email, và số điện thoại.</returns>
    [HttpGet("resident/{residentId}")]
    public async Task<IActionResult> GetApartmentsByResidentId(Guid residentId)
    {
        var apartments = await _apartmentService.GetApartmentsByResidentIdAsync(residentId);
        return Ok(apartments);
    }

    /// <summary>
    /// Lấy thông tin chi tiết thành viên của căn hộ dựa trên ApartmentId và tên thành viên (nếu có).
    /// </summary>
    /// <param name="apartmentId">Id của căn hộ.</param>
    /// <param name="memberName">Tên thành viên để tìm kiếm (tùy chọn).</param>
    /// <returns>Danh sách thông tin thành viên căn hộ.</returns>
    [HttpGet("resident/details/{apartmentId}")]
    public async Task<IActionResult> GetApartmentDetails(Guid apartmentId, [FromQuery] string? memberName = null)
    {
        var result = await _apartmentService.GetApartmentDetailsByApartmentIdAsync(apartmentId, memberName);
        return StatusCode(result.Code, result);
    }


    /// <summary>
    /// Thêm thành viên vào căn hộ.
    /// </summary>
    /// <param name="request">Thông tin thành viên cần thêm.</param>
    /// <returns>Kết quả thêm thành viên.</returns>
    [HttpPost("add-apartment-member")]
    public async Task<IActionResult> AddApartmentMember([FromBody] AddApartmentMemberDto request)
    {
        var result = await _apartmentService.AddApartmentMemberAsync(request);
        if (result.Success)
            return Ok(result);
        return BadRequest(result);
    }

    /// <summary>
    /// Lấy danh sách căn hộ theo tòa nhà
    /// </summary>
    /// <param name="buildingId">ID của tòa nhà</param>
    /// <returns>Danh sách căn hộ</returns>
    [HttpGet("by-building")]
    public async Task<IActionResult> GetApartmentsByBuildingId([FromQuery] Guid buildingId)
    {
        if (buildingId == Guid.Empty)
        {
            return BadRequest(new { Success = false, Message = "Invalid building ID." });
        }

        var result = await _apartmentService.GetApartmentsByBuildingIdAsync(buildingId);

        if (result.Success)
        {
            return Ok(result);
        }
        else
        {
            return StatusCode(result.Code == (int)ErrorCodeAPI.SystemIsError ? 500 : 404, result);
        }
    }

}
