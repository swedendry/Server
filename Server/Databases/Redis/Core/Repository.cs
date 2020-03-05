using StackExchange.Redis;
using StackExchange.Redis.Extensions.Core;
using StackExchange.Redis.Extensions.Newtonsoft;
using System;

namespace Server.Databases.Redis.Core
{
    public class Repository : IRepository
    {
        public Repository(string configuration)
        {
            lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
            {
                return ConnectionMultiplexer.Connect(configuration);
            });
        }

        public ICacheClient GetCacheClient()
        {
            return CacheClient;
        }

        private static Lazy<ConnectionMultiplexer> lazyConnection;

        private static ConnectionMultiplexer Connection => lazyConnection.Value;

        private static readonly Lazy<StackExchangeRedisCacheClient> lazyCacheClient = new Lazy<StackExchangeRedisCacheClient>(() =>
        {
            var serializer = new NewtonsoftSerializer();

            return new StackExchangeRedisCacheClient(Connection, serializer);
        });

        private static StackExchangeRedisCacheClient CacheClient => lazyCacheClient.Value;
    }
}
