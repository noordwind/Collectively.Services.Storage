using System.Threading.Tasks;
using Collectively.Messages.Events;
using Collectively.Common.Domain;
using Collectively.Common.Services;
using Collectively.Services.Storage.Repositories;
using Collectively.Messages.Events.Users;

namespace Collectively.Services.Storage.Handlers
{
    public class AccountUnlockedHandler: IEventHandler<AccountUnlocked>
    {
        private readonly IHandler _handler;
        private readonly IUserRepository _userRepository;

        public AccountUnlockedHandler(IHandler handler, IUserRepository userRepository)
        {
            _handler = handler;
            _userRepository = userRepository;
        }

        public async Task HandleAsync(AccountUnlocked @event)
        {
            await _handler
                .Run(async () => 
                {
                    var user = await _userRepository.GetByIdAsync(@event.UnlockUserId);
                    user.Value.State = "active";
                    await _userRepository.EditAsync(user.Value);
                })
                .OnError((ex, logger) =>
                {
                    logger.Error(ex, $"Error occured while handling {@event.GetType().Name} event");
                })
                .ExecuteAsync();
        }
    }
}