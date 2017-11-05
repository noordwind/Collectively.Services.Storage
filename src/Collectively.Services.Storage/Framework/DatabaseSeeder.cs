using System.Threading.Tasks;
using Collectively.Common.Mongo;
using Collectively.Services.Storage.Models.Remarks;
using Collectively.Services.Storage.Repositories.Queries;
using Collectively.Services.Storage.ServiceClients;
using Collectively.Services.Storage.ServiceClients.Queries;
using MongoDB.Driver;

namespace Collectively.Services.Storage.Framework
{
    public class DatabaseSeeder : IDatabaseSeeder
    {
        private readonly IMongoDatabase _database;
        private readonly IRemarkServiceClient _remarkServiceClient;

        public DatabaseSeeder(IMongoDatabase database, IRemarkServiceClient remarkServiceClient)
        {
            _database = database;
            _remarkServiceClient = remarkServiceClient;
        }

        public async Task SeedAsync()
        {
            if (await _database.Remarks().AsQueryable().AnyAsync() == false)
            {
                var index = new IndexKeysDefinitionBuilder<Remark>().Geo2DSphere(x => x.Location);
                await _database.Remarks().Indexes.CreateOneAsync(index);
            }
            if (await _database.RemarkCategories().AsQueryable().AnyAsync() == false)
            {
                var categories = await _remarkServiceClient
                    .BrowseCategoriesAsync<RemarkCategory>(new BrowseRemarkCategories
                    {
                        Results = int.MaxValue
                    });
                if (categories.HasValue)
                {
                    await _database.RemarkCategories().InsertManyAsync(categories.Value.Items);
                }
            }
            if (await _database.Tags().AsQueryable().AnyAsync() == false)
            {
                var tags = await _remarkServiceClient
                    .BrowseTagsAsync<Models.Remarks.Tag>(new BrowseTags
                    {
                        Results = int.MaxValue
                    });
                if (tags.HasValue)
                {
                    await _database.Tags().InsertManyAsync(tags.Value.Items);
                }
            }
        }
    }
}