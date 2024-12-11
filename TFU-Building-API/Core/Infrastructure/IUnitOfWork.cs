using BuildingModels;
using Role = BuildingModels.Role;

namespace TFU_Building_API.Core.Infrastructure
{
    public partial interface IUnitOfWork : IDisposable
    {
        BuildingContext buildingContext { get; }

        #region Authen
        //IMasterDataRepository<User> UserRepository { get; }
        IMasterDataRepository<Role> RoleRepository { get; }
        #endregion

        #region Others
        //IMasterDataRepository<OTPMail> OTPMailRepository { get; }
        #endregion

        #region Staff
        IMasterDataRepository<Staff> StaffRepository { get; }
        #endregion

        #region Building
        IMasterDataRepository<Building> BuildingRepository { get; }
        #endregion

        #region Living
        IMasterDataRepository<Living> LivingRepository { get; }
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

        #region ServiceContract
        IMasterDataRepository<ServiceContract> ServiceContractRepository { get; }

        #endregion

        #region Service
        IMasterDataRepository<BuildingModels.Service> ServiceRepository { get; }

        IMasterDataRepository<Invoice> InvoiceRepository { get; }

        IMasterDataRepository<PackageService> PackageServiceRepository { get; }
        #endregion

        #region ApartmentType
        IMasterDataRepository<ApartmentType> ApartmentTypeRepository { get; }

        #endregion  
        #region Vehicle
        IMasterDataRepository<Vehicle> VehicleRepository { get; }

        #endregion

        #region Assigment
        IMasterDataRepository<Assigment> AssigmentRepository { get; }
        #endregion

        #region ServiceCategory
        IMasterDataRepository<ServiceCategory> ServiceCategoryRepository { get; }

        #endregion


        #region ThirdParty
        IMasterDataRepository<ThirdParty> ThirdPartyRepository { get; }
        IMasterDataRepository<ThirdPartyContact> ThirdPartyContractRepository { get; }

        #endregion

        #region Transaction
        IMasterDataRepository<Transaction> TransactionRepository { get; }
        #endregion

        IMasterDataRepository<Notify> NotifyRepository { get; }
        IMasterDataRepository<ImgBase> ImgBaseRepository { get; }
        Task<int> SaveChangesAsync();
    }
}
