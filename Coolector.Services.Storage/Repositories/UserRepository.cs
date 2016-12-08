﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Coolector.Common.Dto.General;
using Coolector.Common.Types;
using Coolector.Common.Mongo;
using Coolector.Services.Storage.Queries;
using Coolector.Services.Storage.Repositories.Queries;
using Coolector.Services.Users.Shared.Dto;
using MongoDB.Driver;

namespace Coolector.Services.Storage.Repositories
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

        public async Task<Maybe<AvailableResourceDto>> IsNameAvailableAsync(string name)
        {
            var exists = await _database.Users().NameExistsAsync(name);

            return new AvailableResourceDto {IsAvailable = exists == false};
        }

        public async Task<Maybe<PagedResult<UserDto>>> BrowseAsync(BrowseUsers query)
            => await _database.Users()
                .Query(query)
                .PaginateAsync(query);

        public async Task<Maybe<UserDto>> GetByIdAsync(string id)
            => await _database.Users().GetByIdAsync(id);

        public async Task<Maybe<UserDto>> GetByNameAsync(string name)
            => await _database.Users().GetByNameAsync(name);

        public async Task EditAsync(UserDto user)
            => await _database.Users().ReplaceOneAsync(x => x.UserId == user.UserId, user);

        public async Task AddAsync(UserDto user)
            => await _database.Users().InsertOneAsync(user);

        public async Task AddManyAsync(IEnumerable<UserDto> users)
            => await _database.Users().InsertManyAsync(users);
    }
}