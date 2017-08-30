using System;
using System.Threading.Tasks;
using Collectively.Common.Services;
using Collectively.Services.Storage.Repositories;
using Collectively.Messages.Events;
using Collectively.Messages.Events.Remarks;
using Collectively.Common.Caching;

namespace Collectively.Services.Storage.Handlers
{
    public class AddPhotosToRemarkRejectedHandler : IEventHandler<AddPhotosToRemarkRejected>
    {
        private readonly IHandler _handler;
        private readonly IRemarkRepository _remarkRepository;
        private readonly ICache _cache;

        public AddPhotosToRemarkRejectedHandler(IHandler handler, 
            IRemarkRepository remarkRepository,
            ICache cache)
        {
            _handler = handler;
            _remarkRepository = remarkRepository;
            _cache = cache;
        }

        public async Task HandleAsync(AddPhotosToRemarkRejected @event)
            => await _handler
                .Run(async () =>
                {
                    var remark = await _remarkRepository.GetByIdAsync(@event.RemarkId);
                    remark.Value.Status = null;
                    await _remarkRepository.UpdateAsync(remark.Value);
                    await _cache.AddAsync($"remarks:{remark.Value.Id}", remark.Value);
                })
                .OnError((ex, logger) =>
                {
                    logger.Error(ex, $"Error occured while handling {@event.GetType().Name} event");
                })
                .ExecuteAsync();
    }
}