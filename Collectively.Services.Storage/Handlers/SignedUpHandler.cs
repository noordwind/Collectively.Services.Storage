using System.Threading.Tasks;
using Collectively.Messages.Events;
using Collectively.Common.Services;
using Collectively.Services.Storage.Repositories;

using Collectively.Messages.Events.Users;
using Collectively.Services.Storage.Models.Users;

namespace Collectively.Services.Storage.Handlers
{
    public class SignedUpHandler : IEventHandler<SignedUp>
    {
        private readonly IHandler _handler;
        private readonly IUserRepository _repository;

        public SignedUpHandler(IHandler handler, 
            IUserRepository repository)
        {
            _handler = handler;
            _repository = repository;
        }

        public async Task HandleAsync(SignedUp @event)
        {
            await _handler
                .Run(async () =>
                {
                    if (await _repository.ExistsAsync(@event.UserId))
                        return;

                    var user = new User
                    {
                        UserId = @event.UserId,
                        Name = @event.Name,
                        Email = @event.Email,
                        State = @event.State,
                        CreatedAt = @event.CreatedAt,
                        PictureUrl = @event.PictureUrl,
                        Role = @event.Role,
                        Provider = @event.Provider,
                        ExternalUserId = @event.ExternalUserId
                    };
                    await _repository.AddAsync(user);
                })
                .OnError((ex, logger) =>
                {
                    logger.Error(ex, $"Error occured while handling {@event.GetType().Name} event");
                })
                .ExecuteAsync();
        }
    }
}