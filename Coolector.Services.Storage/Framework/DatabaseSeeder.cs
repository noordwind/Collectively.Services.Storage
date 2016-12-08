using System.Threading.Tasks;
using Coolector.Common.Mongo;
using Coolector.Services.Remarks.Shared.Dto;
using Coolector.Services.Storage.Repositories.Queries;
using MongoDB.Driver;

namespace Coolector.Services.Storage.Framework
{
    public class DatabaseSeeder : IDatabaseSeeder
    {
        private readonly IMongoDatabase _database;

        public DatabaseSeeder(IMongoDatabase database)
        {
            _database = database;
        }

        public async Task SeedAsync()
        {
            if (await _database.Remarks().AsQueryable().AnyAsync() == false)
            {
                var index = new IndexKeysDefinitionBuilder<RemarkDto>().Geo2DSphere(x => x.Location);
                await _database.Remarks().Indexes.CreateOneAsync(index);
            }
        }
    }
}