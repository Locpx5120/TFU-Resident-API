using Core.Entity;
using Entity;
using Microsoft.EntityFrameworkCore;
using SuperOwnerModels;
using TFU_Resident_API.Data;
using TFU_Resident_API.Entity;

namespace Core.Infrastructure
{
    public partial class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private readonly IUserIdentity _currentUser;
        public AppDbContext AppDbContext => _context;

        public UnitOfWork(
            AppDbContext context,
            IUserIdentity currentUser = null)
        {
            _context = context;
            _currentUser = currentUser ?? new UserIdentity(new HttpContextAccessor());
        }


        #region Authen
        private IMasterDataRepository<User> _userRepository;
        public IMasterDataRepository<User> UserRepository
        {
            get { return _userRepository ??= new MasterDataRepository<User>(_context, _currentUser); }
        }

        private IMasterDataRepository<Role> _roleRepository;
        public IMasterDataRepository<Role> RoleRepository
        {
            get { return _roleRepository ??= new MasterDataRepository<Role>(_context, _currentUser); }
        }
        #endregion

        #region Others
        private IMasterDataRepository<Investor> _investorRepository;
        public IMasterDataRepository<Investor> InvestorRepository
        {
            get { return _investorRepository ??= new MasterDataRepository<Investor>(_context, _currentUser); }
        }

        private IMasterDataRepository<OTPMail> _oTPMailRepository;
        public IMasterDataRepository<OTPMail> OTPMailRepository
        {
            get { return _oTPMailRepository ??= new MasterDataRepository<OTPMail>(_context, _currentUser); }
        }
        #endregion

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
