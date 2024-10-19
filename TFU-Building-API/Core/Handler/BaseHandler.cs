using TFU_Building_API.Core.Infrastructure;

namespace TFU_Building_API.Core.Handler
{
    public abstract class BaseHandler
    {
        protected readonly IUnitOfWork UnitOfWork;
        protected readonly IHttpContextAccessor HttpContextAccessor;

        protected BaseHandler(IUnitOfWork UnitOfWork, IHttpContextAccessor HttpContextAccessor)
        {
            this.UnitOfWork = UnitOfWork;
            this.HttpContextAccessor = HttpContextAccessor;
        }
    }
}
