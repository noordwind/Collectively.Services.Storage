using System;
using System.Threading.Tasks;
using Collectively.Common.Security;
using Collectively.Common.Types;
using Collectively.Services.Storage.Models.Users;
using Collectively.Services.Storage.Queries;
using NLog;

namespace Collectively.Services.Storage.Services.Users
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

        public async Task<Maybe<AvailableResource>> IsAvailableAsync(string name)
        {
            Logger.Debug($"Requesting IsAvailableAsync, name:{name}");
            return await _serviceClient.GetAsync<AvailableResource>(_settings.Url, $"users/{name}/available");
        }

        public async Task<Maybe<PagedResult<User>>> BrowseAsync(BrowseUsers query)
        {
            Logger.Debug($"Requesting BrowseAsync, page:{query.Page}, results:{query.Results}");
            return await _serviceClient.GetCollectionAsync<User>(_settings.Url, "users");
        }

        public async Task<Maybe<User>> GetAsync(string userId)
        {
            Logger.Debug($"Requesting GetAsync, userId:{userId}");
            return await _serviceClient.GetAsync<User>(_settings.Url, $"users/{userId}");
        }

        public async Task<Maybe<User>> GetByNameAsync(string name)
        {
            Logger.Debug($"Requesting GetByNameAsync, name:{name}");
            return await _serviceClient.GetAsync<User>(_settings.Url, $"users/{name}/account");
        }

        public async Task<Maybe<UserSession>> GetSessionAsync(Guid id)
        {
            Logger.Debug($"Requesting GetSessionAsync, id:{id}");
            return await _serviceClient.GetAsync<UserSession>(_settings.Url, $"user-sessions/{id}");
        }
    }
}