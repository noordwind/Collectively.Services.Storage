using System.Linq;
using System.Threading.Tasks;
using Collectively.Common.Services;
using Collectively.Messages.Events;
using Collectively.Messages.Events.Remarks;
using Collectively.Services.Storage.Models.Remarks;
using Collectively.Services.Storage.Repositories;
using Collectively.Services.Storage.ServiceClients;
using Microsoft.Azure.Documents.SystemFunctions;

namespace Collectively.Services.Storage.Handlers
{
    public class RemarkStateDeletedHandler : IEventHandler<RemarkStateDeleted>
    {
        private readonly IHandler _handler;
        private readonly IRemarkRepository _remarkRepository;
        private readonly IRemarkServiceClient _remarkServiceClient;

        public RemarkStateDeletedHandler(IHandler handler,
            IRemarkRepository remarkRepository,
            IRemarkServiceClient remarkServiceClient)
        {
            _handler = handler;
            _remarkRepository = remarkRepository;
            _remarkServiceClient = remarkServiceClient;
        }

        public async Task HandleAsync(RemarkStateDeleted @event)
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