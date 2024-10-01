using Core.Entity;
using fake_tool.Core.Helper;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Core.Infrastructure
{
    public abstract class RepositoryBase<T, TC> : CoreRepository<T, TC>, IRepository<T>
        where T : class, IEntityBase
        where TC : DbContext
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryBase{T, TDbContext}"/> class.
        /// </summary>
        /// <param name="dataContext">The data context.</param>
        protected RepositoryBase(TC dataContext) : base(dataContext)
        {
            //base.dataContext.EnableFilter(Constant.EntityFrameworkConfiguration.FilterIsDelete);
        }

        #endregion

        #region Overwrite Methods

        public override void Add(T entity)
        {
            if (entity.Id == Guid.Empty) entity.Id = Guid.NewGuid();
            entity.InsertedById = this.CurrentUserId;
            entity.UpdatedById = this.CurrentUserId;
            entity.InsertedAt = entity.UpdatedAt = DateTime.Now;
            dbSet.Add(entity);
        }

        public override void Update(T entity)
        {
            UpdateEntityObject(entity);
        }

        //public override void Save(T entity)
        //{
        //    dataContext.DisableFilter(Constant.EntityFrameworkConfiguration.FilterIsDelete);
        //    dbset.AddOrUpdate(t => t.Id, entity);
        //    dataContext.EnableFilter(Constant.EntityFrameworkConfiguration.FilterIsDelete);
        //}

        /// <summary>
        /// Deletes the specified context.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="isHardDelete">is hard delete?</param>
        public override void Delete(T entity, bool isHardDelete = false)
        {
            if (isHardDelete)
            {
                dbSet.Remove(entity);
                dataContext.Entry(entity).State = EntityState.Deleted;
            }
            else
            {
                entity.IsDeleted = true;
                UpdateEntityObject(entity);
            }
        }

        /// <summary>
        /// Deletes the specified context.
        /// </summary>
        /// <param name="where">The where.</param>
        /// <param name="isHardDelete">is hard delete?</param>
        public override void Delete(Expression<Func<T, bool>> where, bool isHardDelete = false)
        {
            var entities = GetQuery(where).AsEnumerable();
            foreach (var entity in entities)
            {
                Delete(entity, isHardDelete);
            }
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            return await dbSet.FindAsync(new object[] { id }, CancellationToken.None);
        }

        public IQueryable<T> GetQueryById(Guid id)
        {
            return GetQuery(m => m.Id == id);
        }

        public Task<TResult> GetPropertyById<TResult>(Guid id,
            Expression<Func<T, TResult>> selector)
        {
            return GetQueryById(id).Select(selector).FirstOrDefaultAsync();
        }


        public IQueryable<T> SearchByQuery(List<string> fieldValues, List<string> fieldNames, bool? isActive)
        {
            //if (IsActive != null && IsActive == false)
            //    dataContext.DisableFilter(Constant.EntityFrameworkConfiguration.FilterIsDelete);
            var query = dbSet.Where(s => s.Id != null);
            if (isActive != null)
                query = query.Where(s => s.IsDeleted == !isActive.Value);
            if (fieldValues != null && fieldValues.Count > 0)
            {
                for (int index = 0; index < fieldValues.Count; index++)
                {
                    if (!string.IsNullOrEmpty(fieldValues[index]) && !string.IsNullOrEmpty(fieldNames[index]))
                        query = query.FilterCustom(fieldNames[index], fieldValues[index]);
                }
            }
            return query.AsQueryable();
        }

        ///// <summary>
        ///// SQLs the query.
        ///// </summary>
        ///// <param name="query">The query.</param>
        ///// <param name="jobName">Name of the job.</param>
        ///// <returns></returns>
        //public virtual IQueryable<T> SqlQuery(string query, string jobName)
        //{
        //    //params object[] parameters
        //    var latParam = new ObjectParameter("@job_name", jobName);

        //    object[] parameters = { latParam };

        //    return dbset.SqlQuery(query, parameters).AsQueryable();
        //}

        public T Refresh(T entity)
        {
            dataContext.Entry(entity).State = EntityState.Deleted;
            return GetById(entity.Id);
        }

        public int SaveChanges()
        {
            //try
            //{
            return dataContext.SaveChanges();
            //}
            //catch (DbEntityValidationException e)
            //{
            //    throw new FormattedDbEntityValidationException(e);
            //}
        }

        #endregion

        #region Private Methods

        private void UpdateEntityObject(T entity)
        {
            dbSet.Attach(entity);
            entity.UpdatedById = this.CurrentUserId;
            entity.UpdatedAt = DateTime.Now;
            dataContext.Entry(entity).State = EntityState.Modified;
            dataContext.Entry(entity).GetDatabaseValues().ToObject();
        }
        #endregion

        protected abstract Guid CurrentUserId { get; }
        protected abstract string CurrentUserName { get; }
    }
}
