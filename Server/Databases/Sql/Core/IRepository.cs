using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Server.Databases.Sql.Core
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> Queryable(bool isTracking = false);
        IQueryable<T> Queryable(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> eagerLoad = null, Action<IQueryable<T>> explicitLoad = null, bool isTracking = false);

        int Count();
        T Get(Expression<Func<T, bool>> filter, Func<IQueryable<T>, IIncludableQueryable<T, object>> eagerLoad = null, Action<IQueryable<T>> explicitLoad = null, bool isTracking = false);
        List<T> GetMany(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> eagerLoad = null, Action<IQueryable<T>> explicitLoad = null, bool isTracking = false);
        void Add(params T[] entities);
        void Add(IEnumerable<T> entities);
        void Update(params T[] entities);
        void Update(IEnumerable<T> entities);
        void Delete(params T[] entities);
        void Delete(IEnumerable<T> entities);
        void DeleteMany(Expression<Func<T, bool>> filter = null);
        int ExecuteSqlRaw(string sql);

        Task<int> CountAsync();
        Task<T> GetAsync(Expression<Func<T, bool>> filter, Func<IQueryable<T>, IIncludableQueryable<T, object>> eagerLoad = null, Action<IQueryable<T>> explicitLoad = null, bool isTracking = false);
        Task<List<T>> GetManyAsync(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> eagerLoad = null, Action<IQueryable<T>> explicitLoad = null, bool isTracking = false);
        Task AddAsync(params T[] entities);
        Task AddAsync(IEnumerable<T> entities);
        Task UpdateAsync(params T[] entities);
        Task UpdateAsync(IEnumerable<T> entities);
        Task DeleteAsync(params T[] entities);
        Task DeleteAsync(IEnumerable<T> entities);
        Task DeleteManyAsync(Expression<Func<T, bool>> filter = null);
        Task<int> ExecuteSqlRawAsync(string sql);
    }
}
