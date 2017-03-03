using System.Threading.Tasks;
using System.Linq;
using Collectively.Messages.Events;
using Collectively.Common.Services;
using Collectively.Messages.Events.Remarks;
using Collectively.Services.Storage.Repositories;

namespace Collectively.Services.Storage.Handlers
{
    public class PhotosFromRemarkRemovedHandler : IEventHandler<PhotosFromRemarkRemoved>
    {
        private readonly IHandler _handler;
        private readonly IRemarkRepository _remarkRepository;

        public PhotosFromRemarkRemovedHandler(IHandler handler, IRemarkRepository remarkRepository)
        {
            _handler = handler;
            _remarkRepository = remarkRepository;
        }

        public async Task HandleAsync(PhotosFromRemarkRemoved @event)
        {
            await _handler
                .Run(async () =>
                {
                    var remark = await _remarkRepository.GetByIdAsync(@event.RemarkId);
                    if (remark.HasNoValue)
                        return;

                    foreach (var name in @event.Photos)
                    {
                        var photo = remark.Value.Photos.FirstOrDefault(x => x.Name == name);
                        if (photo != null)
                        {
                            remark.Value.Photos.Remove(photo);
                        }
                    }
                    await _remarkRepository.UpdateAsync(remark.Value);
                })
                .OnError((ex, logger) =>
                {
                    logger.Error(ex, $"Error occured while handling {@event.GetType().Name} event");
                })
                .ExecuteAsync();
        }
    }
}