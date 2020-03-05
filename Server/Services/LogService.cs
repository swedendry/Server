using Server.Databases.Redis.Core;
using StackExchange.Redis.Extensions.Core;
using System;

namespace Server.Services
{
    public enum LogReceiver
    {
        elasticsearch,
        mongodb,
    }

    public enum LogType
    {
        Error,

        Register,
        Login,
        Chat,
    }

    public class LogService
    {
        private static ICacheClient _cache;

        public static void Initialize(IRepository redis)
        {
            _cache = redis.GetCacheClient();
        }

        public static void Log(LogType type, string clientip, string key, object values)
        {
            try
            {
                var rediskey = "logs"; //string.Format("logs:{0}", receiver.ToString());

                _cache.ListAddToLeft(rediskey, new
                {
                    clientip,
                    CollectionName = type.ToString(),
                    Date = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
                    Key = key,
                    Values = values,
                });
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
