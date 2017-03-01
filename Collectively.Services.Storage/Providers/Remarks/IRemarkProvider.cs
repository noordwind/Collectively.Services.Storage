using System;
using System.Threading.Tasks;
using Collectively.Common.Types;

using Collectively.Services.Storage.Queries;

namespace Collectively.Services.Storage.Providers.Remarks
{
    public interface IRemarkProvider
    {
        Task<Maybe<RemarkDto>> GetAsync(Guid id);
        Task<Maybe<PagedResult<RemarkDto>>> BrowseAsync(BrowseRemarks query);
        Task<Maybe<PagedResult<RemarkCategoryDto>>> BrowseCategoriesAsync(BrowseRemarkCategories query);
        Task<Maybe<PagedResult<TagDto>>> BrowseTagsAsync(BrowseRemarkTags query);
    }
}