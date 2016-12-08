using System;
using System.Threading.Tasks;
using Coolector.Common.Events;
using Coolector.Services.Storage.Repositories;

namespace Coolector.Services.Storage.Handlers
{
    public class OperationCreatedHandler : IEventHandler<OperationCreated>
    {
        private readonly IOperationRepository _operationRepository;

        public OperationCreatedHandler(IOperationRepository operationRepository)
        {
            _operationRepository = operationRepository;
        }

        public async Task HandleAsync(OperationCreated @event)
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
        }
    }
}