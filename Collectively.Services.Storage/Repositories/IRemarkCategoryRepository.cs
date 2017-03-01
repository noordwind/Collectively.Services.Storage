using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Collectively.Common.Types;

using Collectively.Services.Storage.Queries;

namespace Collectively.Services.Storage.Repositories
{
    public interface IRemarkCategoryRepository
    {
        Task<Maybe<RemarkCategoryDto>> GetByIdAsync(Guid id);
        Task<Maybe<PagedResult<RemarkCategoryDto>>> BrowseAsync(BrowseRemarkCategories query);
        Task AddManyAsync(IEnumerable<RemarkCategoryDto> categories);
    }
}