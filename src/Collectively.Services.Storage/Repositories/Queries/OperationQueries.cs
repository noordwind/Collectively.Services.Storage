using System;
using System.Threading.Tasks;
using Collectively.Common.Extensions;
using Collectively.Common.Mongo;
using Collectively.Services.Storage.Models.Operations;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Collectively.Services.Storage.Repositories.Queries
{
    public static class OperationQueries
    {
        public static IMongoCollection<Operation> Operations(this IMongoDatabase database)
            => database.GetCollection<Operation>();

        public static async Task<Operation> GetByRequestIdAsync(this IMongoCollection<Operation> operations,
            Guid id)
        {
            if (id.IsEmpty())
                return null;

            return await operations.AsQueryable().FirstOrDefaultAsync(x => x.RequestId == id);
        }
    }
}