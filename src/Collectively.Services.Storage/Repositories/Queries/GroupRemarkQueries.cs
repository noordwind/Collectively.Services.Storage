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
            Guid groupId, Guid remarkId)
            => await groupRemarks
                .AsQueryable()
                .FirstOrDefaultAsync(x => x.GroupId == groupId && x.RemarkId == remarkId);

        public static async Task<IEnumerable<GroupRemark>> GetAllForGroupAsync(this IMongoCollection<GroupRemark> groupRemarks, 
            Guid groupId)
            => await groupRemarks
                .AsQueryable()
                .Where(x => x.GroupId == groupId)
                .ToListAsync();
    }
}