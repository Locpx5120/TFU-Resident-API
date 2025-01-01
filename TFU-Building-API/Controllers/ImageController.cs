using Microsoft.AspNetCore.Mvc;
using TFU_Building_API.Service;

namespace TFU_Building_API.Controllers
{
    [Route("api/Image")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IImageService _imageService;

        public ImageController(IImageService imageService)
        {
            _imageService = imageService;
        }

        [HttpPost("getImageId")]
        public async Task<IActionResult> GetImageId(Guid id)
        {
            var result = await _imageService.Get(id);

            if (result.Success)
            {
                return Ok(result);
            }

            return StatusCode(result.Code, result);
        }

        [HttpPost("getFileId")]
        public async Task<IActionResult> GetFileId(Guid id)
        {
            var result = await _imageService.Get(id);

            if (result.Success)
            {
                return Ok(result);
            }

            return StatusCode(result.Code, result);
        }

        [HttpPost("addFile")]
        public async Task<IActionResult> AddFile([FromForm] IFormFile file)
        {
            if (file == null)
            {
                return BadRequest();
            }
            var result = await _imageService.Add(file);

            if (result.Success)
            {
                return Ok(result);
            }

            return StatusCode(result.Code, result);
        }

    }
}
