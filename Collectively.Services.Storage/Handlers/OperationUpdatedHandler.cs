using System.Threading.Tasks;
using Collectively.Common.Events;
using Collectively.Common.Services;
using Collectively.Messages.Events.Operations;
using Collectively.Services.Storage.Repositories;

namespace Collectively.Services.Storage.Handlers
{
    public class OperationUpdatedHandler : IEventHandler<OperationUpdated>
    {
        private readonly IHandler _handler;
        private readonly IOperationRepository _operationRepository;

        public OperationUpdatedHandler(IHandler handler, IOperationRepository operationRepository)
        {
            _handler = handler;
            _operationRepository = operationRepository;
        }

        public async Task HandleAsync(OperationUpdated @event)
        {
            await _handler
                .Run(async () =>
                {
                    var operation = await _operationRepository.GetAsync(@event.RequestId);
                    if (operation.HasNoValue)
                        return;

                    operation.Value.State = @event.State;
                    operation.Value.Code = @event.Code;
                    operation.Value.Message = @event.Message;
                    operation.Value.UpdatedAt = @event.UpdatedAt;
                    await _operationRepository.UpdateAsync(operation.Value);
                })
                .OnError((ex, logger) =>
                {
                    logger.Error(ex, $"Error occured while handling {@event.GetType().Name} event");
                })
                .ExecuteAsync();
        }
    }
}