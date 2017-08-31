using System.Threading.Tasks;
using Collectively.Messages.Events;
using Collectively.Common.Services;
using Collectively.Messages.Events.Remarks;
using Collectively.Services.Storage.Models.Remarks;
using Collectively.Services.Storage.Repositories;
using Collectively.Services.Storage.ServiceClients;
using Collectively.Common.Caching;

namespace Collectively.Services.Storage.Handlers
{
    public class RemarkCreatedHandler : IEventHandler<RemarkCreated>
    {
        private readonly IHandler _handler;
        private readonly IRemarkRepository _remarkRepository;
        private readonly IRemarkServiceClient _remarkServiceClient;
        private readonly ICache _cache;

        public RemarkCreatedHandler(IHandler handler, 
            IRemarkRepository remarkRepository,
            IRemarkServiceClient remarkServiceClient,
            ICache cache)
        {
            _handler = handler;
            _remarkRepository = remarkRepository;
            _remarkServiceClient = remarkServiceClient;
            _cache = cache;
        }

        public async Task HandleAsync(RemarkCreated @event)
        {
            await _handler
                .Run(async () =>
                {
                    var remark = await _remarkServiceClient.GetAsync<Remark>(@event.RemarkId);
                    remark.Value.Status = null;
                    await _remarkRepository.AddAsync(remark.Value);
                    await _cache.GeoAddAsync("remarks", remark.Value.Location.Longitude, 
                        remark.Value.Location.Latitude, remark.Value.Id.ToString());
                    await _cache.AddAsync($"remarks:{remark.Value.Id}", remark.Value);
                    await _cache.AddToSortedSetAsync("remarks-latest", remark.Value.Id.ToString(), 0, limit: 100);
                })
                .OnError((ex, logger) =>
                {
                    logger.Error(ex, $"Error occured while handling {@event.GetType().Name} event");
                })
                .ExecuteAsync();
        }
    }
}