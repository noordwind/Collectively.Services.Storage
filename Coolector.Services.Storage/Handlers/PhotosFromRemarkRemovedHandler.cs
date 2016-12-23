using System.Threading.Tasks;
using System.Linq;
using Coolector.Common.Events;
using Coolector.Services.Remarks.Shared.Events;
using Coolector.Services.Storage.Repositories;

namespace Coolector.Services.Storage.Handlers
{
    public class PhotosFromRemarkRemovedHandler : IEventHandler<PhotosFromRemarkRemoved>
    {
        private readonly IRemarkRepository _remarkRepository;

        public PhotosFromRemarkRemovedHandler(IRemarkRepository remarkRepository)
        {
            _remarkRepository = remarkRepository;
        }

        public async Task HandleAsync(PhotosFromRemarkRemoved @event)
        {
            var remark = await _remarkRepository.GetByIdAsync(@event.RemarkId);
            if (remark.HasNoValue)
                return;

            foreach(var name in @event.Photos)
            {
                var photo = remark.Value.Photos.FirstOrDefault(x => x.Name == name);
                if(photo != null)
                {
                    remark.Value.Photos.Remove(photo);
                }
            }
            await _remarkRepository.UpdateAsync(remark.Value);
        }
    }
}