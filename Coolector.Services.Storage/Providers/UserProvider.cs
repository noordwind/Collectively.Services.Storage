using System;
using System.Threading.Tasks;
using Coolector.Common.Dto.General;
using Coolector.Common.Dto.Users;
using Coolector.Common.Types;
using Coolector.Services.Storage.Queries;
using Coolector.Services.Storage.Repositories;
using Coolector.Services.Storage.Settings;

namespace Coolector.Services.Storage.Providers
{
    public class UserProvider : IUserProvider
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserSessionRepository _userSessionRepository;
        private readonly IProviderClient _providerClient;
        private readonly ProviderSettings _providerSettings;

        public UserProvider(IUserRepository userRepository,
            IUserSessionRepository userSessionRepository,
            IProviderClient providerClient,
            ProviderSettings providerSettings)
        {
            _userRepository = userRepository;
            _userSessionRepository = userSessionRepository;
            _providerClient = providerClient;
            _providerSettings = providerSettings;
        }

        public async Task<Maybe<AvailableResourceDto>> IsAvailableAsync(string name)
            => await _providerClient.GetUsingStorageAsync(_providerSettings.UsersApiUrl, $"users/{name}/available",
                async () => await _userRepository.IsNameAvailableAsync(name), null);

        public async Task<Maybe<PagedResult<UserDto>>> BrowseAsync(BrowseUsers query)
            => await _providerClient.GetCollectionUsingStorageAsync(_providerSettings.UsersApiUrl, "users",
                async () => await _userRepository.BrowseAsync(query));

        public async Task<Maybe<UserDto>> GetAsync(string userId)
            => await _providerClient.GetUsingStorageAsync(_providerSettings.UsersApiUrl, $"users/{userId}",
                async () =>  await _userRepository.GetByIdAsync(userId));

        public async Task<Maybe<UserDto>> GetByNameAsync(string name)
            => await _providerClient.GetUsingStorageAsync(_providerSettings.UsersApiUrl, $"users/{name}/account",
                async () => await _userRepository.GetByNameAsync(name));

        public async Task<Maybe<UserSessionDto>> GetSessionAsync(Guid id)
            => await _providerClient.GetUsingStorageAsync(_providerSettings.UsersApiUrl, $"user-sessions/{id}",
                async () => await _userSessionRepository.GetByIdAsync(id));
    }
}