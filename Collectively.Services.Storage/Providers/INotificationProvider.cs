using System.Threading.Tasks;
using Collectively.Common.Types;
using Collectively.Services.Storage.Models.Notifications;
using Collectively.Services.Storage.ServiceClients.Queries;

namespace Collectively.Services.Storage.Providers
{
    public interface INotificationProvider
    {
        Task<Maybe<UserNotificationSettings>> GetUserNotificationSettingsAsync(GetUserNotificationSettings query);
    }
}