using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Server.Databases.Mongo.Core
{
    public interface IRepository<T> where T : class, IEntity, new()
    {
        Task<IList<string>> GetDatabases();
        Task<IList<string>> GetCollections(string dbName, ListCollectionsOptions options = null);

        Task<long> Count(string dbName, string collectionName, Expression<Func<T, bool>> filter, CountOptions options = null);
        Task Add(string dbName, string collectionName, T document);
        Task Add(string dbName, string collectionName, IEnumerable<T> documents);
        Task<IList<T>> GetAll(string dbName, string collectionName, FindOptions<T, T> options = null);
        Task<IList<T>> FindBy(string dbName, string collectionName, Expression<Func<T, bool>> filter, FindOptions<T, T> options = null);
        Task<T> GetSingle(string dbName, string collectionName, Expression<Func<T, bool>> filter, FindOptions<T, T> options = null);
    }
}
