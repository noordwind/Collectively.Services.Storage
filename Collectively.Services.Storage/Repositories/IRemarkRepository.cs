using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Collectively.Common.Types;

using Collectively.Services.Storage.Queries;

namespace Collectively.Services.Storage.Repositories
{
    public interface IRemarkRepository
    {
        Task<Maybe<RemarkDto>> GetByIdAsync(Guid id);
        Task<Maybe<PagedResult<RemarkDto>>> BrowseAsync(BrowseRemarks query);
        Task AddAsync(RemarkDto remark);
        Task UpdateAsync(RemarkDto remark);
        Task UpdateUserNamesAsync(string userId, string name);
        Task AddManyAsync(IEnumerable<RemarkDto> remarks);
        Task DeleteAsync(RemarkDto remark);
    }
}