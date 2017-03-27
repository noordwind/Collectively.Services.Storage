using System.Threading.Tasks;
using Collectively.Messages.Events;
using Collectively.Common.Services;
using Collectively.Messages.Events.Remarks;
using Collectively.Services.Storage.Repositories;
using System.Linq;

namespace Collectively.Services.Storage.Handlers
{
    public class RemarkCommentVoteDeletedHandler : IEventHandler<RemarkCommentVoteDeleted>
    {
        private readonly IHandler _handler;
        private readonly IRemarkRepository _remarkRepository;

        public RemarkCommentVoteDeletedHandler(IHandler handler, IRemarkRepository remarkRepository)
        {
            _handler = handler;
            _remarkRepository = remarkRepository;
        }

        public async Task HandleAsync(RemarkCommentVoteDeleted @event)
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
                    
                    var vote = comment.Votes.SingleOrDefault(x => x.UserId == @event.UserId);
                    if (vote.Positive)
                    {
                        comment.Rating--;
                    }
                    else
                    {
                        comment.Rating++;
                    }
                    comment.Votes.Remove(vote);
                    await _remarkRepository.UpdateAsync(remark.Value);
                })
                .ExecuteAsync();
        }
    }
}