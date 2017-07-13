using Collectively.Services.Storage.Models.Notifications;
using Collectively.Services.Storage.Providers;
using Collectively.Services.Storage.ServiceClients.Queries;

namespace Collectively.Services.Storage.Modules
{
    public class NotificationModule : ModuleBase
    {
        public NotificationModule(INotificationProvider provider) : base("notification")
        {
            Get("settings/{userId}",
                async args => await Fetch<GetUserNotificationSettings, UserNotificationSettings>
                (async x => await provider.GetUserNotificationSettingsAsync(x))
                .HandleAsync());
        }
    }
}