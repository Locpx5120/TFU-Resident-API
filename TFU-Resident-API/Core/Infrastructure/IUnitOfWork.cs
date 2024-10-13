using Entity;
using SuperOwnerModels;
using TFU_Resident_API.Data;
using TFU_Resident_API.Entity;

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
        IMasterDataRepository<Investor> InvestorRepository { get; }
        IMasterDataRepository<OTPMail> OTPMailRepository { get; }

        #endregion

        Task<int> SaveChangesAsync();
    }
}
