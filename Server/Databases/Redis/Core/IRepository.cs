using StackExchange.Redis.Extensions.Core;

namespace Server.Databases.Redis.Core
{
    public interface IRepository
    {
        ICacheClient GetCacheClient();
    }
}
