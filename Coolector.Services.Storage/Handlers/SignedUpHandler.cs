using System.Threading.Tasks;
using Coolector.Common.Events;
using Coolector.Common.Services;
using Coolector.Services.Storage.Repositories;
using Coolector.Services.Users.Shared.Dto;
using Coolector.Services.Users.Shared.Events;

namespace Coolector.Services.Storage.Handlers
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

                    var user = new UserDto
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