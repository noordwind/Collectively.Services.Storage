using System.Threading.Tasks;
using Coolector.Common.Events;
using Coolector.Common.Services;
using Coolector.Services.Remarks.Shared.Events;
using Coolector.Services.Storage.Repositories;

namespace Coolector.Services.Storage.Handlers
{
    public class RemarkDeletedHandler : IEventHandler<RemarkDeleted>
    {
        private readonly IHandler _handler;
        private readonly IRemarkRepository _repository;

        public RemarkDeletedHandler(IHandler handler, 
            IRemarkRepository repository)
        {
            _handler = handler;
            _repository = repository;
        }

        public async Task HandleAsync(RemarkDeleted @event)
        {
            await _handler
                .Run(async () =>
                {
                    var remark = await _repository.GetByIdAsync(@event.Id);
                    if (remark.HasNoValue)
                        return;

                    await _repository.DeleteAsync(remark.Value);
                })
                .OnError((ex, logger) =>
                {
                    logger.Error(ex, $"Error occured while handling {@event.GetType().Name} event");
                })
                .ExecuteAsync();
        }
    }
}