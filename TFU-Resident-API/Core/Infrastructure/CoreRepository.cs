using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Core.Infrastructure
{
    public class CoreRepository<T, TC> : ICoreRepository<T>
        where T : class
        where TC : DbContext
    {
        #region protected fields
        protected readonly TC dataContext;
        protected readonly DbSet<T> dbSet;

        public CoreRepository(TC dataContext)
        {
            this.dataContext = dataContext;

            var typeOfDbSet = typeof(DbSet<T>);
            foreach (var prop in dataContext.GetType().GetProperties())
            {
                if (typeOfDbSet == prop.PropertyType)
                {
                    dbSet = prop.GetValue(dataContext, null) as DbSet<T>;
                    break;
                }
            }
            if (dbSet == null)
            {
                dbSet = dataContext.Set<T>();
            }
        }
        #endregion

        #region
        public virtual void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public virtual void Add(IEnumerable<T> entities)
        {
            dbSet.AddRange(entities);
        }

        public virtual void Update(T entity)
        {
            dbSet.Update(entity);
            dataContext.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Update(IEnumerable<T> entities)
        {
            dbSet.UpdateRange(entities);
            dataContext.Entry(entities).State = EntityState.Modified;
        }

        public virtual void Delete(T entity, bool ishardDelete = false)
        {
            dbSet.Remove(entity);
            dataContext.Entry(entity).State = EntityState.Deleted;
        }

        public virtual void Delete(IEnumerable<T> entities, bool ishardDelete = false)
        {
            foreach (var entity in entities)
            {
                Delete(entity, ishardDelete);
            }
        }

        public virtual void Delete(Expression<Func<T, bool>> where, bool ishardDelete = false)
        {
            var entities = GetQuery(where).AsEnumerable();
            foreach (var entity in entities)
            {
                Delete(entity, ishardDelete);
            }
        }

        public virtual T GetById(Guid id)
        {
            return dbSet.Find(id);
        }

        public virtual ValueTask<T> GetByIdAsync(Guid id)
        {
            return dbSet.FindAsync(id);
        }

        public virtual IQueryable<T> GetQuery()
        {
            return dbSet.AsQueryable();
        }

        public virtual IQueryable<T> GetQuery(Expression<Func<T, bool>> where)
        {
            return GetQuery().Where(where);
        }
        #endregion
    }
}
