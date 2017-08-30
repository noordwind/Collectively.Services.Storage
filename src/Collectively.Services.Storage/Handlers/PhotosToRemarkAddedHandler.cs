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
    public class PhotosToRemarkAddedHandler : IEventHandler<PhotosToRemarkAdded>
    {
        private readonly IHandler _handler;
        private readonly IRemarkRepository _remarkRepository;
        private readonly IRemarkServiceClient _remarkServiceClient;
        private readonly ICache _cache;

        public PhotosToRemarkAddedHandler(IHandler handler, 
            IRemarkRepository remarkRepository,
            IRemarkServiceClient remarkServiceClient,
            ICache cache)
        {
            _handler = handler;
            _remarkRepository = remarkRepository;
            _remarkServiceClient = remarkServiceClient;
            _cache = cache;
        }

        public async Task HandleAsync(PhotosToRemarkAdded @event)
            => await _handler
                .Run(async () =>
                {
                    var remark = await _remarkRepository.GetByIdAsync(@event.RemarkId);
                    if (remark.HasNoValue)
                    {
                        return;
                    }

                    var remarkDto = await _remarkServiceClient.GetAsync<Remark>(@event.RemarkId);
                    remark.Value.Status = null;
                    remark.Value.Photos.Clear();
                    foreach(var photo in remarkDto.Value.Photos)
                    {
                        remark.Value.Photos.Add(photo);
                    }
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