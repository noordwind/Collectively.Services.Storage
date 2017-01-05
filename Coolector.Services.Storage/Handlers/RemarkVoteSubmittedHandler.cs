using System.Threading.Tasks;
using Coolector.Common.Events;
using Coolector.Common.Services;
using Coolector.Services.Remarks.Shared.Events;
using RawRabbit;
using Coolector.Services.Storage.Repositories;
using Coolector.Services.Remarks.Shared.Dto;
using System.Collections.Generic;

namespace Coolector.Services.Storage.Handlers
{
    public class RemarkVoteSubmittedHandler : IEventHandler<RemarkVoteSubmitted>
    {
        private readonly IHandler _handler;
        private readonly IBusClient _bus;
        private readonly IRemarkRepository _remarkRepository;

        public RemarkVoteSubmittedHandler(IHandler handler, IBusClient bus, IRemarkRepository remarkRepository)
        {
            _handler = handler;
            _bus = bus;
            _remarkRepository = remarkRepository;
        }

        public async Task HandleAsync(RemarkVoteSubmitted @event)
        {
            await _handler
                .Run(async () => 
                {
                    var remark = await _remarkRepository.GetByIdAsync(@event.RemarkId);
                    if (remark.HasNoValue)
                    {
                        return;
                    }
                    
                    remark.Value.Votes.Add(new VoteDto
                    {
                        UserId = @event.UserId,
                        Positive = @event.Positive,
                        CreatedAt = @event.CreatedAt
                    });
                    await _remarkRepository.UpdateAsync(remark.Value);
                })
                .ExecuteAsync();
        }
    }
}