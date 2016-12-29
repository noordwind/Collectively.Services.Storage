using System.Collections.Generic;
using System.Threading.Tasks;
using Coolector.Common.Types;
using Coolector.Services.Remarks.Shared.Dto;
using Coolector.Services.Storage.Queries;

namespace Coolector.Services.Storage.Repositories
{
    public interface ITagRepository
    {
        Task<Maybe<TagDto>> GetAsync(string name);
        Task<Maybe<PagedResult<TagDto>>> BrowseAsync(BrowseRemarkTags query);
        Task AddManyAsync(IEnumerable<TagDto> tags);         
    }
}