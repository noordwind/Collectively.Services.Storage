using System.Threading.Tasks;
using Coolector.Common.Events;
using Coolector.Services.Remarks.Shared.Events;
using Coolector.Services.Storage.Repositories;

namespace Coolector.Services.Storage.Handlers
{
    public class RemarkDeletedHandler : IEventHandler<RemarkDeleted>
    {
        private readonly IRemarkRepository _repository;

        public RemarkDeletedHandler(IRemarkRepository repository)
        {
            _repository = repository;
        }

        public async Task HandleAsync(RemarkDeleted @event)
        {
            var remark = await _repository.GetByIdAsync(@event.Id);
            if (remark.HasNoValue)
                return;

            await _repository.DeleteAsync(remark.Value);
        }
    }
}