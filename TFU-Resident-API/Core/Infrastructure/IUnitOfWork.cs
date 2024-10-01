using Entity;
using TFU_Resident_API.Data;

namespace Core.Infrastructure
{
    public partial interface IUnitOfWork : IDisposable
    {
        AppDbContext AppDbContext { get; }

        #region Authen
        IMasterDataRepository<User> UserRepository { get; }
        IMasterDataRepository<Role> RoleRepository { get; }
        #endregion

        #region Others

        #endregion

        Task<int> SaveChangesAsync();
    }
}
