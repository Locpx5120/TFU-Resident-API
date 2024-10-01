using Constant;
using System.Security.Claims;

namespace Core.Infrastructure
{
    public class UserIdentity : IUserIdentity
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserIdentity(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }


        private Guid? _userId;
        public Guid? UserId
        {
            get
            {
                if (!_userId.HasValue)
                {
                    if (_httpContextAccessor.HttpContext?.Request.Headers != null)
                    {
                        var userIdString = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                        if (!string.IsNullOrEmpty(userIdString))
                        {
                            _userId = Guid.Parse(userIdString.Trim());
                        }
                    }
                }

                return _userId ?? Constants.SYSTEM_USER_ID;
            }
        }


        private string _userName;
        public string? UserName
        {
            get
            {
                if (string.IsNullOrEmpty(_userName))
                {
                    if (_httpContextAccessor.HttpContext?.Request.Headers != null)
                    {
                        _userName = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
                    }
                }

                return _userName ?? Constants.SYSTEM_USER_NAME;
            }
        }


        private string _roleName;
        public string? RoleName
        {
            get
            {
                if (string.IsNullOrEmpty(_roleName))
                {
                    if (_httpContextAccessor.HttpContext?.Request.Headers != null)
                    {
                        _userName = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);
                    }
                }

                return _userName ?? Constants.SYSTEM_USER_ROLE_NAME;
            }
        }
    }

    public class UserIdentityForBackgroundJob : IUserIdentity
    {
        public virtual Guid? UserId => Constants.SYSTEM_USER_ID;
        public virtual string? UserName => Constants.SYSTEM_USER_NAME;
        public virtual string? RoleName => Constants.SYSTEM_USER_ROLE_NAME;
    }
}
