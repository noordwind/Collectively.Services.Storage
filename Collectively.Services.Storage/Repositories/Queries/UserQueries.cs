using System.Threading.Tasks;
using Collectively.Common.Extensions;
using Collectively.Common.Mongo;
using Collectively.Services.Storage.Queries;
using Collectively.Services.Storage.Dto.Users;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Collectively.Services.Storage.Repositories.Queries
{
    public static class UserQueries
    {
        public static IMongoCollection<UserDto> Users(this IMongoDatabase database)
            => database.GetCollection<UserDto>();

        public static async Task<bool> NameExistsAsync(this IMongoCollection<UserDto> users, string name)
            => await users.AsQueryable().AnyAsync(x => x.Name == name);

        public static async Task<bool> ExistsAsync(this IMongoCollection<UserDto> users, string id)
            => await users.AsQueryable().AnyAsync(x => x.UserId == id);

        public static async Task<UserDto> GetByIdAsync(this IMongoCollection<UserDto> users, string id)
        {
            if (id.Empty())
                return null;

            return await users.AsQueryable().FirstOrDefaultAsync(x => x.UserId == id);
        }

        public static async Task<UserDto> GetByNameAsync(this IMongoCollection<UserDto> users, string name)
        {
            if (name.Empty())
                return null;

            return await users.AsQueryable().FirstOrDefaultAsync(x => x.Name == name);
        }

        public static async Task<UserDto> GetByEmailAsync(this IMongoCollection<UserDto> users, string email)
        {
            if (email.Empty())
                return null;

            return await users.AsQueryable().FirstOrDefaultAsync(x => x.Email == email);
        }

        public static IMongoQueryable<UserDto> Query(this IMongoCollection<UserDto> users,
            BrowseUsers query)
        {
            var values = users.AsQueryable();

            return values.OrderBy(x => x.Name);
        }
    }
}