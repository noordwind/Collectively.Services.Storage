using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Collectively.Common.Types;

using Collectively.Services.Storage.Queries;
using Collectively.Services.Storage.Repositories.Queries;
using MongoDB.Driver;

namespace Collectively.Services.Storage.Repositories
{
    public class RemarkRepository : IRemarkRepository
    {
        private readonly IMongoDatabase _database;

        public RemarkRepository(IMongoDatabase database)
        {
            _database = database;
        }

        public async Task<Maybe<RemarkDto>> GetByIdAsync(Guid id)
            => await _database.Remarks().GetByIdAsync(id);

        public async Task<Maybe<PagedResult<RemarkDto>>> BrowseAsync(BrowseRemarks query)
        {
            var results = await _database.Remarks()
                .QueryAsync(query);

            return results;
        }

        public async Task AddAsync(RemarkDto remark)
            => await _database.Remarks().InsertOneAsync(remark);

        public async Task UpdateAsync(RemarkDto remark)
            => await _database.Remarks().ReplaceOneAsync(x => x.Id == remark.Id, remark);

        public async Task UpdateUserNamesAsync(string userId, string name)
        {
            var updateAuthor = Builders<RemarkDto>.Update.Set("author.name", name);
            await _database.Remarks().UpdateManyAsync(x => x.Author.UserId == userId, updateAuthor);

            var updateStateName = Builders<RemarkDto>.Update.Set("state.user.name", name);
            await _database.Remarks().UpdateManyAsync(x => x.State.User.Name == name, updateStateName);
        }

        public async Task AddManyAsync(IEnumerable<RemarkDto> remarks)
            => await _database.Remarks().InsertManyAsync(remarks);

        public async Task DeleteAsync(RemarkDto remark)
            => await _database.Remarks().DeleteOneAsync(x => x.Id == remark.Id);
    }
}