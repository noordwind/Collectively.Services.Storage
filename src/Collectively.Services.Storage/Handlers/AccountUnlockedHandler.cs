using System.Threading.Tasks;
using Collectively.Messages.Events;
using Collectively.Common.Domain;
using Collectively.Common.Services;
using Collectively.Services.Storage.Repositories;
using Collectively.Messages.Events.Users;
using Collectively.Services.Storage.Services;

namespace Collectively.Services.Storage.Handlers
{
    public class AccountUnlockedHandler: IEventHandler<AccountUnlocked>
    {
        private readonly IHandler _handler;
        private readonly IUserRepository _userRepository;
        private readonly IAccountStateService _stateService;
        private readonly IUserCache _cache;

        public AccountUnlockedHandler(IHandler handler, 
            IUserRepository userRepository,
            IAccountStateService stateService,
            IUserCache cache)
        {
            _handler = handler;
            _userRepository = userRepository;
            _stateService = stateService;
            _cache = cache;
        }

        public async Task HandleAsync(AccountUnlocked @event)
        {
            await _handler
                .Run(async () => 
                {
                    var user = await _userRepository.GetByIdAsync(@event.UnlockUserId);
                    user.Value.State = "active";
                    await _userRepository.EditAsync(user.Value);
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