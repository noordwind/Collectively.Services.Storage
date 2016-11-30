using System;
using System.Threading.Tasks;
using Coolector.Common.Dto.Operations;
using Coolector.Common.Types;
using Coolector.Services.Storage.Settings;
using NLog;

namespace Coolector.Services.Storage.Services.Operations
{
    public class OperationServiceClient : IOperationServiceClient
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IServiceClient _serviceClient;
        private readonly ProviderSettings _settings;

        public OperationServiceClient(IServiceClient serviceClient, ProviderSettings settings)
        {
            _serviceClient = serviceClient;
            _settings = settings;
        }

        public async Task<Maybe<OperationDto>> GetAsync(Guid requestId)
        {
            Logger.Debug($"Requesting GetAsync, requestId:{requestId}");
            return await _serviceClient.GetAsync<OperationDto>(_settings.OperationsApiUrl, $"/operations/{requestId}");
        }
    }
}