using System;
using System.Threading.Tasks;
using Coolector.Common.Dto.General;
using Coolector.Common.Types;
using Coolector.Services.Storage.Queries;
using Coolector.Services.Storage.Settings;
using Coolector.Services.Users.Shared.Dto;
using NLog;

namespace Coolector.Services.Storage.Services.Users
{
    public class UserServiceClient : IUserServiceClient
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IServiceClient _serviceClient;
        private readonly ProviderSettings _settings;

        public UserServiceClient(IServiceClient serviceClient, ProviderSettings settings)
        {
            _serviceClient = serviceClient;
            _settings = settings;
        }

        public async Task<Maybe<AvailableResourceDto>> IsAvailableAsync(string name)
        {
            Logger.Debug($"Requesting IsAvailableAsync, name:{name}");
            return await _serviceClient.GetAsync<AvailableResourceDto>(_settings.UsersApiUrl, $"users/{name}/available");
        }


        public async Task<Maybe<PagedResult<UserDto>>> BrowseAsync(BrowseUsers query)
        {
            Logger.Debug($"Requesting BrowseAsync, page:{query.Page}, results:{query.Results}");
            return await _serviceClient.GetCollectionAsync<UserDto>(_settings.UsersApiUrl, "users");
        }

        public async Task<Maybe<UserDto>> GetAsync(string userId)
        {
            Logger.Debug($"Requesting GetAsync, userId:{userId}");
            return await _serviceClient.GetAsync<UserDto>(_settings.UsersApiUrl, $"users/{userId}");
        }

        public async Task<Maybe<UserDto>> GetByNameAsync(string name)
        {
            Logger.Debug($"Requesting GetByNameAsync, name:{name}");
            return await _serviceClient.GetAsync<UserDto>(_settings.UsersApiUrl, $"users/{name}/account");
        }

        public async Task<Maybe<UserSessionDto>> GetSessionAsync(Guid id)
        {
            Logger.Debug($"Requesting GetSessionAsync, id:{id}");
            return await _serviceClient.GetAsync<UserSessionDto>(_settings.UsersApiUrl, $"user-sessions/{id}");
        }
    }
}