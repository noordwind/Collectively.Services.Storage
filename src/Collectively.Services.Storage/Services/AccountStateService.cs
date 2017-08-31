using System.Threading.Tasks;
using Collectively.Common.Caching;

namespace Collectively.Services.Storage.Services
{
    public class AccountStateService : IAccountStateService
    {
        private readonly ICache _cache;

        public AccountStateService(ICache cache)
        {
            _cache = cache;
        }

        public async Task SetAsync(string userId, string state)
        => await _cache.AddAsync($"users:{userId}:state", state);

        public async Task DeleteAsync(string userId)
        => await _cache.DeleteAsync($"users:{userId}:state");
    }
}