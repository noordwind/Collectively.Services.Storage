using System.Threading.Tasks;
using Collectively.Messages.Events;
using Collectively.Services.Storage.Repositories;
using Collectively.Common.Services;
using Collectively.Messages.Events.Remarks;
using Collectively.Services.Storage.Models.Remarks;
using Collectively.Services.Storage.ServiceClients;
using Collectively.Common.Caching;
using Collectively.Services.Storage.Services;
using System.Linq;

namespace Collectively.Services.Storage.Handlers
{
    public class RemarkRenewedHandler : IEventHandler<RemarkRenewed>
    {
        private readonly IHandler _handler;
        private readonly IRemarkRepository _remarkRepository;
        private readonly IGroupRemarkRepository _groupRemarkRepository;
        private readonly IRemarkServiceClient _remarkServiceClient;
        private readonly IRemarkCache _cache;

        public RemarkRenewedHandler(IHandler handler, 
            IRemarkRepository remarkRepository,
            IGroupRemarkRepository groupRemarkRepository,
            IRemarkServiceClient remarkServiceClient,
            IRemarkCache cache)
        {
            _handler = handler;
            _remarkRepository = remarkRepository;
            _groupRemarkRepository = groupRemarkRepository;
            _remarkServiceClient = remarkServiceClient;
            _cache = cache;
        }

        public async Task HandleAsync(RemarkRenewed @event)
        {
            await _handler
                .Run(async () =>
                {
                    var remark = await _remarkRepository.GetByIdAsync(@event.RemarkId);
                    if (remark.HasNoValue)
                        return;

                    var remarkDto = await _remarkServiceClient.GetAsync<Remark>(@event.RemarkId);
                    remark.Value.UpdatedAt = remarkDto.Value.UpdatedAt;
                    remark.Value.State = remarkDto.Value.State;
                    remark.Value.States = remarkDto.Value.States;
                    remark.Value.Photos = remarkDto.Value.Photos;
                    remark.Value.Resolved = false;
                    await _remarkRepository.UpdateAsync(remark.Value);
                    if (remark.Value.AvailableGroups?.Any() == true)
                    {
                        await _groupRemarkRepository.AddRemarksAsync(remark.Value.Id, remark.Value.AvailableGroups);
                    }
                    await _cache.AddAsync(remark.Value, addGeo: true);
                })
                .OnError((ex, logger) =>
                {
                    logger.Error(ex, $"Error occured while handling {@event.GetType().Name} event");
                })
                .ExecuteAsync();
        }
    }
}