using System.Threading.Tasks;
using Collectively.Messages.Events;
using Collectively.Services.Storage.Repositories;
using Collectively.Common.Services;
using Collectively.Messages.Events.Remarks;
using Collectively.Services.Storage.Models.Remarks;
using Collectively.Services.Storage.ServiceClients;
using Collectively.Common.Caching;

namespace Collectively.Services.Storage.Handlers
{
    public class RemarkCanceledHandler : IEventHandler<RemarkCanceled>
    {
        private readonly IHandler _handler;
        private readonly IRemarkRepository _remarkRepository;
        private readonly IRemarkServiceClient _remarkServiceClient;
        private readonly ICache _cache;

        public RemarkCanceledHandler(IHandler handler, 
            IRemarkRepository remarkRepository,
            IRemarkServiceClient remarkServiceClient,
            ICache cache)
        {
            _handler = handler;
            _remarkRepository = remarkRepository;
            _remarkServiceClient = remarkServiceClient;
            _cache = cache;
        }

        public async Task HandleAsync(RemarkCanceled @event)
        => await _handler
            .Run(async () =>
            {
                var remark = await _remarkRepository.GetByIdAsync(@event.RemarkId);
                if (remark.HasNoValue)
                    return;

                var remarkDto = await _remarkServiceClient.GetAsync<Remark>(@event.RemarkId);
                remark.Value.State = remarkDto.Value.State;
                remark.Value.States = remarkDto.Value.States;
                remark.Value.Photos = remarkDto.Value.Photos;
                remark.Value.Resolved = false;
                await _remarkRepository.UpdateAsync(remark.Value);
                await _cache.GeoRemoveAsync("remarks", remark.Value.Id.ToString());
                await _cache.DeleteAsync($"remarks:{remark.Value.Id}");
                await _cache.RemoveFromSortedSetAsync("remarks-latest", remark.Value.Id.ToString());
            })
            .OnError((ex, logger) =>
            {
                logger.Error(ex, $"Error occured while handling {@event.GetType().Name} event");
            })
            .ExecuteAsync();
    }
}