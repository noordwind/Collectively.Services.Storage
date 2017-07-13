using System.Threading.Tasks;
using Collectively.Common.ServiceClients;
using Collectively.Common.Types;
using Collectively.Services.Storage.Models.Notifications;
using Collectively.Services.Storage.ServiceClients.Queries;
using NLog;

namespace Collectively.Services.Storage.ServiceClients
{
    public class NotificationServiceClient : INotificationServiceClient
    {
        private readonly IServiceClient _serviceClient;
        private readonly string _name;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger(); 

        public NotificationServiceClient(IServiceClient serviceClient, string name)
        {
            _serviceClient = serviceClient;
            _name = name;
        }

        public async Task<Maybe<UserNotificationSettings>> GetUserNotificationSettingsAsync(GetUserNotificationSettings query)
        {
            Logger.Debug($"Requesting GetUserNotificationSettingsAsync, userId:{query.UserId}");
            return await _serviceClient
                .GetAsync<UserNotificationSettings>(_name, $"notification/settings/{query.UserId}");
        }
    }
}