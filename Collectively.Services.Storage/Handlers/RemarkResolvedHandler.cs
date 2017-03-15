using System.Threading.Tasks;
using Collectively.Messages.Events;
using Collectively.Services.Storage.Repositories;
using Collectively.Common.Services;
using Collectively.Messages.Events.Remarks;
using Collectively.Services.Storage.Models.Remarks;
using Collectively.Services.Storage.ServiceClients;

namespace Collectively.Services.Storage.Handlers
{
    public class RemarkResolvedHandler : IEventHandler<RemarkResolved>
    {
        private readonly IHandler _handler;
        private readonly IRemarkRepository _remarkRepository;
        private readonly IRemarkServiceClient _remarkServiceClient;

        public RemarkResolvedHandler(IHandler handler, 
            IRemarkRepository remarkRepository,
            IRemarkServiceClient remarkServiceClient)
        {
            _handler = handler;
            _remarkRepository = remarkRepository;
            _remarkServiceClient = remarkServiceClient;
        }

        public async Task HandleAsync(RemarkResolved @event)
        {
            await _handler
                .Run(async () =>
                {
                    var remark = await _remarkRepository.GetByIdAsync(@event.RemarkId);
                    if (remark.HasNoValue)
                        return;

                    var remarkDto = await _remarkServiceClient.GetAsync<Remark>(@event.RemarkId);
                    remark.Value.State = remarkDto.Value.State;
                    remark.Value.States = remarkDto.Value.States;
                    remark.Value.Photos = remarkDto.Value.Photos;
                    remark.Value.Resolved = true;
                    await _remarkRepository.UpdateAsync(remark.Value);
                })
                .OnError((ex, logger) =>
                {
                    logger.Error(ex, $"Error occured while handling {@event.GetType().Name} event");
                })
                .ExecuteAsync();
        }
    }
}