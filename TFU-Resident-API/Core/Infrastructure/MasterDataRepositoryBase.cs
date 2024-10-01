using Core.Entity;
using System.Linq.Expressions;
using TFU_Resident_API.Data;

namespace Core.Infrastructure
{
    public abstract class MasterDataRepositoryBase<T, TC> : RepositoryBase<T, TC>, IMasterDataRepository<T>
        where T : class, IMasterDataEntityBase
        where TC : AppDbContext
    {

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="MasterDataRepositoryBase{T,TC}"/> class.
        /// </summary>
        /// <param name="dataContext">The data context.</param>
        protected MasterDataRepositoryBase(TC dataContext) : base(dataContext)
        {
        }

        #endregion


        public virtual IEnumerable<T> GetAllWithInactive()
        {
            return GetQuery(true).ToList();
        }

        public virtual IEnumerable<T> GetManyWithInactive(Expression<Func<T, bool>> where)
        {
            return GetQuery(true).Where(where).ToList();
        }

        public IQueryable<T> GetQuery(bool includeInactive = false)
        {
            var query = base.GetQuery();
            if (includeInactive)
            {
                return query;
            }

            return query.Where(m => m.IsActive);
        }

        public new IQueryable<T> GetQuery(Expression<Func<T, bool>> where)
        {
            return GetQuery().Where(where);
        }

        public IQueryable<T> GetQueryWithInactive(Expression<Func<T, bool>> where)
        {
            return GetQuery(true).Where(where);
        }

        public IEnumerable<T> GetMany(Expression<Func<T, bool>> where)
        {
            return GetQuery(false).Where(where).ToList();
        }
    }
}