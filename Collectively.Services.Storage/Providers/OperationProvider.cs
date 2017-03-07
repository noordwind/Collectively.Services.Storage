using System;
using System.Threading.Tasks;
using Collectively.Services.Storage.ServiceClients;
using Collectively.Common.Types;
using Collectively.Services.Storage.Models.Operations;
using Collectively.Services.Storage.Repositories;

namespace Collectively.Services.Storage.Providers
{
    public class OperationProvider : IOperationProvider
    {
        private readonly IProviderClient _provider;
        private readonly IOperationRepository _operationRepository;
        private readonly IOperationServiceClient _serviceClient;

        public OperationProvider(IProviderClient provider,
            IOperationRepository operationRepository,
            IOperationServiceClient serviceClient)
        {
            _provider = provider;
            _operationRepository = operationRepository;
            _serviceClient = serviceClient;
        }

        public async Task<Maybe<Operation>> GetAsync(Guid requestId)
            => await _provider.GetAsync(
                async () => await _operationRepository.GetAsync(requestId),
                async () => await _serviceClient.GetAsync<Operation>(requestId));
    }
}