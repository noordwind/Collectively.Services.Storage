using System.Threading.Tasks;
using System.Linq;
using Coolector.Common.Events;
using Coolector.Services.Remarks.Shared.Events;
using Coolector.Services.Storage.Repositories;
using Coolector.Common.Dto.General;

namespace Coolector.Services.Storage.Handlers
{
    public class PhotosToRemarkAddedHandler : IEventHandler<PhotosToRemarkAdded>
    {
        private readonly IRemarkRepository _remarkRepository;

        public PhotosToRemarkAddedHandler(IRemarkRepository remarkRepository)
        {
            _remarkRepository = remarkRepository;
        }

        public async Task HandleAsync(PhotosToRemarkAdded @event)
        {
            var remark = await _remarkRepository.GetByIdAsync(@event.RemarkId);
            if (remark.HasNoValue)
                return;

            remark.Value.Photos = @event.Photos.Select(x => new FileDto
            {
                Name = x.Name,
                Size = x.Size,
                Url = x.Url,
                Metadata = x.Metadata
            }).ToList();
            await _remarkRepository.UpdateAsync(remark.Value);
        }
    }
}