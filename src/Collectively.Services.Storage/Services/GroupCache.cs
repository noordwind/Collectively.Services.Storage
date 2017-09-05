using System;
using System.Threading.Tasks;
using Collectively.Common.Caching;
using Collectively.Services.Storage.Models.Groups;

namespace Collectively.Services.Storage.Services
{
    public class GroupCache : IGroupCache
    {
        private readonly ICache _cache;

        public GroupCache(ICache cache)
        {
            _cache = cache;
        }

        public async Task AddAsync(Group group)
        => await _cache.AddAsync(GetCacheKey(group.Id), group);
        
        public async Task DeleteAsync(Guid groupId)
        => await _cache.DeleteAsync(GetCacheKey(groupId));

        private static string GetCacheKey(Guid groupId)
        => $"groups:{groupId}";
    }
}