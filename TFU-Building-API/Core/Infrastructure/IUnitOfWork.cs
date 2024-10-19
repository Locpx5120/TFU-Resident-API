using BuildingModels;
using Role = BuildingModels.Role;

namespace TFU_Building_API.Core.Infrastructure
{
    public partial interface IUnitOfWork : IDisposable
    {
        BuildingContext buildingContext { get; }

        #region Authen
        IMasterDataRepository<User> UserRepository { get; }
        IMasterDataRepository<Role> RoleRepository { get; }
        #endregion

        #region Others
        IMasterDataRepository<OTPMail> OTPMailRepository { get; }
        #endregion

        Task<int> SaveChangesAsync();
    }
}
