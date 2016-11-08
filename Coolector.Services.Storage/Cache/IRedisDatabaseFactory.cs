using Coolector.Common.Types;

namespace Coolector.Services.Storage.Cache
{
    public interface IRedisDatabaseFactory
    {
        Maybe<RedisDatabase> GetDatabase(int id = -1);
    }
}