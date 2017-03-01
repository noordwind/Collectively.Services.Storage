using Collectively.Common.Types;

namespace Collectively.Services.Storage.Cache
{
    public interface IRedisDatabaseFactory
    {
        Maybe<RedisDatabase> GetDatabase(int id = -1);
    }
}