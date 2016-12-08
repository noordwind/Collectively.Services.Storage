using System;
using System.Threading.Tasks;
using Coolector.Common.Dto.General;
using Coolector.Common.Types;
using Coolector.Services.Storage.Queries;
using Coolector.Services.Storage.Repositories;
using Coolector.Services.Storage.Services.Users;
using Coolector.Services.Users.Shared.Dto;

namespace Coolector.Services.Storage.Providers.Users
{
    public class UserProvider : IUserProvider
    {
        private readonly IProviderClient _providerClient;
        private readonly IUserRepository _userRepository;
        private readonly IUserSessionRepository _userSessionRepository;
        private readonly IUserServiceClient _userServiceClient;

        public UserProvider(IProviderClient providerClient, 
            IUserRepository userRepository,
            IUserSessionRepository userSessionRepository,
            IUserServiceClient userServiceClient)
        {
            _providerClient = providerClient;
            _userRepository = userRepository;
            _userSessionRepository = userSessionRepository;
            _userServiceClient = userServiceClient;
        }

        public async Task<Maybe<AvailableResourceDto>> IsAvailableAsync(string name) 
            => await _providerClient.GetAsync(
                async () => await _userRepository.IsNameAvailableAsync(name),
                async () => await _userServiceClient.IsAvailableAsync(name));

        public async Task<Maybe<PagedResult<UserDto>>> BrowseAsync(BrowseUsers query) 
            => await _providerClient.GetCollectionAsync(
                async () => await _userRepository.BrowseAsync(query));

        public async Task<Maybe<UserDto>> GetAsync(string userId) 
            => await _providerClient.GetAsync(
                async () => await _userRepository.GetByIdAsync(userId),
                async () => await _userServiceClient.GetAsync(userId));

        public async Task<Maybe<UserDto>> GetByNameAsync(string name)
            => await _providerClient.GetAsync(
                async () => await _userRepository.GetByNameAsync(name),
                async () => await _userServiceClient.GetByNameAsync(name));

        public async Task<Maybe<UserSessionDto>> GetSessionAsync(Guid id)
            => await _providerClient.GetAsync(
                async () => await _userSessionRepository.GetByIdAsync(id),
                async () => await _userServiceClient.GetSessionAsync(id));
    }
}