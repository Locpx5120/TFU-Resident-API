using BuildingModels;
using Constant;
using Core.Entity;

namespace TFU_Building_API.Core.Infrastructure
{
    public class MasterDataRepository<T> : MasterDataRepositoryBase<T, BuildingContext> where T : class, IMasterDataEntityBase
    {
        private readonly IUserIdentity _currentUser;

        public MasterDataRepository(BuildingContext dataContext, IUserIdentity currentUser)
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