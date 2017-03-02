using System.Threading.Tasks;
using Collectively.Common.Extensions;
using Collectively.Common.Mongo;
using Collectively.Services.Storage.Models.Users;
using Collectively.Services.Storage.Queries;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Collectively.Services.Storage.Repositories.Queries
{
    public static class UserQueries
    {
        public static IMongoCollection<User> Users(this IMongoDatabase database)
            => database.GetCollection<User>();

        public static async Task<bool> NameExistsAsync(this IMongoCollection<User> users, string name)
            => await users.AsQueryable().AnyAsync(x => x.Name == name);

        public static async Task<bool> ExistsAsync(this IMongoCollection<User> users, string id)
            => await users.AsQueryable().AnyAsync(x => x.UserId == id);

        public static async Task<User> GetByIdAsync(this IMongoCollection<User> users, string id)
        {
            if (id.Empty())
                return null;

            return await users.AsQueryable().FirstOrDefaultAsync(x => x.UserId == id);
        }

        public static async Task<User> GetByNameAsync(this IMongoCollection<User> users, string name)
        {
            if (name.Empty())
                return null;

            return await users.AsQueryable().FirstOrDefaultAsync(x => x.Name == name);
        }

        public static async Task<User> GetByEmailAsync(this IMongoCollection<User> users, string email)
        {
            if (email.Empty())
                return null;

            return await users.AsQueryable().FirstOrDefaultAsync(x => x.Email == email);
        }

        public static IMongoQueryable<User> Query(this IMongoCollection<User> users,
            BrowseUsers query)
        {
            var values = users.AsQueryable();

            return values.OrderBy(x => x.Name);
        }
    }
}