using System.Collections.Generic;
using System.Threading.Tasks;
using Collectively.Common.Caching;
using Collectively.Common.Services;
using Collectively.Messages.Events;
using Collectively.Messages.Events.Remarks;
using Collectively.Services.Storage.Models.Remarks;
using Collectively.Services.Storage.Repositories;
using Collectively.Services.Storage.Services;

namespace Collectively.Services.Storage.Handlers
{
    public class RemarkActionTakenHandler : IEventHandler<RemarkActionTaken>
    {
        private readonly IHandler _handler;
        private readonly IRemarkRepository _remarkRepository;
        private readonly IRemarkCache _cache;

        public RemarkActionTakenHandler(IHandler handler, 
            IRemarkRepository remarkRepository,
            IRemarkCache cache)
        {
            _handler = handler;
            _remarkRepository = remarkRepository;
            _cache = cache;
        }

        public async Task HandleAsync(RemarkActionTaken @event)
        {
            await _handler
                .Run(async () => 
                {
                    var remark = await _remarkRepository.GetByIdAsync(@event.RemarkId);
                    if (remark.HasNoValue)
                    {
                        return;
                    }

                    var participant = new Participant
                    {
                        User = new RemarkUser
                        {
                            UserId = @event.UserId,
                            Name = @event.Username
                        },
                        Description = @event.Description,
                        CreatedAt = @event.CreatedAt
                    };
                    if (remark.Value.Participants == null)
                    {
                        remark.Value.Participants = new HashSet<Participant>();
                    }
                    remark.Value.Participants.Add(participant);
                    remark.Value.ParticipantsCount++;
                    await _remarkRepository.UpdateAsync(remark.Value);
                    await _cache.AddAsync(remark.Value);
                })
                .ExecuteAsync();
        }
    }
}