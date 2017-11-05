using System.Collections.Generic;
using System.Threading.Tasks;
using Collectively.Services.Storage.Models.Remarks;

namespace Collectively.Services.Storage.Services
{
    public interface ITagService
    {
        Task AddOrUpdateAsync(IEnumerable<Tag> tags);
    }
}