using Core.Enums;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TFU_Building_API.Configure;
using TFU_Building_API.Dto;
using TFU_Building_API.Service;

[Route("api/thirdparty")]
[ApiController]
[CustomFilter]
public class ThirdPartyController : ControllerBase
{
    private readonly IThirdPartyService _thirdPartyService;

    public ThirdPartyController(
        IThirdPartyService thirdPartyService)
    {
        _thirdPartyService = thirdPartyService;
    }

    /// <summary>
    /// Thêm bên thứ ba thuê căn hộ
    /// </summary>
    [HttpPost("add")]
    public async Task<IActionResult> AddThirdParty([FromBody] AddThirdPartyRequestDto request)
    {
        var result = await _thirdPartyService.AddThirdPartyAsync(request);
        if (result.Success)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    /// <summary>
    /// Thêm hợp đồng bên thứ ba
    /// </summary>
    /// <param name="request">Thông tin hợp đồng</param>
    /// <returns>Kết quả thêm hợp đồng</returns>
    [HttpPost("add-contract")]
    public async Task<IActionResult> AddThirdPartyContact([FromBody] AddThirdPartyContactRequestDto request)
    {
        var result = await _thirdPartyService.AddThirdPartyContactAsync(request);
        return Ok(result);
    }

    /// <summary>
    /// Lấy danh sách các bên thứ ba thuê mặt bằng
    /// </summary>
    /// <param name="request">Tiêu chí tìm kiếm và lọc</param>
    /// <returns>Danh sách các bên thuê mặt bằng</returns>
    [HttpGet("list")]
    public async Task<IActionResult> GetThirdPartyList([FromQuery] ThirdPartyListRequestDto request)
    {
        var result = await _thirdPartyService.GetThirdPartyListAsync(request);
        if (result.Success)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    /// <summary>
    /// Lấy chi tiết hợp đồng của bên thứ ba
    /// </summary>
    /// <param name="thirdPartyId">ID của bên thứ ba</param>
    /// <returns>Thông tin chi tiết hợp đồng</returns>
    [HttpGet("contract-detail/{thirdPartyId}")]
    public async Task<IActionResult> GetThirdPartyContractDetail(Guid thirdPartyId)
    {
        var result = await _thirdPartyService.GetThirdPartyContractDetailAsync(thirdPartyId);
        if (result.Success)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    /// <summary>
    /// Lấy thông tin hợp đồng của bên thứ ba theo StaffId từ token
    /// </summary>
    [HttpGet("contracts")]
    public async Task<IActionResult> GetContractDetails()
    {
        // Giả sử StaffId lấy từ token
        var staffId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        var result = await _thirdPartyService.GetContractDetailsForThirdPartyAsync(staffId);
        if (result.Success)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    /// <summary>
    /// Thêm bên thứ ba thuê dịch vụ.
    /// </summary>
    /// <param name="request">Thông tin của bên thứ ba cần thêm.</param>
    /// <returns>Phản hồi kết quả thêm bên thứ ba.</returns>
    [HttpPost("hire-thirdParty")]
    public async Task<IActionResult> AddThirdPartyHire([FromBody] AddThirdPartyHireRequestDto request)
    {
        var result = await _thirdPartyService.AddThirdPartyHireAsync(request);
        return StatusCode(result.Code, result);
    }
}
