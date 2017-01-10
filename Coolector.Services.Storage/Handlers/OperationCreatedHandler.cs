using System;
using System.Threading.Tasks;
using Coolector.Common.Events;
using Coolector.Common.Services;
using Coolector.Services.Operations.Shared.Dto;
using Coolector.Services.Operations.Shared.Events;
using Coolector.Services.Storage.Repositories;

namespace Coolector.Services.Storage.Handlers
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
                    var operation = new OperationDto
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