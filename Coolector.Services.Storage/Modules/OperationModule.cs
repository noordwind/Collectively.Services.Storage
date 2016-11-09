using Coolector.Dto.Operations;
using Coolector.Services.Storage.Providers;
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