using System;
using System.Threading.Tasks;
using Collectively.Common.Extensions;
using Collectively.Common.Mongo;
using Collectively.Services.Storage.Dto.Operations;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Collectively.Services.Storage.Repositories.Queries
{
    public static class OperationQueries
    {
        public static IMongoCollection<OperationDto> Operations(this IMongoDatabase database)
            => database.GetCollection<OperationDto>();

        public static async Task<OperationDto> GetByRequestIdAsync(this IMongoCollection<OperationDto> operations,
            Guid id)
        {
            if (id.IsEmpty())
                return null;

            return await operations.AsQueryable().FirstOrDefaultAsync(x => x.RequestId == id);
        }
    }
}