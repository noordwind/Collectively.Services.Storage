using System;
using System.Threading.Tasks;
using Collectively.Messages.Events;
using Collectively.Common.Services;
using Collectively.Services.Storage.Repositories;
using Collectively.Messages.Events.Users;
using Collectively.Services.Storage.Models.Users;
using Collectively.Services.Storage.ServiceClients;
using System.Collections.Generic;

namespace Collectively.Services.Storage.Handlers
{
    public class SignedUpHandler : IEventHandler<SignedUp>
    {
        private readonly IHandler _handler;
        private readonly IUserRepository _repository;
        private readonly IUserServiceClient _userServiceClient;

        public SignedUpHandler(IHandler handler, 
            IUserRepository repository, IUserServiceClient userServiceClient)
        {
            _handler = handler;
            _repository = repository;
            _userServiceClient = userServiceClient;
        }

        public async Task HandleAsync(SignedUp @event)
        {
            await _handler
                .Run(async () =>
                {
                    var user = await _userServiceClient.GetAsync<User>(@event.UserId);
                    if(user.Value.FavoriteRemarks == null)
                    {
                        user.Value.FavoriteRemarks = new HashSet<Guid>();
                    }
                    await _repository.AddAsync(user.Value);
                })
                .OnError((ex, logger) =>
                {
                    logger.Error(ex, $"Error occured while handling {@event.GetType().Name} event");
                })
                .ExecuteAsync();
        }
    }
}