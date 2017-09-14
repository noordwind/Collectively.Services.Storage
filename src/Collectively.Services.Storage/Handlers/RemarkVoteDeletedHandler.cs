using System.Threading.Tasks;
using Collectively.Messages.Events;
using Collectively.Common.Services;
using Collectively.Messages.Events.Remarks;
using Collectively.Services.Storage.Repositories;
using System.Linq;
using Collectively.Common.Caching;
using Collectively.Services.Storage.Services;

namespace Collectively.Services.Storage.Handlers
{
    public class RemarkVoteDeletedHandler : IEventHandler<RemarkVoteDeleted>
    {
        private readonly IHandler _handler;
        private readonly IRemarkRepository _remarkRepository;
        private readonly IRemarkCache _cache;

        public RemarkVoteDeletedHandler(IHandler handler, 
            IRemarkRepository remarkRepository,
            IRemarkCache cache)
        {
            _handler = handler;
            _remarkRepository = remarkRepository;
            _cache = cache;
        }

        public async Task HandleAsync(RemarkVoteDeleted @event)
        {
            await _handler
                .Run(async () => 
                {
                    var remark = await _remarkRepository.GetByIdAsync(@event.RemarkId);
                    if (remark.HasNoValue)
                    {
                        return;
                    }

                    var vote = remark.Value.Votes
                        .SingleOrDefault(x => x.UserId == @event.UserId);

                    if (vote.Positive)
                    {
                        remark.Value.Rating--;
                        remark.Value.PositiveVotesCount--;
                    }
                    else
                    {
                        remark.Value.Rating++;
                        remark.Value.NegativeVotesCount--;
                    }
                    remark.Value.Votes.Remove(vote);
                    await _remarkRepository.UpdateAsync(remark.Value);
                    await _cache.AddAsync(remark.Value);
                })
                .ExecuteAsync();
        }
    }
}