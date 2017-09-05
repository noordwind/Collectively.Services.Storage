using System;
using System.Threading.Tasks;
using Collectively.Common.Caching;
using Collectively.Services.Storage.Models.Users;

namespace Collectively.Services.Storage.Services
{
    public class UserCache : IUserCache
    {
        private readonly ICache _cache;

        public UserCache(ICache cache)
        {
            _cache = cache;
        }

        public async Task AddAsync(User user)
        => await _cache.AddAsync(GetCacheKey(user.UserId), user);
        
        public async Task DeleteAsync(string userId)
        => await _cache.DeleteAsync(GetCacheKey(userId));

        private static string GetCacheKey(string userId)
        => $"users:{userId}";        
    }
}