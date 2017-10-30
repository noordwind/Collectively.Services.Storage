using System;
using System.Threading.Tasks;
using Collectively.Common.Mongo;
using Collectively.Common.Extensions;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Collectively.Services.Storage.Models.Groups;

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
    }
}