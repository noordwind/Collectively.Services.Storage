using StackExchange.Redis;

namespace Coolector.Services.Storage.Cache
{
    public class RedisDatabase
    {
        public IDatabase Database { get; }

        public RedisDatabase(IDatabase database)
        {
            Database = database;
        }
    }
}