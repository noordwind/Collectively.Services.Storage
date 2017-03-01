using Collectively.Common.Types;
using System.Threading.Tasks;

namespace Collectively.Services.Storage.Tests.Framework
{
    public interface IStorage
    {
        Task<Maybe<object>> FetchAsync();
        Task<Maybe<PagedResult<object>>> FetchCollectionAsync();
        Task SaveAsync(object obj);
    }
}