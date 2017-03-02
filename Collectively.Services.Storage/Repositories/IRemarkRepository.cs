using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Collectively.Common.Types;
using Collectively.Services.Storage.Models.Remarks;
using Collectively.Services.Storage.Queries;

namespace Collectively.Services.Storage.Repositories
{
    public interface IRemarkRepository
    {
        Task<Maybe<Remark>> GetByIdAsync(Guid id);
        Task<Maybe<PagedResult<Remark>>> BrowseAsync(BrowseRemarks query);
        Task AddAsync(Remark remark);
        Task UpdateAsync(Remark remark);
        Task UpdateUserNamesAsync(string userId, string name);
        Task AddManyAsync(IEnumerable<Remark> remarks);
        Task DeleteAsync(Remark remark);
    }
}