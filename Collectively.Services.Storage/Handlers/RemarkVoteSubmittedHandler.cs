using System.Collections.Generic;
using System.Threading.Tasks;
using Collectively.Common.Events;
using Collectively.Common.Services;
using Collectively.Messages.Events.Remarks;
using Collectively.Services.Storage.Repositories;

using System.Linq;

namespace Collectively.Services.Storage.Handlers
{
    public class RemarkVoteSubmittedHandler : IEventHandler<RemarkVoteSubmitted>
    {
        private readonly IHandler _handler;
        private readonly IRemarkRepository _remarkRepository;

        public RemarkVoteSubmittedHandler(IHandler handler, IRemarkRepository remarkRepository)
        {
            _handler = handler;
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
                    Vote(remark.Value, @event.UserId, @event.Positive);
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

        private void Vote(RemarkDto remark, string userId, bool positive)
        {
            if (remark.Votes == null)
                remark.Votes = new List<VoteDto>();

            var vote = remark.Votes.SingleOrDefault(x => x.UserId == userId);
            if (vote != null)
            {
                if (vote.Positive)
                {
                    remark.Rating--;
                }
                else
                {
                    remark.Rating++;
                }
                remark.Votes.Remove(vote);
            }
            if (positive)
            {
                remark.Rating++;
            }
            else
            {
                remark.Rating--;
            }
        }
    }
}