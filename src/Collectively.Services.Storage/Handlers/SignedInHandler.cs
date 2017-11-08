using System;
using System.Threading.Tasks;
using Collectively.Messages.Events;
using Collectively.Common.Services;
using Collectively.Services.Storage.Repositories;
using Collectively.Messages.Events.Users;
using Collectively.Services.Storage.Models.Users;
using Collectively.Services.Storage.ServiceClients;
using System.Collections.Generic;
using Collectively.Services.Storage.Services;

namespace Collectively.Services.Storage.Handlers
{
    public class SignedInHandler : IEventHandler<SignedIn>
    {
        private readonly IHandler _handler;
        private readonly IUserRepository _repository;
        private readonly IUserServiceClient _userServiceClient;
        private readonly IAccountStateService _accountStateService;
        private readonly IUserCache _cache;

        public SignedInHandler(IHandler handler, 
            IUserRepository repository, 
            IUserServiceClient userServiceClient,
            IAccountStateService accountStateService,
            IUserCache cache)
        {
            _handler = handler;
            _repository = repository;
            _userServiceClient = userServiceClient;
            _accountStateService = accountStateService;
            _cache = cache;
        }

        public async Task HandleAsync(SignedIn @event)
        {
            await _handler
                .Run(async () =>
                {
                    var userDto = await _userServiceClient.GetAsync<User>(@event.UserId);
                    var user = await _repository.GetByIdAsync(@event.UserId);
                    user.Value.State = userDto.Value.State;
                    await _repository.EditAsync(user.Value);
                    await _accountStateService.SetAsync(userDto.Value.UserId, userDto.Value.State);
                    await _cache.AddAsync(user.Value);
                })
                .OnError((ex, logger) =>
                {
                    logger.Error(ex, $"Error occured while handling {@event.GetType().Name} event");
                })
                .ExecuteAsync();
        }
    }
}