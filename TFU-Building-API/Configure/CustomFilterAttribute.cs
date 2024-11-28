using BuildingModels;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using TFU_Resident_API.Data;

namespace TFU_Building_API.Configure
{
    public class CustomFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            AppDbContext superOwnerContext = context.HttpContext.RequestServices.GetRequiredService<AppDbContext>();
            BuildingContext buildingContext = context.HttpContext.RequestServices.GetRequiredService<BuildingContext>();

            Guid? buildingPermalink = null;
            //BuildingPermalink --> id building
            // Lấy giá trị từ request header
            var headers = context.HttpContext.Request.Headers;

            // Lấy thông tin từ token (giả sử bạn sử dụng JWT Bearer)
            var tokenBuildingPermalink = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "BuildingPermalink")?.Value;

            // kiểm tra token building có khớp header ( nếu có ) hay không
            if (!String.IsNullOrEmpty(tokenBuildingPermalink))
            {
                buildingPermalink = Guid.Parse(tokenBuildingPermalink);
            }

            // Kiểm tra xem header có chứa "buildingPermalink" hay không
            if (headers.TryGetValue("buildingPermalink", out var headerValues)
                && Guid.TryParse(headerValues.FirstOrDefault(), out Guid buildingPermalinkHeader))
            {
                if (buildingPermalink == null)
                {
                    buildingPermalink = buildingPermalinkHeader;
                }
            }

            if (buildingPermalink == null || buildingPermalink == Guid.Empty)
            {
                // Xử lý khi không có hoặc giá trị buildingPermalink không hợp lệ
                throw new ArgumentException("Invalid or missing buildingPermalink.");
            }

            // Tìm tòa nhà tương ứng với buildingPermalink
            var building = superOwnerContext.Projects.FirstOrDefault(x => x.Id == buildingPermalink);

            // Kiểm tra tòa nhà có tồn tại và có connection string hợp lệ không
            if (building != null && !string.IsNullOrEmpty(building.ConnectionString))
            {
                // Thiết lập connection string cho BuildingContext
                buildingContext.Database.SetConnectionString(building.ConnectionString);
            }
            else
            {
                // Xử lý khi không tìm thấy tòa nhà hoặc connection string không hợp lệ
                throw new InvalidOperationException("Building not found or invalid connection string.");
            }

            base.OnActionExecuting(context);
        }
    }

}
