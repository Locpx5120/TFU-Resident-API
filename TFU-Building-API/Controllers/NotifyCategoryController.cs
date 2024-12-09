using Microsoft.AspNetCore.Mvc;
using TFU_Building_API.Configure;

namespace TFU_Building_API.Controllers
{
    [Route("api/notify-category")]
    [ApiController]
    [CustomFilter]
    public class NotifyCategoryController : ControllerBase
    {
        //private readonly INotifyCategoryService _notifyService;

        //public NotifyCategoryController(INotifyCategoryService notifyService)
        //{
        //    _notifyService = notifyService;
        //}

        //[HttpGet("get-all")]
        //public async Task<IActionResult> GetNotifyCategories()
        //{
        //    var result = await _notifyService.GetNotifyCategoriesAsync();
        //    return StatusCode(result.Code, result);
        //}
    }
}

