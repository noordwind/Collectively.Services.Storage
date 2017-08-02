using System.Threading.Tasks;
using Collectively.Messages.Events;
using Collectively.Common.Domain;
using Collectively.Common.Services;
using Collectively.Services.Storage.Repositories;
using Collectively.Messages.Events.Users;

namespace Collectively.Services.Storage.Handlers
{
    public class AccountDeletedHandler: IEventHandler<AccountDeleted>
    {
        private readonly IHandler _handler;
        private readonly IUserRepository _userRepository;

        public AccountDeletedHandler(IHandler handler, IUserRepository userRepository)
        {
            _handler = handler;
            _userRepository = userRepository;
        }

        public async Task HandleAsync(AccountDeleted @event)
        {
            await _handler
                .Run(async () => 
                {
                    if(@event.Soft)
                    {
                        var user = await _userRepository.GetByIdAsync(@event.UserId);
                        user.Value.State = "deleted";
                        await _userRepository.EditAsync(user.Value);

                        return;
                    }
                    await _userRepository.DeleteAsync(@event.UserId);
                })
                .OnError((ex, logger) =>
                {
                    logger.Error(ex, $"Error occured while handling {@event.GetType().Name} event");
                })
                .ExecuteAsync();
        }
    }
}