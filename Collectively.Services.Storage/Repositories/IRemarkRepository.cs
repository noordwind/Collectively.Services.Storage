using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Collectively.Services.Storage.ServiceClients.Queries;
using Collectively.Common.Types;
using Collectively.Services.Storage.Models.Remarks;

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