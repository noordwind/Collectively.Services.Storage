using System.Threading.Tasks;
using Collectively.Common.Types;
using Collectively.Services.Storage.Models.Notifications;
using Collectively.Services.Storage.ServiceClients;
using Collectively.Services.Storage.ServiceClients.Queries;

namespace Collectively.Services.Storage.Providers
{
    public class NotificationProvider : INotificationProvider
    {
        private readonly IProviderClient _providerClient;
        private readonly INotificationServiceClient _serviceClient;

        public NotificationProvider(IProviderClient providerClient,
            INotificationServiceClient serviceClient)
        {
            _providerClient = providerClient;
            _serviceClient = serviceClient;
        }

        public async Task<Maybe<UserNotificationSettings>> GetUserNotificationSettingsAsync(
            GetUserNotificationSettings query)
            => await _providerClient.GetAsync(
                async () => await _serviceClient.GetUserNotificationSettingsAsync(query));
    }
}