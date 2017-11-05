using System;
using System.Threading.Tasks;
using Collectively.Common.Mongo;
using Collectively.Common.Extensions;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Collectively.Services.Storage.Models.Groups;
using System.Collections.Generic;
using System.Linq;

namespace Collectively.Services.Remarks.Repositories.Queries
{
    public static class GroupRemarkQueries
    {
        public static IMongoCollection<GroupRemark> GroupRemarks(this IMongoDatabase database)
            => database.GetCollection<GroupRemark>();

        public static async Task<GroupRemark> GetAsync(this IMongoCollection<GroupRemark> groupRemarks, 
            Guid groupId)
        {
            if (groupId.IsEmpty())
            {
                return null;
            }

            return await groupRemarks.AsQueryable().FirstOrDefaultAsync(x => x.GroupId == groupId);
        }

        public static async Task<IEnumerable<GroupRemark>> GetAllAsync(this IMongoCollection<GroupRemark> groupRemarks, 
            Guid remarkId)
        {
            if (remarkId.IsEmpty())
            {
                return Enumerable.Empty<GroupRemark>();
            }

            return await groupRemarks
                .AsQueryable()
                .Where(x => x.Remarks.Any(r => r.Id == remarkId))
                .ToListAsync();
        }
    }
}