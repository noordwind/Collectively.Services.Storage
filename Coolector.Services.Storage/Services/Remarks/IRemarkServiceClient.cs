using System;
using System.Threading.Tasks;
using Coolector.Common.Types;
using Coolector.Services.Remarks.Shared.Dto;
using Coolector.Services.Storage.Queries;

namespace Coolector.Services.Storage.Services.Remarks
{
    public interface IRemarkServiceClient
    {
        Task<Maybe<RemarkDto>> GetAsync(Guid id);
        Task<Maybe<PagedResult<RemarkCategoryDto>>> BrowseCategoriesAsync(BrowseRemarkCategories query);
        Task<Maybe<PagedResult<TagDto>>> BrowseTagsAsync(BrowseRemarkTags query);
    }
}