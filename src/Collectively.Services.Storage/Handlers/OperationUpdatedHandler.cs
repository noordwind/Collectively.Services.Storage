using System.Threading.Tasks;
using Collectively.Messages.Events;
using Collectively.Common.Services;
using Collectively.Messages.Events.Operations;
using Collectively.Services.Storage.Repositories;
using Collectively.Services.Storage.Services;

namespace Collectively.Services.Storage.Handlers
{
    public class OperationUpdatedHandler : IEventHandler<OperationUpdated>
    {
        private readonly IHandler _handler;
        private readonly IOperationService _operationService;

        public OperationUpdatedHandler(IHandler handler, IOperationService operationService)
        {
            _handler = handler;
            _operationService = operationService;
        }

        public async Task HandleAsync(OperationUpdated @event)
        {
            await _handler
                .Run(async () =>
                {
                    var operation = await _operationService.GetAsync(@event.RequestId);
                    if (operation.HasNoValue)
                        return;

                    operation.Value.State = @event.State;
                    operation.Value.Code = @event.Code;
                    operation.Value.Message = @event.Message;
                    operation.Value.UpdatedAt = @event.UpdatedAt;
                    await _operationService.SetAsync(operation.Value);
                })
                .OnError((ex, logger) =>
                {
                    logger.Error(ex, $"Error occured while handling {@event.GetType().Name} event");
                })
                .ExecuteAsync();
        }
    }
}