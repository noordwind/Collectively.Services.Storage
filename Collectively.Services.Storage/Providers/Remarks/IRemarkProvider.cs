using System;
using System.Threading.Tasks;
using Collectively.Common.Types;
using Collectively.Services.Storage.Models.Remarks;
using Collectively.Services.Storage.Queries;

namespace Collectively.Services.Storage.Providers.Remarks
{
    public interface IRemarkProvider
    {
        Task<Maybe<Remark>> GetAsync(Guid id);
        Task<Maybe<PagedResult<Remark>>> BrowseAsync(BrowseRemarks query);
        Task<Maybe<PagedResult<RemarkCategory>>> BrowseCategoriesAsync(BrowseRemarkCategories query);
        Task<Maybe<PagedResult<Tag>>> BrowseTagsAsync(BrowseRemarkTags query);
    }
}