using Constant;
using Core.Entity;
using TFU_Resident_API.Data;

namespace Core.Infrastructure
{
    public class MasterDataRepository<T> : MasterDataRepositoryBase<T, AppDbContext> where T : class, IMasterDataEntityBase
    {
        private readonly IUserIdentity _currentUser;

        public MasterDataRepository(AppDbContext dataContext, IUserIdentity currentUser)
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