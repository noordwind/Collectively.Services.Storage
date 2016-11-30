using Coolector.Common.Types;
using System.Threading.Tasks;

namespace Coolector.Services.Storage.Tests.Framework
{
    public interface IStorage
    {
        Task<Maybe<object>> FetchAsync();
        Task<Maybe<PagedResult<object>>> FetchCollectionAsync();
        Task SaveAsync(object obj);
    }
}