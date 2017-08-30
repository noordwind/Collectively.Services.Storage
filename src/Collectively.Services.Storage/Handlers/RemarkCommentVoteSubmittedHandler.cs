using System.Collections.Generic;
using System.Threading.Tasks;
using Collectively.Messages.Events;
using Collectively.Common.Services;
using Collectively.Messages.Events.Remarks;
using Collectively.Services.Storage.Repositories;
using System.Linq;
using Collectively.Services.Storage.Models.Remarks;
using Collectively.Common.Caching;

namespace Collectively.Services.Storage.Handlers
{
    public class RemarkCommentVoteSubmittedHandler : IEventHandler<RemarkCommentVoteSubmitted>
    {
        private readonly IHandler _handler;
        private readonly IRemarkRepository _remarkRepository;
        private readonly ICache _cache;

        public RemarkCommentVoteSubmittedHandler(IHandler handler, 
            IRemarkRepository remarkRepository,
            ICache cache)
        {
            _handler = handler;
            _remarkRepository = remarkRepository;
            _cache = cache;
        }

        public async Task HandleAsync(RemarkCommentVoteSubmitted @event)
        {
            await _handler
                .Run(async () => 
                {
                    var remark = await _remarkRepository.GetByIdAsync(@event.RemarkId);
                    if (remark.HasNoValue)
                    {
                        return;
                    }

                    var comment = remark.Value.Comments.SingleOrDefault(x => x.Id == @event.CommentId);
                    if(comment == null)
                    {
                        return;
                    }
                    Vote(comment, @event.UserId, @event.Positive);
                    comment.Votes.Add(new Vote
                    {
                        UserId = @event.UserId,
                        Positive = @event.Positive,
                        CreatedAt = @event.CreatedAt
                    });
                    await _remarkRepository.UpdateAsync(remark.Value);
                    await _cache.AddAsync($"remarks:{remark.Value.Id}", remark.Value);
                })
                .ExecuteAsync();
        }

        private void Vote(Comment comment, string userId, bool positive)
        {
            var vote = comment.Votes.SingleOrDefault(x => x.UserId == userId);
            if (vote != null)
            {
                if (vote.Positive)
                {
                    comment.Rating--;
                }
                else
                {
                    comment.Rating++;
                }
                comment.Votes.Remove(vote);
            }
            if (positive)
            {
                comment.Rating++;
            }
            else
            {
                comment.Rating--;
            }
        }
    }
}