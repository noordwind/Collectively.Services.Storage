using System;
using System.Threading.Tasks;
using Collectively.Common.ServiceClients;
using Collectively.Common.Types;
using Collectively.Services.Storage.ServiceClients.Queries;
using Serilog;

namespace Collectively.Services.Storage.ServiceClients
{
    public class GroupServiceClient : IGroupServiceClient
    {
        private static readonly ILogger Logger = Log.Logger;
        private readonly IServiceClient _serviceClient;
        private readonly string _name;

        public GroupServiceClient(IServiceClient serviceClient, string name)
        {
            _serviceClient = serviceClient;
            _name = name;
        }

        public async Task<Maybe<T>> GetAsync<T>(Guid id) where T : class
        => await _serviceClient.GetAsync<T>(_name, $"groups/{id}");

        public async Task<Maybe<dynamic>> GetAsync(Guid id)
        => await GetAsync<dynamic>(id);

        public async Task<Maybe<T>> GetOrganizationAsync<T>(Guid id) where T : class
        => await _serviceClient.GetAsync<T>(_name, $"organizations/{id}");

        public async Task<Maybe<dynamic>> GetOrganizationAsync(Guid id)
        => await GetOrganizationAsync<dynamic>(id);
    }
}