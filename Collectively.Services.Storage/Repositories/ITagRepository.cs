using System.Collections.Generic;
using System.Threading.Tasks;
using Collectively.Common.Types;

using Collectively.Services.Storage.Queries;

namespace Collectively.Services.Storage.Repositories
{
    public interface ITagRepository
    {
        Task<Maybe<TagDto>> GetAsync(string name);
        Task<Maybe<PagedResult<TagDto>>> BrowseAsync(BrowseRemarkTags query);
        Task AddManyAsync(IEnumerable<TagDto> tags);         
    }
}