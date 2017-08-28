using System;
using System.Threading.Tasks;
using Collectively.Common.Types;
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

        public async Task<Maybe<Operation>> GetAsync(Guid requestId)
        {
            var operation = await _cache.GetStringAsync(GetCacheKey(requestId));

            return operation == null ? null : JsonConvert.DeserializeObject<Operation>(operation);
        }

        public async Task SetAsync(Operation operation)
        => await _cache.SetStringAsync(GetCacheKey(operation.RequestId), 
            JsonConvert.SerializeObject(operation), new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.UtcNow.AddMinutes(1)
            });

        private static string GetCacheKey(Guid requestId)
        => $"operations:{requestId}";
    }
}