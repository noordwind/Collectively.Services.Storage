using System.Threading.Tasks;
using Collectively.Common.Types;
using Collectively.Services.Storage.Models.Notifications;
using Collectively.Services.Storage.Repositories;
using Collectively.Services.Storage.ServiceClients;
using Collectively.Services.Storage.ServiceClients.Queries;

namespace Collectively.Services.Storage.Providers
{
    public class NotificationProvider : INotificationProvider
    {
        private readonly IProviderClient _providerClient;
        private readonly INotificationServiceClient _serviceClient;
        private readonly IUserNotificationSettingsRepository _repository;

        public NotificationProvider(IProviderClient providerClient,
            INotificationServiceClient serviceClient,
            IUserNotificationSettingsRepository repository)
        {
            _providerClient = providerClient;
            _serviceClient = serviceClient;
            _repository = repository;
        }

        public async Task<Maybe<UserNotificationSettings>> GetUserNotificationSettingsAsync(
            GetUserNotificationSettings query)
            => await _providerClient.GetAsync(
                async () => await _repository.GetAsync(query.UserId),
                async () => await _serviceClient.GetUserNotificationSettingsAsync(query));
    }
}