using System;
using System.Threading.Tasks;
using Coolector.Common.Types;
using Coolector.Dto.Users;
using Coolector.Services.Storage.Repositories.Queries;
using MongoDB.Driver;

namespace Coolector.Services.Storage.Repositories
{
    public class UserSessionRepository : IUserSessionRepository
    {
        private readonly IMongoDatabase _database;

        public UserSessionRepository(IMongoDatabase database)
        {
            _database = database;
        }

        public async Task<Maybe<UserSessionDto>> GetByIdAsync(Guid id)
            => await _database.UserSessions().GetByIdAsync(id);

        public async Task AddAsync(UserSessionDto session)
            => await _database.UserSessions().InsertOneAsync(session);
    }
}