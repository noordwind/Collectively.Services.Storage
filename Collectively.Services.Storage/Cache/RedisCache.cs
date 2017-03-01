using System;
using System.Threading.Tasks;
using Collectively.Common.Types;
using Collectively.Common.Extensions;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Collectively.Services.Storage.Cache
{
    public class RedisCache : ICache
    {
        private readonly IDatabase _database;
        private readonly RedisSettings _settings;
        private bool Available => _database != null && _settings.Enabled;

        public RedisCache(Maybe<RedisDatabase> database, RedisSettings settings)
        {
            _database = database.HasValue ? database.Value.Database : null;
            _settings = settings;
        }

        public async Task<Maybe<T>> GetAsync<T>(string key) where T : class
        {
            if (!Available)
                return default(T);

            var fixedKey = key.ToLowerInvariant();
            var value = Deserialize<T>(await _database.StringGetAsync(fixedKey));

            return value;
        }

        public async Task AddAsync(string key, object value, TimeSpan? expiry = null)
        {
            if(!Available)
                return;

            var fixedKey = key.ToLowerInvariant();
            var obj = Serialize(value);
            await _database.StringSetAsync(fixedKey, obj, expiry);
        }

        public async Task DeleteAsync(string key)
        {
            if (!Available)
                return;

            var fixedKey = key.ToLowerInvariant();
            await AddAsync(fixedKey, null, TimeSpan.FromMilliseconds(1));
        }

        private static string Serialize<T>(T value) => JsonConvert.SerializeObject(value);

        private static T Deserialize<T>(string serializedObject)
            => serializedObject.Empty() ? default(T) : JsonConvert.DeserializeObject<T>(serializedObject);
    }
}