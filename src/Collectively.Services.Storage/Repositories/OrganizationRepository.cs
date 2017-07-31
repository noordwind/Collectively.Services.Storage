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
    public class OrganizationRepository : IOrganizationRepository
    {
        private readonly IMongoDatabase _database;

        public OrganizationRepository(IMongoDatabase database)
        {
            _database = database;
        }

        public async Task<bool> ExistsAsync(string name)
        => await _database.Organizations().ExistsAsync(name.ToCodename());

        public async Task<Maybe<Organization>> GetAsync(Guid id)
        => await _database.Organizations().GetAsync(id);

        public async Task<Maybe<PagedResult<Organization>>> BrowseAsync(BrowseOrganizations query)
        => await _database.Organizations().Query(query).PaginateAsync(query);

        public async Task AddAsync(Organization organization)
        => await _database.Organizations().InsertOneAsync(organization);

        public async Task UpdateAsync(Organization organization)
        => await _database.Organizations().ReplaceOneAsync(x => x.Id == organization.Id, organization);

        public async Task DeleteAsync(Guid id)
        => await _database.Organizations().DeleteOneAsync(x => x.Id == id);
    }
}