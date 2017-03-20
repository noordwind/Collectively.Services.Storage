using System.Threading.Tasks;
using Collectively.Messages.Events;
using Collectively.Common.Domain;
using Collectively.Common.Services;
using Collectively.Services.Storage.Repositories;
using Collectively.Messages.Events.Users;

namespace Collectively.Services.Storage.Handlers
{
    public class AvatarUploadedHandler : IEventHandler<AvatarUploaded>
    {
        private readonly IHandler _handler;
        private readonly IUserRepository _userRepository;

        public AvatarUploadedHandler(IHandler handler, IUserRepository userRepository)
        {
            _handler = handler;
            _userRepository = userRepository;
        }

        public async Task HandleAsync(AvatarUploaded @event)
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
                    // user.Value.AvatarUrl = @event.AvatarUrl;
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