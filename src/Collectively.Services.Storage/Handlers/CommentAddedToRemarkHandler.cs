using System;
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
    public class CommentAddedToRemarkHandler : IEventHandler<CommentAddedToRemark>
    {
        private readonly IHandler _handler;
        private readonly IRemarkRepository _repository;
        private readonly IRemarkCache _cache;

        public CommentAddedToRemarkHandler(IHandler handler, 
            IRemarkRepository repository,
            IRemarkCache cache)
        {
            _handler = handler;
            _repository = repository;
            _cache = cache;
        }

        public async Task HandleAsync(CommentAddedToRemark @event)
        {
            await _handler
                .Run(async () =>
                {
                    var remark = await _repository.GetByIdAsync(@event.RemarkId);
                    if (remark.HasNoValue)
                    {
                        return;
                    }
                    remark.Value.UpdatedAt = DateTime.UtcNow;
                    if(remark.Value.Comments == null)
                    {
                        remark.Value.Comments = new List<Comment>();
                    }
                    remark.Value.Comments.Add(new Comment
                    {
                        Id = @event.CommentId,
                        Text = @event.Text,
                        User = new RemarkUser
                        {
                            UserId = @event.UserId,
                            Name = @event.Username
                        },
                        CreatedAt = @event.CreatedAt,
                        Votes = new List<Vote>(),
                        History = new List<CommentHistory>()
                    });
                    await _repository.UpdateAsync(remark.Value);
                    await _cache.AddAsync(remark.Value);
                })
                .OnError((ex, logger) =>
                {
                    logger.Error(ex, $"Error occured while handling {@event.GetType().Name} event");
                })
                .ExecuteAsync();
        }
    }
}