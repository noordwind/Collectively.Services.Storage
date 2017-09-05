using System;
using System.Linq;
using System.Threading.Tasks;
using Collectively.Common.Caching;
using Collectively.Services.Storage.Models.Remarks;
using Collectively.Services.Storage.Repositories;

namespace Collectively.Services.Storage.Services
{
    public class RemarkCache : IRemarkCache
    {
        private readonly ICache _cache;
        private readonly IRemarkRepository _remarkRepository;
        private readonly IGroupRepository _groupRepository;

        public RemarkCache(ICache cache,
            IRemarkRepository remarkRepository, 
            IGroupRepository groupRepository)
        {
            _cache = cache;
            _remarkRepository = remarkRepository;
            _groupRepository = groupRepository;
        }

        public async Task AddAsync(Remark remark, bool addGeo = false, bool addLatest = false)
        {
            if (remark.Group != null)
            {
                var group = await _groupRepository.GetAsync(remark.Group.Id);
                remark.Group.Criteria = group.Value.Criteria;
                remark.Group.Members = group.Value.Members.ToDictionary(x => x.UserId, x => x.Role);                
            }
            await _cache.AddAsync(GetDetailsCacheKey(remark.Id), remark);
            if (addGeo)
            {
                await _cache.GeoAddAsync(GetGeoCacheKey(), remark.Location.Longitude, remark.Location.Latitude, 
                    remark.Id.ToString());
            }
            if (addLatest)
            {
                await _cache.AddToSortedSetAsync(GetLatestRemarksCacheKey(), remark.Id.ToString(), 0, limit: 100);
            }
        }

        public async Task DeleteAsync(Guid remarkId, bool deleteGeo = false, bool deleteLatest = false)
        {
            await _cache.DeleteAsync(GetDetailsCacheKey(remarkId));
            if (deleteGeo)
            {
                await _cache.GeoRemoveAsync(GetGeoCacheKey(), remarkId.ToString());
            }
            if (deleteLatest)
            {
                await _cache.RemoveFromSortedSetAsync(GetLatestRemarksCacheKey(), remarkId.ToString());
            }
        }

        private static string GetDetailsCacheKey(Guid remarkId)
        => $"remarks:{remarkId}";

        private static string GetGeoCacheKey()
        => "remarks";

        private static string GetLatestRemarksCacheKey()
        => "remarks-latest";
    }
}