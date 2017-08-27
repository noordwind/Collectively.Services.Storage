using System;
using System.Threading.Tasks;
using Collectively.Services.Storage.Models.Operations;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Collectively.Services.Storage.Services
{
    public class OperationService : IOperationService
    {
        private readonly IDistributedCache _cache;

        public OperationService(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task SetAsync(Operation operation)
        => await _cache.SetStringAsync($"operations:{operation.RequestId}", 
            JsonConvert.SerializeObject(operation), new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.UtcNow.AddMinutes(1)
            });
    }
}