using System.Threading.Tasks;
using Collectively.Common.Extensions;
using Collectively.Common.Mongo;
using Collectively.Services.Storage.Models.Users;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Collectively.Services.Storage.Models.Notifications;

namespace Collectively.Services.Storage.Repositories.Queries
{
    public static class UserNotificationSettingsQueries
    {
        public static IMongoCollection<UserNotificationSettings> UserNotificationSettings(this IMongoDatabase database)
            => database.GetCollection<UserNotificationSettings>();

        public static async Task<UserNotificationSettings> GetAsync(this IMongoCollection<UserNotificationSettings> settings, 
            string userId)
        {
            if (userId.Empty())
                return null;

            return await settings.AsQueryable().FirstOrDefaultAsync(x => x.UserId == userId);
        }
    }
}