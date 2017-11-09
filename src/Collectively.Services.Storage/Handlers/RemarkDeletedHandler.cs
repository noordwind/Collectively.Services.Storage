using System.Threading.Tasks;
using Collectively.Messages.Events;
using Collectively.Common.Services;
using Collectively.Messages.Events.Remarks;
using Collectively.Services.Storage.Repositories;
using Collectively.Common.Caching;
using Collectively.Services.Storage.Services;
using System.Linq;

namespace Collectively.Services.Storage.Handlers
{
    public class RemarkDeletedHandler : IEventHandler<RemarkDeleted>
    {
        private readonly IHandler _handler;
        private readonly IRemarkRepository _repository;
        private readonly IGroupRemarkRepository _groupRemarkRepository;
        private readonly IRemarkCache _remarkCache;
        private readonly IUserCache _userCache;

        public RemarkDeletedHandler(IHandler handler, 
            IRemarkRepository repository,
            IGroupRemarkRepository groupRemarkRepository,
            IRemarkCache remarkCache,
            IUserCache userCache)
        {
            _handler = handler;
            _repository = repository;
            _groupRemarkRepository = groupRemarkRepository;
            _remarkCache = remarkCache;
            _userCache = userCache;
        }

        public async Task HandleAsync(RemarkDeleted @event)
            => await _handler
                .Run(async () =>
                {
                    var remark = await _repository.GetByIdAsync(@event.RemarkId);
                    if (remark.HasNoValue)
                    {
                        return;
                    }

                    await _repository.DeleteAsync(remark.Value);
                    await _groupRemarkRepository.DeleteAllForRemarkAsync(@event.RemarkId);
                    await _remarkCache.DeleteAsync(@event.RemarkId, deleteGeo: true, deleteLatest: true);
                    await _userCache.DeleteRemarkAsync(remark.Value.Author.UserId, @event.RemarkId);
                })
                .OnError((ex, logger) =>
                {
                    logger.Error(ex, $"Error occured while handling {@event.GetType().Name} event");
                })
                .ExecuteAsync();
    }
}