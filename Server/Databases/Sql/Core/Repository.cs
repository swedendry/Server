using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Server.Databases.Sql.Core
{
    public class Repository<U, T> : IRepository<T> where U : IUnitOfWork where T : class
    {
        protected readonly U _unitOfWork;

        public Repository(U unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// common
        /// </summary>
        protected DbContext Context()
        {
            return _unitOfWork.Context;
        }

        protected DbSet<T> Set()
        {
            return Context().Set<T>();
        }

        public IQueryable<T> Queryable(bool isTracking = false)
        {
            return isTracking ? Set() : Set().AsNoTracking();
        }

        public IQueryable<T> Queryable(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> eagerLoad = null, Action<IQueryable<T>> explicitLoad = null, bool isTracking = false)
        {
            var query = Queryable(isTracking);

            if (eagerLoad != null)
                query = eagerLoad(query);

            if (filter != null)
                query = query.Where(filter);

            explicitLoad?.Invoke(query);

            return query;
        }

        /// <summary>
        /// Count
        /// </summary>
        public virtual int Count()
        {
            return Queryable(false).Count();
        }

        public virtual async Task<int> CountAsync()
        {
            return await Queryable(false).CountAsync();
        }

        /// <summary>
        /// Get
        /// </summary>
        public virtual T Get(Expression<Func<T, bool>> filter, Func<IQueryable<T>, IIncludableQueryable<T, object>> eagerLoad = null, Action<IQueryable<T>> explicitLoad = null, bool isTracking = false)
        {
            return Queryable(filter, eagerLoad, explicitLoad, isTracking).FirstOrDefault();
        }

        public virtual async Task<T> GetAsync(Expression<Func<T, bool>> filter, Func<IQueryable<T>, IIncludableQueryable<T, object>> eagerLoad = null, Action<IQueryable<T>> explicitLoad = null, bool isTracking = false)
        {
            return await Queryable(filter, eagerLoad, explicitLoad, isTracking).FirstOrDefaultAsync();
        }

        /// <summary>
        /// GetMany
        /// </summary>
        public virtual List<T> GetMany(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> eagerLoad = null, Action<IQueryable<T>> explicitLoad = null, bool isTracking = false)
        {
            return Queryable(filter, eagerLoad, explicitLoad, isTracking).ToList();
        }

        public virtual async Task<List<T>> GetManyAsync(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> eagerLoad = null, Action<IQueryable<T>> explicitLoad = null, bool isTracking = false)
        {
            return await Queryable(filter, eagerLoad, explicitLoad, isTracking).ToListAsync();
        }

        /// <summary>
        /// Add
        /// </summary>
        public virtual void Add(params T[] entities)
        {
            Set().AddRange(entities);
        }

        public virtual void Add(IEnumerable<T> entities)
        {
            Set().AddRange(entities);
        }

        public virtual async Task AddAsync(params T[] entities)
        {
            await Set().AddRangeAsync(entities);
        }

        public virtual async Task AddAsync(IEnumerable<T> entities)
        {
            await Set().AddRangeAsync(entities);
        }

        /// <summary>
        /// Update
        /// </summary>
        public virtual void Update(params T[] entities)
        {
            Set().UpdateRange(entities);
        }

        public virtual void Update(IEnumerable<T> entities)
        {
            Set().UpdateRange(entities);
        }

        public virtual async Task UpdateAsync(params T[] entities)
        {
            Update(entities);

            await Task.CompletedTask;
        }

        public virtual async Task UpdateAsync(IEnumerable<T> entities)
        {
            Update(entities);

            await Task.CompletedTask;
        }

        /// <summary>
        /// Delete
        /// </summary>
        public virtual void Delete(params T[] entities)
        {
            Set().RemoveRange(entities);
        }

        public virtual void Delete(IEnumerable<T> entities)
        {
            Set().RemoveRange(entities);
        }

        public virtual async Task DeleteAsync(params T[] entities)
        {
            Delete(entities);

            await Task.CompletedTask;
        }

        public virtual async Task DeleteAsync(IEnumerable<T> entities)
        {
            Delete(entities);

            await Task.CompletedTask;
        }

        /// <summary>
        /// DeleteMany
        /// </summary>
        public virtual void DeleteMany(Expression<Func<T, bool>> filter = null)
        {
            var entities = GetMany(filter, isTracking: false);

            Delete(entities.ToArray());
        }

        public virtual async Task DeleteManyAsync(Expression<Func<T, bool>> filter = null)
        {
            var entities = await GetManyAsync(filter, isTracking: false);

            await DeleteAsync(entities.ToArray());

            await Task.CompletedTask;
        }

        /// <summary>
        /// ExecuteSqlRaw
        /// </summary>
        public virtual int ExecuteSqlRaw(string sql)
        {
            return Context().Database.ExecuteSqlRaw(sql);
        }

        public virtual async Task<int> ExecuteSqlRawAsync(string sql)
        {
            return await Context().Database.ExecuteSqlRawAsync(sql);
        }

        ///// <summary>
        ///// Commit
        ///// </summary>
        //public virtual void Commit()
        //{
        //    Context().SaveChanges();
        //}

        //public virtual async Task CommitAsync()
        //{
        //    await Context().SaveChangesAsync();
        //}
    }
}
