using System;
using System.Threading.Tasks;
using Coolector.Common.Dto.Remarks;
using Coolector.Common.Types;
using Coolector.Services.Storage.Queries;

namespace Coolector.Services.Storage.Providers.Remarks
{
    public interface IRemarkProvider
    {
        Task<Maybe<RemarkDto>> GetAsync(Guid id);
        Task<Maybe<PagedResult<RemarkDto>>> BrowseAsync(BrowseRemarks query);
        Task<Maybe<PagedResult<RemarkCategoryDto>>> BrowseCategoriesAsync(BrowseRemarkCategories query);
    }
}