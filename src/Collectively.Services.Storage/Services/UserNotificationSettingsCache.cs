using System;
using System.Threading.Tasks;
using Collectively.Common.Caching;
using Collectively.Services.Storage.Models.Notifications;

namespace Collectively.Services.Storage.Services
{
    public class UserNotificationSettingsCache : IUserNotificationSettingsCache
    {
        private readonly ICache _cache;

        public UserNotificationSettingsCache(ICache cache)
        {
            _cache = cache;
        }

        public async Task AddAsync(UserNotificationSettings settings)
        => await _cache.AddAsync(GetCacheKey(settings.UserId), settings);
        
        public async Task DeleteAsync(string userId)
        => await _cache.DeleteAsync(GetCacheKey(userId));

        private static string GetCacheKey(string userId)
        => $"users:{userId}:notifications:settings";            
    }
}