using System.Threading.Tasks;
using Collectively.Services.Storage.Models.Notifications;

namespace Collectively.Services.Storage.Services
{
    public interface IUserNotificationSettingsCache
    {
        Task AddAsync(UserNotificationSettings settings);
        Task DeleteAsync(string userId);          
    }
}