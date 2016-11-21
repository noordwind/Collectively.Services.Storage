using System.Threading.Tasks;
using Coolector.Common.Events;
using Coolector.Common.Events.Operations;
using Coolector.Services.Storage.Repositories;

namespace Coolector.Services.Storage.Handlers
{
    public class OperationUpdatedHandler : IEventHandler<OperationUpdated>
    {
        private readonly IOperationRepository _operationRepository;

        public OperationUpdatedHandler(IOperationRepository operationRepository)
        {
            _operationRepository = operationRepository;
        }

        public async Task HandleAsync(OperationUpdated @event)
        {
            var operation = await _operationRepository.GetAsync(@event.RequestId);
            if (operation.HasNoValue)
                return;

            operation.Value.State = @event.State;
            operation.Value.Code = @event.Code;
            operation.Value.Message = @event.Message;
            operation.Value.UpdatedAt = @event.UpdatedAt;
            await _operationRepository.UpdateAsync(operation.Value);
        }
    }
}