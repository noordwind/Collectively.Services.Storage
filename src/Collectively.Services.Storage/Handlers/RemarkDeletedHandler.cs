using System.Threading.Tasks;
using Collectively.Messages.Events;
using Collectively.Common.Services;
using Collectively.Messages.Events.Remarks;
using Collectively.Services.Storage.Repositories;
using Collectively.Common.Caching;
using Collectively.Services.Storage.Services;

namespace Collectively.Services.Storage.Handlers
{
    public class RemarkDeletedHandler : IEventHandler<RemarkDeleted>
    {
        private readonly IHandler _handler;
        private readonly IRemarkRepository _repository;
        private readonly IRemarkCache _cache;

        public RemarkDeletedHandler(IHandler handler, 
            IRemarkRepository repository,
            IRemarkCache cache)
        {
            _handler = handler;
            _repository = repository;
            _cache = cache;
        }

        public async Task HandleAsync(RemarkDeleted @event)
        {
            await _handler
                .Run(async () =>
                {
                    var remark = await _repository.GetByIdAsync(@event.RemarkId);
                    if (remark.HasNoValue)
                        return;

                    await _repository.DeleteAsync(remark.Value);
                    await _cache.DeleteAsync(@event.RemarkId, deleteGeo: true, deleteLatest: true);
                })
                .OnError((ex, logger) =>
                {
                    logger.Error(ex, $"Error occured while handling {@event.GetType().Name} event");
                })
                .ExecuteAsync();
        }
    }
}