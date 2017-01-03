using System;
using System.Threading.Tasks;
using Coolector.Common.Security;
using Coolector.Common.Types;
using Coolector.Services.Storage.Queries;
using Coolector.Services.Users.Shared.Dto;
using NLog;

namespace Coolector.Services.Storage.Services.Users
{
    public class UserServiceClient : IUserServiceClient
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IServiceClient _serviceClient;
        private readonly ServiceSettings _settings;

        public UserServiceClient(IServiceClient serviceClient, ServiceSettings settings)
        {
            _serviceClient = serviceClient;
            _settings = settings;
            _serviceClient.SetSettings(settings);
        }

        public async Task<Maybe<AvailableResourceDto>> IsAvailableAsync(string name)
        {
            Logger.Debug($"Requesting IsAvailableAsync, name:{name}");
            return await _serviceClient.GetAsync<AvailableResourceDto>(_settings.Url, $"users/{name}/available");
        }

        public async Task<Maybe<PagedResult<UserDto>>> BrowseAsync(BrowseUsers query)
        {
            Logger.Debug($"Requesting BrowseAsync, page:{query.Page}, results:{query.Results}");
            return await _serviceClient.GetCollectionAsync<UserDto>(_settings.Url, "users");
        }

        public async Task<Maybe<UserDto>> GetAsync(string userId)
        {
            Logger.Debug($"Requesting GetAsync, userId:{userId}");
            return await _serviceClient.GetAsync<UserDto>(_settings.Url, $"users/{userId}");
        }

        public async Task<Maybe<UserDto>> GetByNameAsync(string name)
        {
            Logger.Debug($"Requesting GetByNameAsync, name:{name}");
            return await _serviceClient.GetAsync<UserDto>(_settings.Url, $"users/{name}/account");
        }

        public async Task<Maybe<UserSessionDto>> GetSessionAsync(Guid id)
        {
            Logger.Debug($"Requesting GetSessionAsync, id:{id}");
            return await _serviceClient.GetAsync<UserSessionDto>(_settings.Url, $"user-sessions/{id}");
        }
    }
}