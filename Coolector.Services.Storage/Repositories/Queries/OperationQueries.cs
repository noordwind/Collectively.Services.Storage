using System;
using System.Threading.Tasks;
using Coolector.Dto.Operations;
using Coolector.Common.Extensions;
using Coolector.Common.Mongo;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Coolector.Services.Storage.Repositories.Queries
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