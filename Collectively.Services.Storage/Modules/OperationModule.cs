using Collectively.Common.ServiceClients.Queries;
using Collectively.Services.Storage.Models.Operations;
using Collectively.Services.Storage.Providers.Operations;

namespace Collectively.Services.Storage.Modules
{
    public class OperationModule : ModuleBase
    {
        public OperationModule(IOperationProvider operationProvider) : base("operations")
        {
            Get("{requestId}", args => Fetch<GetOperation, Operation>
                (async x => await operationProvider.GetAsync(x.RequestId)).HandleAsync());
        }
    }
}