using System.Threading.Tasks;
using System.Linq;
using Collectively.Messages.Events;
using Collectively.Common.Services;
using Collectively.Messages.Events.Remarks;
using Collectively.Services.Storage.Models.Remarks;
using Collectively.Services.Storage.Repositories;


namespace Collectively.Services.Storage.Handlers
{
    public class PhotosToRemarkAddedHandler : IEventHandler<PhotosToRemarkAdded>
    {
        private readonly IHandler _handler;
        private readonly IRemarkRepository _remarkRepository;

        public PhotosToRemarkAddedHandler(IHandler handler, IRemarkRepository remarkRepository)
        {
            _handler = handler;
            _remarkRepository = remarkRepository;
        }

        public async Task HandleAsync(PhotosToRemarkAdded @event)
        {
            await _handler
                .Run(async () =>
                {
                    var remark = await _remarkRepository.GetByIdAsync(@event.RemarkId);
                    if (remark.HasNoValue)
                        return;

                    remark.Value.Photos = @event.Photos.Select(x => new File
                    {
                        GroupId = x.GroupId,
                        Name = x.Name,
                        Size = x.Size,
                        Url = x.Url,
                        Metadata = x.Metadata
                    }).ToList();
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