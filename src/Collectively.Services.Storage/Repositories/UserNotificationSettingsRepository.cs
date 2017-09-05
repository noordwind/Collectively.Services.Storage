using System.Threading.Tasks;
using Collectively.Common.Types;
using Collectively.Services.Storage.Models.Notifications;
using Collectively.Services.Storage.Repositories.Queries;
using MongoDB.Driver;

namespace Collectively.Services.Storage.Repositories
{
    public class UserNotificationSettingsRepository : IUserNotificationSettingsRepository
    {
        private readonly IMongoDatabase _database;

        public UserNotificationSettingsRepository(IMongoDatabase database)
        {
            _database = database;
        }

        public async Task<Maybe<UserNotificationSettings>> GetAsync(string userId)
            => await _database.UserNotificationSettings().GetAsync(userId);

        public async Task AddAsync(UserNotificationSettings settings)
            => await _database.UserNotificationSettings().InsertOneAsync(settings);

        public async Task EditAsync(UserNotificationSettings settings)
            => await _database.UserNotificationSettings().ReplaceOneAsync(x => x.UserId == settings.UserId, settings);

        public async Task DeleteAsync(string userId)
            => await _database.UserNotificationSettings().DeleteOneAsync(x => x.UserId == userId);
    }
}