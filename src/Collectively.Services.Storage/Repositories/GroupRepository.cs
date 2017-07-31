using System;
using System.Threading.Tasks;
using Collectively.Common.Mongo;
using Collectively.Common.Types;
using Collectively.Services.Storage.Framework;
using Collectively.Services.Storage.Models.Groups;
using Collectively.Services.Storage.Repositories.Queries;
using Collectively.Services.Storage.ServiceClients.Queries;
using MongoDB.Driver;

namespace Collectively.Services.Storage.Repositories
{
    public class GroupRepository : IGroupRepository
    {
        private readonly IMongoDatabase _database;

        public GroupRepository(IMongoDatabase database)
        {
            _database = database;
        }

        public async Task<bool> ExistsAsync(string name)
        => await _database.Groups().ExistsAsync(name.ToCodename());

        public async Task<Maybe<Group>> GetAsync(Guid id)
        => await _database.Groups().GetAsync(id);

        public async Task<Maybe<PagedResult<Group>>> BrowseAsync(BrowseGroups query)
        => await _database.Groups().Query(query).PaginateAsync(query);

        public async Task AddAsync(Group group)
        => await _database.Groups().InsertOneAsync(group);

        public async Task UpdateAsync(Group group)
        => await _database.Groups().ReplaceOneAsync(x => x.Id == group.Id, group);

        public async Task DeleteAsync(Guid id)
        => await _database.Groups().DeleteOneAsync(x => x.Id == id);
    }
}