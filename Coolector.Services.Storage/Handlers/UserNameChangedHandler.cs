using System.Threading.Tasks;
using Coolector.Common.Events;
using Coolector.Common.Events.Users;
using Coolector.Common.Domain;
using Coolector.Services.Storage.Repositories;
using NLog;

namespace Coolector.Services.Storage.Handlers
{
    public class UserNameChangedHandler : IEventHandler<UserNameChanged>
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly IUserRepository _userRepository;
        private readonly IRemarkRepository _remarkRepository;

        public UserNameChangedHandler(IUserRepository userRepository, IRemarkRepository remarkRepository)
        {
            _userRepository = userRepository;
            _remarkRepository = remarkRepository;
        }

        public async Task HandleAsync(UserNameChanged @event)
        {
            Logger.Debug($"Handle {nameof(UserNameChanged)} event, userId:{@event.UserId}, newName:{@event.NewName}");
            var user = await _userRepository.GetByIdAsync(@event.UserId);
            if (user.HasNoValue)
                throw new ServiceException($"User name cannot be changed because user: {@event.UserId} does not exist");

            Logger.Debug($"Update userName, userId:{@event.UserId}, newName:{@event.NewName}");
            user.Value.Name = @event.NewName;
            await _userRepository.EditAsync(user.Value);

            Logger.Debug($"Update user's remarks with new userName, userId:{@event.UserId}, newName:{@event.NewName}");
            await _remarkRepository.UpdateUserNamesAsync(@event.UserId, @event.NewName);
        }
    }
}