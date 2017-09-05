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
        private readonly IOperationCache _cache;

        public OperationUpdatedHandler(IHandler handler, IOperationCache cache)
        {
            _handler = handler;
            _cache = cache;
        }

        public async Task HandleAsync(OperationUpdated @event)
        {
            await _handler
                .Run(async () =>
                {
                    var operation = await _cache.GetAsync(@event.RequestId);
                    if (operation.HasNoValue)
                        return;

                    operation.Value.State = @event.State;
                    operation.Value.Code = @event.Code;
                    operation.Value.Message = @event.Message;
                    operation.Value.UpdatedAt = @event.UpdatedAt;
                    await _cache.AddAsync(operation.Value);
                })
                .OnError((ex, logger) =>
                {
                    logger.Error(ex, $"Error occured while handling {@event.GetType().Name} event");
                })
                .ExecuteAsync();
        }
    }
}