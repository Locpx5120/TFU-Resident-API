﻿using BuildingModels;
using Core.Entity;
using Microsoft.EntityFrameworkCore;

namespace TFU_Building_API.Core.Infrastructure
{
    public partial class UnitOfWork : IUnitOfWork
    {
        private readonly BuildingContext _context;
        private readonly IUserIdentity _currentUser;
        public BuildingContext buildingContext => _context;

        public UnitOfWork(
            BuildingContext context,
            IUserIdentity currentUser = null)
        {
            _context = context;
            _currentUser = currentUser ?? new UserIdentity(new HttpContextAccessor());
        }


        #region Authen
        //private IMasterDataRepository<User> _userRepository;
        //public IMasterDataRepository<User> UserRepository
        //{
        //    get { return _userRepository ??= new MasterDataRepository<User>(_context, _currentUser); }
        //}

        private IMasterDataRepository<Role> _roleRepository;
        public IMasterDataRepository<Role> RoleRepository
        {
            get { return _roleRepository ??= new MasterDataRepository<Role>(_context, _currentUser); }
        }

        #endregion

        #region Others
        //private IMasterDataRepository<OTPMail> _oTPMailRepository;
        //public IMasterDataRepository<OTPMail> OTPMailRepository => _oTPMailRepository ??= new MasterDataRepository<OTPMail>(_context, _currentUser);
        #endregion

        #region Staff
        private IMasterDataRepository<Staff> _staffRepository;
        public IMasterDataRepository<Staff> StaffRepository
        {
            get { return _staffRepository ??= new MasterDataRepository<Staff>(_context, _currentUser); }
        }
        #endregion

        #region Building
        private IMasterDataRepository<Building> _buildingRepository;
        public IMasterDataRepository<Building> BuildingRepository
        {
            get { return _buildingRepository ??= new MasterDataRepository<Building>(_context, _currentUser); }
        }
        #endregion

        #region OwnerShip
        private IMasterDataRepository<OwnerShip> _ownerShipRepository;
        public IMasterDataRepository<OwnerShip> OwnerShipRepository
        {
            get { return _ownerShipRepository ??= new MasterDataRepository<OwnerShip>(_context, _currentUser); }
        }
        #endregion

        #region Apartment
        private IMasterDataRepository<Apartment> _apartmentRepository;
        public IMasterDataRepository<Apartment> ApartmentRepository
        {
            get { return _apartmentRepository ??= new MasterDataRepository<Apartment>(_context, _currentUser); }
        }
        #endregion

        #region Resident
        private IMasterDataRepository<Resident> _residentRepository;
        public IMasterDataRepository<Resident> ResidentRepository
        {
            get { return _residentRepository ??= new MasterDataRepository<Resident>(_context, _currentUser); }
        }
        #endregion

        #region ServiceContract
        private IMasterDataRepository<ServiceContract> _serviceContractRepository;
        public IMasterDataRepository<ServiceContract> ServiceContractRepository
        {
            get { return _serviceContractRepository ??= new MasterDataRepository<ServiceContract>(_context, _currentUser); }
        }
        #endregion

        #region Service
        private IMasterDataRepository<BuildingModels.Service> _serviceRepository;
        public IMasterDataRepository<BuildingModels.Service> ServiceRepository
        {
            get { return _serviceRepository ??= new MasterDataRepository<BuildingModels.Service>(_context, _currentUser); }
        }

        private IMasterDataRepository<Invoice> _invoiceRepository;
        public IMasterDataRepository<Invoice> InvoiceRepository
        {
            get { return _invoiceRepository ??= new MasterDataRepository<Invoice>(_context, _currentUser); }
        }

        private IMasterDataRepository<PackageService> _packageServiceRepository;
        public IMasterDataRepository<PackageService> PackageServiceRepository
        {
            get { return _packageServiceRepository ??= new MasterDataRepository<PackageService>(_context, _currentUser); }
        }
        #endregion

        #region ApartmentType
        private IMasterDataRepository<ApartmentType> _apartmentTypeRepository;
        public IMasterDataRepository<ApartmentType> ApartmentTypeRepository
        {
            get { return _apartmentTypeRepository ??= new MasterDataRepository<ApartmentType>(_context, _currentUser); }
        }
        #endregion

        #region Living
        private IMasterDataRepository<Living> _livingRepository;
        public IMasterDataRepository<Living> LivingRepository
        {
            get { return _livingRepository ??= new MasterDataRepository<Living>(_context, _currentUser); }
        }
        #endregion

        #region Vehicle
        private IMasterDataRepository<Vehicle> _vehicleRepository;
        public IMasterDataRepository<Vehicle> VehicleRepository
        {
            get { return _vehicleRepository ??= new MasterDataRepository<Vehicle>(_context, _currentUser); }
        }
        #endregion 
        #region ServiceCategory
        private IMasterDataRepository<ServiceCategory> _serviceCategoryRepository;
        public IMasterDataRepository<ServiceCategory> ServiceCategoryRepository
        {
            get { return _serviceCategoryRepository ??= new MasterDataRepository<ServiceCategory>(_context, _currentUser); }
        }
        #endregion
        #region ThirdParty
        private IMasterDataRepository<ThirdParty> _thirdPartyRepository;
        public IMasterDataRepository<ThirdParty> ThirdPartyRepository
        {
            get { return _thirdPartyRepository ??= new MasterDataRepository<ThirdParty>(_context, _currentUser); }
        }

        private IMasterDataRepository<ThirdPartyContact> _thirdPartyContractRepository;
        public IMasterDataRepository<ThirdPartyContact> ThirdPartyContractRepository
        {
            get { return _thirdPartyContractRepository ??= new MasterDataRepository<ThirdPartyContact>(_context, _currentUser); }
        }
        #endregion

        private IMasterDataRepository<Notify> _notifyRepository;
        public IMasterDataRepository<Notify> NotifyRepository
        {
            get { return _notifyRepository ??= new MasterDataRepository<Notify>(_context, _currentUser); }
        }
        private IMasterDataRepository<ImgBase> _imgBaseRepository;
        public IMasterDataRepository<ImgBase> ImgBaseRepository
        {
            get { return _imgBaseRepository ??= new MasterDataRepository<ImgBase>(_context, _currentUser); }
        }
        private IMasterDataRepository<Assigment> _assigmentRepository;
        public IMasterDataRepository<Assigment> AssigmentRepository
        {
            get { return _assigmentRepository ??= new MasterDataRepository<Assigment>(_context, _currentUser); }
        }

        public async Task<int> SaveChangesAsync()
        {
            if (_currentUser != null)
            {
                SaveChangeDetail();
            }
            return await _context.SaveChangesAsync();
        }

        private void SaveChangeDetail()
        {
            var entries = _context.ChangeTracker.Entries();
            var userDateTime = DateTime.Now;

            foreach (var e in entries)
            {
                if (e.Entity is IEntityBase entity)
                {
                    switch (e.State)
                    {
                        case EntityState.Added:
                            entity.IsDeleted = false;
                            entity.InsertedById = _currentUser.UserId ?? Guid.Empty;
                            entity.InsertedAt = userDateTime;
                            entity.UpdatedById = entity.UpdatedById != Guid.Empty ? entity.UpdatedById : _currentUser.UserId;
                            entity.UpdatedAt = userDateTime;
                            break;
                        case EntityState.Modified:
                            entity.UpdatedById = entity.UpdatedById != Guid.Empty ? entity.UpdatedById : _currentUser.UserId;
                            entity.UpdatedAt = userDateTime;
                            break;
                    }
                }
            }
        }


        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
