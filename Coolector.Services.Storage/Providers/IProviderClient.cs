using System;
using System.IO;
using System.Threading.Tasks;
using Coolector.Common.Queries;
using Coolector.Common.Types;

namespace Coolector.Services.Storage.Providers
{
    public interface IProviderClient
    {
        Task<Maybe<T>> GetAsync<T>(string url, string endpoint) where T : class;
        Task<Maybe<Stream>> GetStreamAsync(string url, string endpoint);

        Task<Maybe<T>> GetUsingStorageAsync<T>(string url, string endpoint,
            Func<Task<Maybe<T>>> storageFetch, Func<T, Task> storageSave) where T : class;

        Task<Maybe<PagedResult<T>>> GetCollectionAsync<T>(string url, string endpoint) where T : class;

        Task<Maybe<PagedResult<T>>> GetCollectionUsingStorageAsync<T>(string url, string endpoint,
            Func<Task<Maybe<PagedResult<T>>>> storageFetch, Func<PagedResult<T>, Task> storageSave) where T : class;

        Task<Maybe<PagedResult<TResult>>> GetFilteredCollectionAsync<TResult, TQuery>(TQuery query,
            string url, string endpoint) where TResult : class where TQuery : class, IPagedQuery;

        Task<Maybe<PagedResult<TResult>>> GetFilteredCollectionUsingStorageAsync<TResult, TQuery>(TQuery query,
            string url, string endpoint, Func<Task<Maybe<PagedResult<TResult>>>> storageFetch,
            Func<PagedResult<TResult>, Task> storageSave)
            where TResult : class where TQuery : class, IPagedQuery;
    }
}