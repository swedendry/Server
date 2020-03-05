using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.Databases.Sql.Core
{
    public class UnitOfWork : IUnitOfWork
    {
        public DbContext Context { get; }

        private Dictionary<Type, object> _repositories;

        public UnitOfWork(DbContext context)
        {
            Context = context;
        }

        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            _repositories ??= new Dictionary<Type, object>();

            var type = typeof(TEntity);
            if (!_repositories.ContainsKey(type))
            {
                _repositories[type] = new Repository<IUnitOfWork, TEntity>(this);
            }

            return (IRepository<TEntity>)_repositories[type];
        }

        public virtual async Task CommitAsync()
        {
            await Context.SaveChangesAsync();
        }
    }
}
