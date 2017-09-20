using System.Collections.Generic;
using System.Threading.Tasks;
using Collectively.Messages.Events;
using Collectively.Common.Services;
using Collectively.Messages.Events.Remarks;
using Collectively.Services.Storage.Repositories;
using System.Linq;
using Collectively.Services.Storage.Models.Remarks;
using Collectively.Common.Caching;
using Collectively.Services.Storage.Services;

namespace Collectively.Services.Storage.Handlers
{
    public class RemarkVoteSubmittedHandler : IEventHandler<RemarkVoteSubmitted>
    {
        private readonly IHandler _handler;
        private readonly IRemarkRepository _remarkRepository;
        private readonly IRemarkCache _cache;

        public RemarkVoteSubmittedHandler(IHandler handler, 
            IRemarkRepository remarkRepository,
            IRemarkCache cache)
        {
            _handler = handler;
            _remarkRepository = remarkRepository;
            _cache = cache;
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
                    remark.Value.Votes.Add(new Vote
                    {
                        UserId = @event.UserId,
                        Positive = @event.Positive,
                        CreatedAt = @event.CreatedAt
                    });
                    await _remarkRepository.UpdateAsync(remark.Value);
                    await _cache.AddAsync(remark.Value);
                })
                .ExecuteAsync();
        }

        private void Vote(Remark remark, string userId, bool positive)
        {
            if (remark.Votes == null)
            {
                remark.Votes = new List<Vote>();
            }

            var vote = remark.Votes.SingleOrDefault(x => x.UserId == userId);
            if (vote != null)
            {
                if (vote.Positive)
                {
                    remark.Rating--;
                    remark.PositiveVotesCount--;
                }
                else
                {
                    remark.Rating++;
                    remark.NegativeVotesCount--;
                }
                remark.Votes.Remove(vote);
            }
            if (positive)
            {
                remark.Rating++;
                remark.PositiveVotesCount++;
            }
            else
            {
                remark.Rating--;
                remark.NegativeVotesCount++;
            }
        }
    }
}