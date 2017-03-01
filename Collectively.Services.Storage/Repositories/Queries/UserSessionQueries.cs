using System;
using System.Threading.Tasks;
using Collectively.Common.Mongo;

using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Collectively.Services.Storage.Repositories.Queries
{
    public static class UserSessionQueries
    {
        public static IMongoCollection<UserSessionDto> UserSessions(this IMongoDatabase database)
            => database.GetCollection<UserSessionDto>();

        public static async Task<UserSessionDto> GetByIdAsync(this IMongoCollection<UserSessionDto> sessions, Guid id)
        {
            if (id == Guid.Empty)
                return null;

            return await sessions.AsQueryable().FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}