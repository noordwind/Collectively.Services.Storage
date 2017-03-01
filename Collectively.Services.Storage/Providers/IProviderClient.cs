using System;
using System.Threading.Tasks;
using Collectively.Common.Types;

namespace Collectively.Services.Storage.Providers
{
    public interface IProviderClient
    {
        Task<Maybe<T>> GetAsync<T>(params Func<Task<Maybe<T>>>[] fetch) 
            where T : class;

        Task<Maybe<PagedResult<T>>> GetCollectionAsync<T>(params Func<Task<Maybe<PagedResult<T>>>>[] fetch) 
            where T : class;
    }
}