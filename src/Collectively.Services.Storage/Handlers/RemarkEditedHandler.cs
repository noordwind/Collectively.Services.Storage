using System.Threading.Tasks;
using Collectively.Messages.Events;
using Collectively.Services.Storage.Repositories;
using Collectively.Common.Services;
using Collectively.Messages.Events.Remarks;
using Collectively.Services.Storage.Models.Remarks;
using Collectively.Services.Storage.ServiceClients;
using Collectively.Common.Caching;
using Collectively.Services.Storage.Services;

namespace Collectively.Services.Storage.Handlers
{
    public class RemarkEditedHandler : IEventHandler<RemarkEdited>
    {
        private readonly IHandler _handler;
        private readonly IRemarkRepository _remarkRepository;
        private readonly IRemarkServiceClient _remarkServiceClient;
        private readonly IRemarkCache _cache;

        public RemarkEditedHandler(IHandler handler, 
            IRemarkRepository remarkRepository,
            IRemarkServiceClient remarkServiceClient,
            IRemarkCache cache)
        {
            _handler = handler;
            _remarkRepository = remarkRepository;
            _remarkServiceClient = remarkServiceClient;
            _cache = cache;
        }

        public async Task HandleAsync(RemarkEdited @event)
            => await _handler
                .Run(async () =>
                {
                    var remark = await _remarkRepository.GetByIdAsync(@event.RemarkId);
                    if (remark.HasNoValue)
                    {
                        return;
                    }
                    var remarkDto = await _remarkServiceClient.GetAsync<Remark>(@event.RemarkId);
                    remark.Value.Group = remarkDto.Value.Group;
                    remark.Value.Category = remarkDto.Value.Category;
                    remark.Value.Description = remarkDto.Value.Description;
                    remark.Value.Location = remarkDto.Value.Location;
                    remark.Value.Tags = remarkDto.Value.Tags;
                    remark.Value.UpdatedAt = remarkDto.Value.UpdatedAt;
                    remark.Value.States = remarkDto.Value.States;
                    remark.Value.State = remarkDto.Value.State;
                    await _remarkRepository.UpdateAsync(remark.Value);
                    await _cache.AddAsync(remark.Value);
                })
                .OnError((ex, logger) =>
                {
                    logger.Error(ex, $"Error occured while handling {@event.GetType().Name} event");
                })
                .ExecuteAsync();
    }
}