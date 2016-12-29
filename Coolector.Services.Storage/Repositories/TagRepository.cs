using System.Collections.Generic;
using System.Threading.Tasks;
using Coolector.Common.Mongo;
using Coolector.Common.Types;
using Coolector.Services.Remarks.Shared.Dto;
using Coolector.Services.Storage.Queries;
using Coolector.Services.Storage.Repositories.Queries;
using MongoDB.Driver;

namespace Coolector.Services.Storage.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly IMongoDatabase _database;

        public TagRepository(IMongoDatabase database)
        {
            _database = database;
        }

        public async Task<Maybe<TagDto>> GetAsync(string name)
            => await _database.Tags().GetAsync(name);

        public async Task<Maybe<PagedResult<TagDto>>> BrowseAsync(BrowseRemarkTags query)
            => await _database.Tags()
                .Query(query)
                .PaginateAsync();

        public async Task AddManyAsync(IEnumerable<TagDto> tags)
            => await _database.Tags().InsertManyAsync(tags);        
    }
}