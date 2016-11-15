using System.Threading.Tasks;
using Coolector.Common.Events;
using Coolector.Common.Events.Users;
using Coolector.Services.Storage.Repositories;
using Coolector.Dto.Users;

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
                Provider = @event.Provider
            };
            await _repository.AddAsync(user);
        }
    }
}