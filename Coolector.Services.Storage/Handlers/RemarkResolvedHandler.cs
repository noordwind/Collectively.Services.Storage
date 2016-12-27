using System.Threading.Tasks;
using Coolector.Common.Events;
using Coolector.Services.Storage.Repositories;
using System.Linq;
using Coolector.Services.Remarks.Shared.Dto;
using Coolector.Services.Remarks.Shared.Events;

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

            remark.Value.Photos = @event.Photos.Select(x => new FileDto
            {
                GroupId = x.GroupId,
                Name = x.Name,
                Size = x.Size,
                Url = x.Url,
                Metadata = x.Metadata
            }).ToList();
            remark.Value.Resolved = true;
            remark.Value.ResolvedAt = @event.ResolvedAt;
            remark.Value.Resolver = new RemarkAuthorDto
            {
                UserId = user.Value.UserId,
                Name = user.Value.Name
            };
            await _remarkRepository.UpdateAsync(remark.Value);
        }
    }
}