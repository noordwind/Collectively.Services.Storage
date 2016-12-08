using Coolector.Services.Operations.Shared.Dto;
using Coolector.Services.Storage.Providers.Operations;
using Coolector.Services.Storage.Queries;

namespace Coolector.Services.Storage.Modules
{
    public class OperationModule : ModuleBase
    {
        public OperationModule(IOperationProvider operationProvider) : base("operations")
        {
            Get("{requestId}", args => Fetch<GetOperation, OperationDto>
                (async x => await operationProvider.GetAsync(x.RequestId)).HandleAsync());
        }
    }
}