using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Coolector.Common.Types;
using Coolector.Services.Remarks.Shared.Dto;
using Coolector.Services.Storage.Queries;

namespace Coolector.Services.Storage.Repositories
{
    public interface IRemarkCategoryRepository
    {
        Task<Maybe<RemarkCategoryDto>> GetByIdAsync(Guid id);
        Task<Maybe<PagedResult<RemarkCategoryDto>>> BrowseAsync(BrowseRemarkCategories query);
        Task AddManyAsync(IEnumerable<RemarkCategoryDto> categories);
    }
}