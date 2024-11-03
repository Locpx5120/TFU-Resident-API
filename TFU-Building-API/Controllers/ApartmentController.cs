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
    /// Lấy thông tin chi tiết thành viên của căn hộ dựa trên ApartmentId.
    /// </summary>
    /// <param name="apartmentId">Id của căn hộ.</param>
    /// <returns>Danh sách thông tin thành viên căn hộ.</returns>
    [HttpGet("resident/details/{apartmentId}")]
    public async Task<IActionResult> GetApartmentDetails(Guid apartmentId)
    {
        var result = await _apartmentService.GetApartmentDetailsByApartmentIdAsync(apartmentId);
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

}
