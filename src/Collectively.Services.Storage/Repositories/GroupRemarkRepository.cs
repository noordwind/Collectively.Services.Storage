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

        public async Task<Maybe<GroupRemark>> GetAsync(Guid groupId, Guid remarkId)
            => await _database.GroupRemarks().GetAsync(groupId, remarkId);

        public async Task<IEnumerable<GroupRemark>> GetAllForGroupAsync(Guid groupId)
            => await _database.GroupRemarks().GetAllForGroupAsync(groupId);

        public async Task AddAsync(GroupRemark groupRemark)
            => await _database.GroupRemarks().InsertOneAsync(groupRemark);

        public async Task AddRemarksAsync(Guid remarkId, IEnumerable<Guid> groupIds)
        {
            var groupRemarks = groupIds.Select(x => new GroupRemark 
            {
                Id = Guid.NewGuid(),
                RemarkId = remarkId,
                GroupId = x
            });
            await _database.GroupRemarks().InsertManyAsync(groupRemarks);
        }

        public async Task DeleteAsync(Guid groupId, Guid remarkId)
            => await _database.GroupRemarks().DeleteOneAsync(x => x.GroupId == groupId && x.RemarkId == remarkId);

        public async Task DeleteAllForRemarkAsync(Guid remarkId)
            => await _database.GroupRemarks().DeleteManyAsync(x => x.RemarkId == remarkId);
    }
}