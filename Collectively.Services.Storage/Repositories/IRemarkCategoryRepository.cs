using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Collectively.Services.Storage.ServiceClients.Queries;
using Collectively.Common.Types;
using Collectively.Services.Storage.Models.Remarks;

namespace Collectively.Services.Storage.Repositories
{
    public interface IRemarkCategoryRepository
    {
        Task<Maybe<RemarkCategory>> GetByIdAsync(Guid id);
        Task<Maybe<PagedResult<RemarkCategory>>> BrowseAsync(BrowseRemarkCategories query);
        Task AddManyAsync(IEnumerable<RemarkCategory> categories);
    }
}