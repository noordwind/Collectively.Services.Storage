using System.Threading.Tasks;
using Collectively.Messages.Events;
using Collectively.Common.Domain;
using Collectively.Common.Services;
using Collectively.Services.Storage.Repositories;
using Collectively.Messages.Events.Users;

namespace Collectively.Services.Storage.Handlers
{
    public class AccountLockedHandler: IEventHandler<AccountLocked>
    {
        private readonly IHandler _handler;
        private readonly IUserRepository _userRepository;

        public AccountLockedHandler(IHandler handler, IUserRepository userRepository)
        {
            _handler = handler;
            _userRepository = userRepository;
        }

        public async Task HandleAsync(AccountLocked @event)
        {
            await _handler
                .Run(async () => 
                {
                    var user = await _userRepository.GetByIdAsync(@event.LockUserId);
                    user.Value.State = "locked";
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