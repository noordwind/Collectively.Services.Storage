using System;
using System.Threading.Tasks;
using Collectively.Common.Caching;
using Collectively.Services.Storage.Models.Groups;

namespace Collectively.Services.Storage.Services
{
    public class OrganizationCache : IOrganizationCache
    {
        private readonly ICache _cache;

        public OrganizationCache(ICache cache)
        {
            _cache = cache;
        }

        public async Task AddAsync(Organization organization)
        => await _cache.AddAsync(GetCacheKey(organization.Id), organization);
        
        public async Task DeleteAsync(Guid organizationId)
        => await _cache.DeleteAsync(GetCacheKey(organizationId));

        private static string GetCacheKey(Guid organizationId)
        => $"organizations:{organizationId}";        
    }
}