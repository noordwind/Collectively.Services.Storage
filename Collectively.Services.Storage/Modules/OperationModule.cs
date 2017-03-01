
using Collectively.Services.Storage.Providers.Operations;
using Collectively.Services.Storage.Queries;

namespace Collectively.Services.Storage.Modules
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