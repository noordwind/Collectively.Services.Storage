using System.Threading.Tasks;
using Coolector.Common.Extensions;
using Coolector.Common.Mongo;
using Coolector.Services.Remarks.Shared.Dto;
using Coolector.Services.Storage.Queries;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Coolector.Services.Storage.Repositories.Queries
{
    public static class TagQueries
    {
        public static IMongoCollection<TagDto> Tags(this IMongoDatabase database)
            => database.GetCollection<TagDto>();

        public static async Task<TagDto> GetAsync(this IMongoCollection<TagDto> tags, string name)
        {
            if (name.Empty())
                return null;

            return await tags.AsQueryable().FirstOrDefaultAsync(x => x.Name == name);
        }

        public static IMongoQueryable<TagDto> Query(this IMongoCollection<TagDto> tags,
            BrowseRemarkTags query)
        {
            var values = tags.AsQueryable();

            return values;
        }
    }
}