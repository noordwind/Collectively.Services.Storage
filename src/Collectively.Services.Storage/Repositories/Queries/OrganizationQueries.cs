using System;
using System.Threading.Tasks;
using Collectively.Common.Mongo;
using Collectively.Common.Extensions;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Collectively.Services.Storage.Models.Groups;
using Collectively.Services.Storage.ServiceClients.Queries;

namespace Collectively.Services.Storage.Repositories.Queries
{
    public static class OrganizationQueries
    {
        public static IMongoCollection<Organization> Organizations(this IMongoDatabase database)
            => database.GetCollection<Organization>();

        public static async Task<bool> ExistsAsync(this IMongoCollection<Organization> organizations, string codename)
            => await organizations.AsQueryable().AnyAsync(x => x.Codename == codename);

        public static async Task<Organization> GetAsync(this IMongoCollection<Organization> organizations, Guid id)
        {
            if (id.IsEmpty())
                return null;

            return await organizations.AsQueryable().FirstOrDefaultAsync(x => x.Id == id);
        }

        public static IMongoQueryable<Organization> Query(this IMongoCollection<Organization> organizations, 
            BrowseOrganizations query)
        {
            var values = organizations.AsQueryable();

            return values.OrderBy(x => x.Name);
        }
    }
}