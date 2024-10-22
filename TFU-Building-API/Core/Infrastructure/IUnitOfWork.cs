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

        #region Staff
        IMasterDataRepository<Staff> StaffRepository { get; }
        #endregion

        #region Building
        IMasterDataRepository<Building> BuildingRepository { get; }
        #endregion

        #region OwnerShip
        IMasterDataRepository<OwnerShip> OwnerShipRepository { get; }
        #endregion

        #region Apartment
        IMasterDataRepository<Apartment> ApartmentRepository { get; }
        #endregion

        #region Resident
        IMasterDataRepository<Resident> ResidentRepository { get; }
        #endregion
        Task<int> SaveChangesAsync();
    }
}
