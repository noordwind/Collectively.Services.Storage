using System;
using System.Threading.Tasks;
using Collectively.Messages.Events;
using Collectively.Common.Services;
using Collectively.Messages.Events.Operations;
using Collectively.Services.Storage.Models.Operations;
using Collectively.Services.Storage.Repositories;

namespace Collectively.Services.Storage.Handlers
{
    public class OperationCreatedHandler : IEventHandler<OperationCreated>
    {
        private readonly IHandler _handler;
        private readonly IOperationRepository _operationRepository;

        public OperationCreatedHandler(IHandler handler, IOperationRepository operationRepository)
        {
            _handler = handler;
            _operationRepository = operationRepository;
        }

        public async Task HandleAsync(OperationCreated @event)
        {
            await _handler
                .Run(async () =>
                {
                    var operation = new Operation
                    {
                        Id = Guid.NewGuid(),
                        RequestId = @event.RequestId,
                        Name = @event.Name,
                        UserId = @event.UserId,
                        Origin = @event.Origin,
                        Resource = @event.Resource,
                        State = @event.State,
                        CreatedAt = @event.CreatedAt
                    };
                    await _operationRepository.AddAsync(operation);
                })
                .OnError((ex, logger) =>
                {
                    logger.Error(ex, $"Error occured while handling {@event.GetType().Name} event");
                })
                .ExecuteAsync();
        }
    }
}