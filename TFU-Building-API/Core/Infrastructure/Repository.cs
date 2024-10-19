using Constant;
using Core.Entity;
using Microsoft.EntityFrameworkCore;

namespace TFU_Building_API.Core.Infrastructure
{
    public class Repository<T> : RepositoryBase<T, DbContext> where T : class, IEntityBase
    {
        private readonly IUserIdentity _currentUser;

        public Repository(DbContext dataContext, IUserIdentity currentUser)
            : base(dataContext)
        {
            _currentUser = currentUser;
        }

        protected override Guid CurrentUserId
        {
            get
            {
                if (_currentUser != null)
                {
                    return _currentUser.UserId ?? Guid.Empty;
                }

                return Constants.SYSTEM_USER_ID;
            }
        }

        protected override string CurrentUserName
        {
            get
            {
                if (_currentUser != null)
                {
                    return _currentUser.UserName ?? "";
                }

                return Constants.SYSTEM_USER_NAME;
            }
        }
    }
}