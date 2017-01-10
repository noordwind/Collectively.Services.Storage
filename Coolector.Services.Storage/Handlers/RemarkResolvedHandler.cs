using System.Threading.Tasks;
using Coolector.Common.Events;
using Coolector.Services.Storage.Repositories;
using System.Linq;
using Coolector.Common.Services;
using Coolector.Services.Remarks.Shared.Dto;
using Coolector.Services.Remarks.Shared.Events;

namespace Coolector.Services.Storage.Handlers
{
    public class RemarkResolvedHandler : IEventHandler<RemarkResolved>
    {
        private readonly IHandler _handler;
        private readonly IRemarkRepository _remarkRepository;
        private readonly IUserRepository _userRepository;

        public RemarkResolvedHandler(IHandler handler, 
            IRemarkRepository remarkRepository,
            IUserRepository userRepository)
        {
            _handler = handler;
            _remarkRepository = remarkRepository;
            _userRepository = userRepository;
        }

        public async Task HandleAsync(RemarkResolved @event)
        {
            await _handler
                .Run(async () =>
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
                })
                .OnError((ex, logger) =>
                {
                    logger.Error(ex, $"Error occured while handling {@event.GetType().Name} event");
                })
                .ExecuteAsync();
        }
    }
}