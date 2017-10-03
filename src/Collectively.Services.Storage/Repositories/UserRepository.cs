using System.Collections.Generic;
using System.Threading.Tasks;
using Collectively.Common.Types;
using Collectively.Common.Mongo;
using Collectively.Services.Storage.ServiceClients.Queries;
using Collectively.Services.Storage.Models.Users;
using Collectively.Services.Storage.Repositories.Queries;
using MongoDB.Driver;
using System;

namespace Collectively.Services.Storage.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoDatabase _database;

        public UserRepository(IMongoDatabase database)
        {
            _database = database;
        }

        public async Task<bool> ExistsAsync(string id)
            => await _database.Users().ExistsAsync(id);

        public async Task<Maybe<AvailableResource>> IsNameAvailableAsync(string name)
        {
            var exists = await _database.Users().NameExistsAsync(name);

            return new AvailableResource {IsAvailable = exists == false};
        }

        public async Task<Maybe<PagedResult<User>>> BrowseAsync(BrowseUsers query)
            => await _database.Users()
                .Query(query)
                .PaginateAsync(query);

        public async Task<Maybe<User>> GetByIdAsync(string id)
            => await _database.Users().GetByIdAsync(id);

        public async Task<Maybe<User>> GetByNameAsync(string name)
            => await _database.Users().GetByNameAsync(name);

        public async Task<Maybe<string>> GetStateAsync(string id)
            => await _database.Users().GetStateAsync(id);

        public async Task EditAsync(User user)
            => await _database.Users().ReplaceOneAsync(x => x.UserId == user.UserId, user);

        public async Task AddAsync(User user)
            => await _database.Users().InsertOneAsync(user);

        public async Task AddManyAsync(IEnumerable<User> users)
            => await _database.Users().InsertManyAsync(users);

        public async Task DeleteAsync(string userId)
            => await _database.Users().DeleteOneAsync(x => x.UserId == userId);
    }
}