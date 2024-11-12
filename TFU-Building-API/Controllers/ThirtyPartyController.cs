using Core.Enums;
using Microsoft.AspNetCore.Mvc;
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
}
