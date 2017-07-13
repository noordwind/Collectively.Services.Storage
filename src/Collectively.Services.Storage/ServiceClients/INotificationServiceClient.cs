using System.Threading.Tasks;
using Collectively.Common.Types;
using Collectively.Services.Storage.Models.Notifications;
using Collectively.Services.Storage.ServiceClients.Queries;

namespace Collectively.Services.Storage.ServiceClients
{
    public interface INotificationServiceClient
    {
        Task<Maybe<UserNotificationSettings>> GetUserNotificationSettingsAsync(GetUserNotificationSettings query);
    }
}