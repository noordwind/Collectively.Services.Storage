using System.Threading.Tasks;
using Coolector.Common.Events;
using Coolector.Services.Storage.Repositories;
using Coolector.Services.Users.Shared.Dto;
using Coolector.Services.Users.Shared.Events;

namespace Coolector.Services.Storage.Handlers
{
    public class UserSignedUpHandler : IEventHandler<UserSignedUp>
    {
        private readonly IUserRepository _repository;

        public UserSignedUpHandler(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task HandleAsync(UserSignedUp @event)
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
        }
    }
}