using System.IO;
using System.Threading.Tasks;
using Coolector.Common.Events;
using Coolector.Common.Events.Remarks;
using Coolector.Dto.Common;
using Coolector.Dto.Remarks;
using Coolector.Services.Storage.Repositories;
using System.Linq;

namespace Coolector.Services.Storage.Handlers
{
    public class RemarkResolvedHandler : IEventHandler<RemarkResolved>
    {
        private readonly IRemarkRepository _remarkRepository;
        private readonly IUserRepository _userRepository;

        public RemarkResolvedHandler(IRemarkRepository remarkRepository,
            IUserRepository userRepository)
        {
            _remarkRepository = remarkRepository;
            _userRepository = userRepository;
        }

        public async Task HandleAsync(RemarkResolved @event)
        {
            var remark = await _remarkRepository.GetByIdAsync(@event.RemarkId);
            if (remark.HasNoValue)
                return;

            var user = await _userRepository.GetByIdAsync(@event.UserId);
            if (user.HasNoValue)
                return;

            var photos = @event.Photos.Select(x => new FileDto
            {
                Size = x.Size,
                Url = x.Url,
                Metadata = x.Metadata
            });

            remark.Value.Resolved = true;
            remark.Value.ResolvedAt = @event.ResolvedAt;
            remark.Value.Resolver = new RemarkAuthorDto
            {
                UserId = user.Value.UserId,
                Name = user.Value.Name
            };
            remark.Value.Photos = photos.ToList();
            await _remarkRepository.UpdateAsync(remark.Value);
        }
    }
}