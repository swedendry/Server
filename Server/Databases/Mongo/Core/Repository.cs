using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Server.Databases.Mongo.Core
{
    public class Repository<T> : IRepository<T> where T : class, IEntity, new()
    {
        private readonly MongoClient _client = null;

        public Repository(string connectionString)
        {
            _client = new MongoClient(connectionString);
        }

        private IMongoDatabase GetDatabase(string dbName)
        {
            return _client.GetDatabase(dbName);
        }

        private IMongoCollection<T> GetCollection(string dbName, string collectionName)
        {
            return GetDatabase(dbName).GetCollection<T>(collectionName);
        }

        public async Task<IList<string>> GetDatabases()
        {
            try
            {
                return (await _client.ListDatabasesAsync()).ToList().Select(x => x["name"].ToString()).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IList<string>> GetCollections(string dbName, ListCollectionsOptions options = null)
        {
            try
            {
                return (await GetDatabase(dbName).ListCollectionsAsync(options)).ToList().Select(x => x["name"].ToString()).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<long> Count(string dbName, string collectionName, Expression<Func<T, bool>> filter, CountOptions options = null)
        {
            try
            {
                return (await GetCollection(dbName, collectionName).CountDocumentsAsync(filter, options));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task Add(string dbName, string collectionName, T document)
        {
            try
            {
                await GetCollection(dbName, collectionName).InsertOneAsync(document);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task Add(string dbName, string collectionName, IEnumerable<T> documents)
        {
            try
            {
                await GetCollection(dbName, collectionName).InsertManyAsync(documents);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IList<T>> GetAll(string dbName, string collectionName, FindOptions<T, T> options = null)
        {
            try
            {
                return (await GetCollection(dbName, collectionName).FindAsync(_ => true, options)).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IList<T>> FindBy(string dbName, string collectionName, Expression<Func<T, bool>> filter, FindOptions<T, T> options = null)
        {
            try
            {
                return (await GetCollection(dbName, collectionName).FindAsync(filter, options)).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<T> GetSingle(string dbName, string collectionName, Expression<Func<T, bool>> filter, FindOptions<T, T> options = null)
        {
            try
            {
                return (await GetCollection(dbName, collectionName).FindAsync(filter, options)).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
