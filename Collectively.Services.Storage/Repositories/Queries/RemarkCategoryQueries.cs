using System;
using System.Threading.Tasks;
using Collectively.Common.Mongo;
using Collectively.Services.Storage.Models.Remarks;
using Collectively.Services.Storage.Queries;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Collectively.Services.Storage.Repositories.Queries
{
    public static class RemarkCategoryQueries
    {
        public static IMongoCollection<RemarkCategory> RemarkCategories(this IMongoDatabase database)
            => database.GetCollection<RemarkCategory>();

        public static async Task<RemarkCategory> GetByIdAsync(this IMongoCollection<RemarkCategory> categories, Guid id)
        {
            if (id == Guid.Empty)
                return null;

            return await categories.AsQueryable().FirstOrDefaultAsync(x => x.Id == id);
        }

        public static IMongoQueryable<RemarkCategory> Query(this IMongoCollection<RemarkCategory> categories,
            BrowseRemarkCategories query)
        {
            var values = categories.AsQueryable();

            return values;
        }
    }
}