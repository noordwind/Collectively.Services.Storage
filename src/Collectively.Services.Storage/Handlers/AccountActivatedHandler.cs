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
        private readonly IUserCache _cache;

        public AccountActivatedHandler(IHandler handler,
            IUserRepository repository,
            IAccountStateService stateService,
            IUserCache cache)
        {
            _handler = handler;
            _repository = repository;
            _stateService = stateService;
            _cache = cache;
        }

        public async Task HandleAsync(AccountActivated @event)
        {
            await _handler
                .Run(async () =>
                {
                    var user = await _repository.GetByIdAsync(@event.UserId);
                    user.Value.State = "active";
                    await _repository.EditAsync(user.Value);
                    await _stateService.SetAsync(user.Value.UserId, user.Value.State);
                    await _cache.AddAsync(user.Value);
                })
                .OnError((ex, logger) =>
                {
                    logger.Error(ex, $"Error occured while handling {@event.GetType().Name} event");
                })
                .ExecuteAsync();
        }
    }
}