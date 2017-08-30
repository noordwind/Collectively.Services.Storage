using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Collectively.Common.Caching;
using Collectively.Common.Services;
using Collectively.Messages.Events;
using Collectively.Messages.Events.Remarks;
using Collectively.Services.Storage.Models.Remarks;
using Collectively.Services.Storage.Repositories;

namespace Collectively.Services.Storage.Handlers
{
    public class CommentDeletedFromRemarkHandler : IEventHandler<CommentDeletedFromRemark>
    {
        private readonly IHandler _handler;
        private readonly IRemarkRepository _repository;
        private readonly ICache _cache;

        public CommentDeletedFromRemarkHandler(IHandler handler, 
            IRemarkRepository repository,
            ICache cache)
        {
            _handler = handler;
            _repository = repository;
            _cache = cache;
        }

        public async Task HandleAsync(CommentDeletedFromRemark @event)
        {
            await _handler
                .Run(async () =>
                {
                    var remark = await _repository.GetByIdAsync(@event.RemarkId);
                    if (remark.HasNoValue)
                    {
                        return;
                    }
                    
                    var comment = remark.Value.Comments.SingleOrDefault(x => x.Id == @event.CommentId);
                    if(comment == null)
                    {
                        return;
                    }
                    comment.History.Clear();
                    comment.Text = string.Empty;
                    comment.Removed = true;
                    await _repository.UpdateAsync(remark.Value);
                    await _cache.AddAsync($"remarks:{remark.Value.Id}", remark.Value);
                })
                .OnError((ex, logger) =>
                {
                    logger.Error(ex, $"Error occured while handling {@event.GetType().Name} event");
                })
                .ExecuteAsync();
        }
    }
}