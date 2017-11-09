using System;
using System.Linq;
using System.Threading.Tasks;
using Collectively.Common.Caching;
using Collectively.Common.Services;
using Collectively.Messages.Events;
using Collectively.Messages.Events.Remarks;
using Collectively.Services.Storage.Repositories;
using Collectively.Services.Storage.Services;

namespace Collectively.Services.Storage.Handlers
{
    public class RemarkActionCanceledHandler : IEventHandler<RemarkActionCanceled>
    {
        private readonly IHandler _handler;
        private readonly IRemarkRepository _remarkRepository;
        private readonly IRemarkCache _cache;

        public RemarkActionCanceledHandler(IHandler handler, 
            IRemarkRepository remarkRepository,
            IRemarkCache cache)
        {
            _handler = handler;
            _remarkRepository = remarkRepository;
            _cache = cache;
        }

        public async Task HandleAsync(RemarkActionCanceled @event)
        {
            await _handler
                .Run(async () => 
                {
                    var remark = await _remarkRepository.GetByIdAsync(@event.RemarkId);
                    if (remark.HasNoValue)
                    {
                        return;
                    }

                    var participant = remark.Value.Participants.SingleOrDefault(x => x.User.UserId == @event.UserId);
                    if (participant == null)
                    {
                        return;
                    }
                    remark.Value.UpdatedAt = DateTime.UtcNow;
                    remark.Value.Participants.Remove(participant);
                    remark.Value.ParticipantsCount--;
                    await _remarkRepository.UpdateAsync(remark.Value);
                    await _cache.AddAsync(remark.Value);
                })
                .ExecuteAsync();
        }
    }
}