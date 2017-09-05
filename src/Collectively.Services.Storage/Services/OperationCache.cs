using System;
using System.Threading.Tasks;
using Collectively.Common.Caching;
using Collectively.Common.Types;
using Collectively.Services.Storage.Models.Operations;
using Newtonsoft.Json;

namespace Collectively.Services.Storage.Services
{
    public class OperationCache : IOperationCache
    {
        private readonly ICache _cache;

        public OperationCache(ICache cache)
        {
            _cache = cache;
        }

        public async Task<Maybe<Operation>> GetAsync(Guid requestId)
        => await _cache.GetAsync<Operation>(GetCacheKey(requestId));

        public async Task AddAsync(Operation operation)
        => await _cache.AddAsync(GetCacheKey(operation.RequestId), 
            operation, TimeSpan.FromMinutes(1));

        private static string GetCacheKey(Guid requestId)
        => $"operations:{requestId}";
    }
}