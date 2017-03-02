using System;
using System.Threading.Tasks;
using Collectively.Common.Security;
using Collectively.Common.Types;
using Collectively.Services.Storage.Models.Operations;
using NLog;

namespace Collectively.Services.Storage.Services.Operations
{
    public class OperationServiceClient : IOperationServiceClient
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IServiceClient _serviceClient;
        private readonly ServiceSettings _settings;

        public OperationServiceClient(IServiceClient serviceClient, ServiceSettings settings)
        {
            _serviceClient = serviceClient;
            _settings = settings;
            _serviceClient.SetSettings(settings);
        }

        public async Task<Maybe<Operation>> GetAsync(Guid requestId)
        {
            Logger.Debug($"Requesting GetAsync, requestId:{requestId}");
            return await _serviceClient.GetAsync<Operation>(_settings.Url, $"/operations/{requestId}");
        }
    }
}