using Microsoft.AspNetCore.Mvc;
using TFU_Building_API.Configure;
using TFU_Building_API.Service;

namespace TFU_Building_API.Controllers
{
    [ApiController]
    [Route("api/servicecategory")]
    [CustomFilter]
    public class ServiceCategoryController : ControllerBase
    {
        private readonly IServiceCategoryService _serviceCategoryService;

        public ServiceCategoryController(IServiceCategoryService serviceCategoryService)
        {
            _serviceCategoryService = serviceCategoryService;
        }

        /// <summary>
        /// Lấy danh sách tất cả các danh mục dịch vụ.
        /// </summary>
        /// <returns>Danh sách các danh mục dịch vụ.</returns>
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetServiceCategories()
        {
            var result = await _serviceCategoryService.GetServiceCategoriesAsync();
            return StatusCode(result.Code, result);
        }
    }
}
