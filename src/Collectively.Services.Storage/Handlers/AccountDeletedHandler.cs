using System.Threading.Tasks;
using Collectively.Messages.Events;
using Collectively.Common.Domain;
using Collectively.Common.Services;
using Collectively.Services.Storage.Repositories;
using Collectively.Messages.Events.Users;
using Collectively.Services.Storage.Services;

namespace Collectively.Services.Storage.Handlers
{
    public class AccountDeletedHandler: IEventHandler<AccountDeleted>
    {
        private readonly IHandler _handler;
        private readonly IUserRepository _userRepository;
        private readonly IAccountStateService _stateService;
        private readonly IUserCache _cache;

        public AccountDeletedHandler(IHandler handler, 
            IUserRepository userRepository,
            IAccountStateService stateService,
            IUserCache cache)
        {
            _handler = handler;
            _userRepository = userRepository;
            _stateService = stateService;
            _cache = cache;
        }

        public async Task HandleAsync(AccountDeleted @event)
        {
            await _handler
                .Run(async () => 
                {
                    if (@event.Soft)
                    {
                        var user = await _userRepository.GetByIdAsync(@event.UserId);
                        user.Value.State = "deleted";
                        await _userRepository.EditAsync(user.Value);
                        await _stateService.SetAsync(@event.UserId, user.Value.State);

                        return;
                    }
                    await _cache.DeleteAsync(@event.UserId);
                    await _stateService.SetAsync(@event.UserId, "deleted");
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