using System.Threading.Tasks;
using Collectively.Messages.Events;
using Collectively.Common.Domain;
using Collectively.Common.Services;
using Collectively.Services.Storage.Repositories;
using Collectively.Messages.Events.Users;
using Serilog;
using Collectively.Services.Storage.Services;

namespace Collectively.Services.Storage.Handlers
{
    public class UsernameChangedHandler : IEventHandler<UsernameChanged>
    {
        private static readonly ILogger Logger = Log.Logger;

        private readonly IHandler _handler;
        private readonly IUserRepository _userRepository;
        private readonly IRemarkRepository _remarkRepository;
        private readonly IUserCache _cache;

        public UsernameChangedHandler(IHandler handler, 
            IUserRepository userRepository, 
            IRemarkRepository remarkRepository,
            IUserCache cache)
        {
            _handler = handler;
            _userRepository = userRepository;
            _remarkRepository = remarkRepository;
            _cache = cache;
        }

        public async Task HandleAsync(UsernameChanged @event)
        {
            await _handler
                .Run(async () =>
                {
                    Logger.Debug($"Handle {nameof(UsernameChanged)} event, userId:{@event.UserId}, newName:{@event.NewName}");
                    var user = await _userRepository.GetByIdAsync(@event.UserId);
                    if (user.HasNoValue)
                    {
                        throw new ServiceException(OperationCodes.UserNotFound,
                            $"User name cannot be changed because user: {@event.UserId} does not exist");
                    }

                    Logger.Debug($"Update userName, userId:{@event.UserId}, newName:{@event.NewName}");
                    user.Value.Name = @event.NewName;
                    user.Value.State = @event.State;
                    await _userRepository.EditAsync(user.Value);
                    await _cache.AddAsync(user.Value);

                    Logger.Debug($"Update user's remarks with new userName, userId:{@event.UserId}, newName:{@event.NewName}");
                    await _remarkRepository.UpdateUserNamesAsync(@event.UserId, @event.NewName);
                })
                .OnError((ex, logger) =>
                {
                    logger.Error(ex, $"Error occured while handling {@event.GetType().Name} event");
                })
                .ExecuteAsync();
        }
    }
}