using System.Threading.Tasks;
using Collectively.Common.Services;
using Collectively.Messages.Events;
using Collectively.Messages.Events.Users;
using Collectively.Services.Storage.Repositories;
using Collectively.Services.Storage.Services;

namespace Collectively.Services.Storage.Handlers
{
    public class AccountActivatedHandler : IEventHandler<AccountActivated>
    {
        private readonly IHandler _handler;
        private readonly IUserRepository _repository;
        private readonly IAccountStateService _stateService;

        public AccountActivatedHandler(IHandler handler,
            IUserRepository repository,
            IAccountStateService stateService)
        {
            _handler = handler;
            _repository = repository;
            _stateService = stateService;
        }

        public async Task HandleAsync(AccountActivated @event)
        {
            await _handler
                .Run(async () =>
                {
                    var user = await _repository.GetByIdAsync(@event.UserId);
                    user.Value.State = "active";
                    await _repository.EditAsync(user.Value);
                    await _stateService.SetAsync(@event.UserId, user.Value.State);

                })
                .OnError((ex, logger) =>
                {
                    logger.Error(ex, $"Error occured while handling {@event.GetType().Name} event");
                })
                .ExecuteAsync();
        }
    }
}