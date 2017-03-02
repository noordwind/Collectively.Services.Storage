using System;
using System.Threading.Tasks;
using Collectively.Common.Types;
using Collectively.Services.Storage.Dto.Remarks;
using Collectively.Services.Storage.Queries;

namespace Collectively.Services.Storage.Services.Remarks
{
    public interface IRemarkServiceClient
    {
        Task<Maybe<RemarkDto>> GetAsync(Guid id);
        Task<Maybe<PagedResult<RemarkCategoryDto>>> BrowseCategoriesAsync(BrowseRemarkCategories query);
        Task<Maybe<PagedResult<TagDto>>> BrowseTagsAsync(BrowseRemarkTags query);
    }
}