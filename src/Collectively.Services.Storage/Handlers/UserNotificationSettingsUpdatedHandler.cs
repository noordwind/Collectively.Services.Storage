using System.Threading.Tasks;
using Collectively.Messages.Events;
using Collectively.Common.Services;
using Collectively.Messages.Events.Notifications;
using Collectively.Services.Storage.Models.Notifications;
using Collectively.Services.Storage.Repositories;
using Collectively.Services.Storage.ServiceClients;
using Collectively.Common.Caching;
using Collectively.Services.Storage.Services;
using Collectively.Services.Storage.ServiceClients.Queries;

namespace Collectively.Services.Storage.Handlers
{
    public class UserNotificationSettingsUpdatedHandler : IEventHandler<UserNotificationSettingsUpdated>
    {
        private readonly IHandler _handler;
        private readonly IUserNotificationSettingsRepository _repository;
        private readonly INotificationServiceClient _serviceClient;
        private readonly IUserNotificationSettingsCache _cache;

        public UserNotificationSettingsUpdatedHandler(IHandler handler, 
            IUserNotificationSettingsRepository repository,
            INotificationServiceClient serviceClient,
            IUserNotificationSettingsCache cache)
        {
            _handler = handler;
            _repository = repository;
            _serviceClient = serviceClient;
            _cache = cache;
        }

        public async Task HandleAsync(UserNotificationSettingsUpdated @event)
        => await _handler
            .Run(async () =>
            {
                var settings = await _serviceClient.GetUserNotificationSettingsAsync(
                    new GetUserNotificationSettings
                    {
                        UserId = @event.UserId
                    });
                var existingSettings = await _repository.GetAsync(@event.UserId);
                if (existingSettings.HasValue) 
                {
                    await _repository.EditAsync(settings.Value);
                }
                else 
                {
                    await _repository.AddAsync(settings.Value);
                }
                await _cache.AddAsync(settings.Value);
            })
            .OnError((ex, logger) =>
            {
                logger.Error(ex, $"Error occured while handling {@event.GetType().Name} event");
            })
            .ExecuteAsync();
    }
}