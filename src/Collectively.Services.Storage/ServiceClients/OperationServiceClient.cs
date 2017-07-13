using System;
using System.Threading.Tasks;
using Collectively.Common.ServiceClients;
using Collectively.Common.Types;
using NLog;

namespace Collectively.Services.Storage.ServiceClients
{
    public class OperationServiceClient : IOperationServiceClient
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IServiceClient _serviceClient;
        private readonly string _name;

        public OperationServiceClient(IServiceClient serviceClient, string name)
        {
            _serviceClient = serviceClient;
            _name = name;
        }

        public async Task<Maybe<T>> GetAsync<T>(Guid requestId) where T : class 
        {
            Logger.Debug($"Requesting GetAsync, requestId:{requestId}");
            return await _serviceClient.GetAsync<T>(_name, $"/operations/{requestId}");
        }

        public async Task<Maybe<dynamic>> GetAsync(Guid requestId)
            => await GetAsync<dynamic>(requestId);
    }
}