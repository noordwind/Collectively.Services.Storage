using System.Threading.Tasks;
using Collectively.Common.Types;
using Collectively.Services.Storage.Models.Notifications;

namespace Collectively.Services.Storage.Repositories
{
    public interface IUserNotificationSettingsRepository
    {
        Task<Maybe<UserNotificationSettings>> GetAsync(string userId);
        Task AddAsync(UserNotificationSettings settings);
        Task EditAsync(UserNotificationSettings settings);
        Task DeleteAsync(string userId);         
    }
}