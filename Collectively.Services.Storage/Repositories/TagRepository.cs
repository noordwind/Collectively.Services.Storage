using System.Collections.Generic;
using System.Threading.Tasks;
using Collectively.Common.Mongo;
using Collectively.Common.Types;
using Collectively.Services.Storage.Models.Remarks;
using Collectively.Services.Storage.Queries;
using Collectively.Services.Storage.Repositories.Queries;
using MongoDB.Driver;
using Tag = Collectively.Services.Storage.Models.Remarks.Tag;

namespace Collectively.Services.Storage.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly IMongoDatabase _database;

        public TagRepository(IMongoDatabase database)
        {
            _database = database;
        }

        public async Task<Maybe<Tag>> GetAsync(string name)
            => await _database.Tags().GetAsync(name);

        public async Task<Maybe<PagedResult<Tag>>> BrowseAsync(BrowseRemarkTags query)
            => await _database.Tags()
                .Query(query)
                .PaginateAsync();

        public async Task AddManyAsync(IEnumerable<Tag> tags)
            => await _database.Tags().InsertManyAsync(tags);        
    }
}