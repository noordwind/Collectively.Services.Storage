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

        public async Task AddRemarkAsync(string userId, Guid remarkId)
        => await _cache.AddToSetAsync(GetRemarksCacheKey(userId), remarkId.ToString());

        public async Task DeleteRemarkAsync(string userId, Guid remarkId)
        => await _cache.RemoveFromSetAsync(GetRemarksCacheKey(userId), remarkId.ToString());

        private static string GetCacheKey(string userId)
        => $"users:{userId}";

        private static string GetRemarksCacheKey(string userId)
        => $"users:{userId}:remarks";
    }
}