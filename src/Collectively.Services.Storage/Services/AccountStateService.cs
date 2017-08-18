using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;

namespace Collectively.Services.Storage.Services
{
    public class AccountStateService : IAccountStateService
    {
        private readonly IDistributedCache _cache;

        public AccountStateService(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task SetAsync(string userId, string state)
        => await _cache.SetStringAsync($"users:{userId}:state", state);

        public async Task DeleteAsync(string userId)
        => await _cache.RemoveAsync($"users:{userId}:state");
    }
}