using Microsoft.AspNetCore.Mvc;
using TFU_Building_API.Configure;
using TFU_Building_API.Dto;
using TFU_Building_API.Service;

namespace TFU_Building_API.Controllers
{
    [Route("api/notify")]
    [ApiController]
    [CustomFilter]
    public class NotifyController : ControllerBase
    {
        private readonly INotifyService _notifyService;

        public NotifyController(INotifyService notifyService)
        {
            _notifyService = notifyService;
        }

        /// <summary>
        /// Tạo mới một bản tin.
        /// </summary>
        /// <param name="request">Thông tin bản tin cần tạo.</param>
        /// <returns>Thông tin về bản tin vừa tạo.</returns>
        [HttpPost("create")]
        public async Task<IActionResult> CreateNotify([FromForm] CreateNotifyRequestDto request)
        {
            var result = await _notifyService.CreateNotifyAsync(request);

            if (result.Success)
            {
                return Ok(result);
            }

            return StatusCode(result.Code, result);
        }

        [HttpPost("get-notifies")]
        public async Task<IActionResult> GetNotifies([FromBody] NotifyFilterRequestDto request)
        {
            var result = await _notifyService.GetNotifiesAsync(request);
            return StatusCode(result.Code, result);
        }
    }
}

