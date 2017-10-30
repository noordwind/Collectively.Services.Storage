using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Collectively.Common.Types;
using Collectively.Services.Remarks.Repositories.Queries;
using Collectively.Services.Storage.Models.Groups;
using MongoDB.Driver;

namespace Collectively.Services.Storage.Repositories
{
    public class GroupRemarkRepository : IGroupRemarkRepository
    {
        private readonly IMongoDatabase _database;

        public GroupRemarkRepository(IMongoDatabase database)
        {
            _database = database;
        }

        public async Task<Maybe<GroupRemark>> GetAsync(Guid groupId)
            => await _database.GroupRemarks().GetAsync(groupId);

        public async Task AddAsync(GroupRemark groupRemark)
            => await _database.GroupRemarks().InsertOneAsync(groupRemark);

        public async Task AddRemarksAsync(Guid remarkId, IEnumerable<Guid> groupIds)
        {
            var groupRemarkState = new GroupRemarkState
            {
                Id = remarkId
            };
            var filter = Builders<GroupRemark>.Filter.Where(x => groupIds.Contains(x.GroupId));
            var update = Builders<GroupRemark>.Update.AddToSet(x => x.Remarks, groupRemarkState);
            update.AddToSet(x => x.Remarks, groupRemarkState);
            await _database.GroupRemarks().UpdateManyAsync(filter, update);
        }

        public async Task DeleteRemarksAsync(Guid remarkId, IEnumerable<Guid> groupIds)
        {
            //TODO
            await Task.CompletedTask;
        }
    }
}