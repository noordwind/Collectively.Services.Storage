using System.Threading.Tasks;
using Coolector.Common.Events;
using Coolector.Common.Domain;
using Coolector.Common.Services;
using Coolector.Services.Storage.Repositories;
using Coolector.Services.Users.Shared.Events;

namespace Coolector.Services.Storage.Handlers
{
    public class AvatarChangedHandler : IEventHandler<AvatarChanged>
    {
        private readonly IHandler _handler;
        private readonly IUserRepository _userRepository;

        public AvatarChangedHandler(IHandler handler, IUserRepository userRepository)
        {
            _handler = handler;
            _userRepository = userRepository;
        }

        public async Task HandleAsync(AvatarChanged @event)
        {
            await _handler
                .Run(async () =>
                {
                    var user = await _userRepository.GetByIdAsync(@event.UserId);
                    if (user.HasNoValue)
                    {
                        throw new ServiceException(OperationCodes.UserNotFound,
                            $"Avatar cannot be changed because user: {@event.UserId} does not exist");
                    }
                    user.Value.PictureUrl = @event.PictureUrl;
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