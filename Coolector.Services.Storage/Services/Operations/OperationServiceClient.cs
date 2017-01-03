using System;
using System.Threading.Tasks;
using Coolector.Common.Security;
using Coolector.Common.Types;
using Coolector.Services.Operations.Shared.Dto;
using NLog;

namespace Coolector.Services.Storage.Services.Operations
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

        public async Task<Maybe<OperationDto>> GetAsync(Guid requestId)
        {
            Logger.Debug($"Requesting GetAsync, requestId:{requestId}");
            return await _serviceClient.GetAsync<OperationDto>(_settings.Url, $"/operations/{requestId}");
        }
    }
}