using System;
using Collectively.Common.Types;
using NLog;
using StackExchange.Redis;

namespace Collectively.Services.Storage.Cache
{
    public class RedisDatabaseFactory : IRedisDatabaseFactory
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly RedisSettings _redisSettings;
        private ConnectionMultiplexer _connectionMultiplexer;

        public RedisDatabaseFactory(RedisSettings redisSettings)
        {
            _redisSettings = redisSettings;
            TryConnect();
        }

        private void TryConnect()
        {
            if (!_redisSettings.Enabled)
            {
                Logger.Info("Connection to Redis server has been skipped (disabled).");

                return;
            }

            try
            {
                _connectionMultiplexer = ConnectionMultiplexer.Connect(_redisSettings.ConnectionString);
                Logger.Info("Connection to Redis server has been established.");
            }
            catch (Exception ex)
            {
                Logger.Error("Could not connect to Redis server.");
                Logger.Error(ex);
            }
        }

        public Maybe<RedisDatabase> GetDatabase(int id = -1)
        {
            var database = _connectionMultiplexer?.GetDatabase(id);

            return database == null ? null : new RedisDatabase(database);
        }
    }
}