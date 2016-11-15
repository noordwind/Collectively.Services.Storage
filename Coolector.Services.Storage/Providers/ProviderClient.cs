using System;
using System.IO;
using System.Threading.Tasks;
using Coolector.Common.Queries;
using Coolector.Common.Types;
using Coolector.Common.Extensions;
using NLog;

namespace Coolector.Services.Storage.Providers
{
    public class ProviderClient : IProviderClient
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IServiceClient _serviceClient;

        public ProviderClient(IServiceClient serviceClient)
        {
            _serviceClient = serviceClient;
        }

        public async Task<Maybe<T>> GetAsync<T>(string url, string endpoint) where T : class
            => await _serviceClient.GetAsync<T>(url, endpoint);

        public async Task<Maybe<Stream>> GetStreamAsync(string url, string endpoint)
            => await _serviceClient.GetStreamAsync(url, endpoint);

        public async Task<Maybe<T>> GetUsingStorageAsync<T>(string url, string endpoint,
            Func<Task<Maybe<T>>> storageFetch, Func<T, Task> storageSave) where T : class
        {
            if (storageFetch != null)
            {
                var data = await storageFetch();
                if (data.HasValue)
                    return data;
            }

            var response = await GetAsync<T>(url, endpoint);
            if (response.HasNoValue)
                return new Maybe<T>();

            if (storageSave != null)
                await storageSave(response.Value);

            return response.Value;
        }

        public Task<Maybe<PagedResult<T>>> GetCollectionAsync<T>(string url, string endpoint) where T : class
            => _serviceClient.GetCollectionAsync<T>(url, endpoint);

        public async Task<Maybe<PagedResult<T>>> GetCollectionUsingStorageAsync<T>(string url, string endpoint, 
            Func<Task<Maybe<PagedResult<T>>>> storageFetch, Func<PagedResult<T>, Task> storageSave) where T : class
        {
            if (storageFetch != null)
            {
                var data = await storageFetch();
                if (data.HasValue && data.Value.IsNotEmpty)
                    return data;
            }

            var response = await GetCollectionAsync<T>(url, endpoint);
            if (response.HasNoValue)
                return response;

            if (storageSave != null && response.Value.IsNotEmpty)
                await storageSave(response.Value);

            return response;
        }

        public async Task<Maybe<PagedResult<TResult>>> GetFilteredCollectionAsync<TResult, TQuery>(TQuery query,
            string url, string endpoint) where TResult : class where TQuery : class, IPagedQuery
        {
            Logger.Debug($"Get filtered data from service, endpoint: {endpoint}, queryType: {typeof(TQuery).Name}");
            var queryString = endpoint.ToQueryString(query);
            var results = await GetCollectionAsync<TResult>(url, queryString);
            if (results.HasNoValue || results.Value.IsEmpty)
                return PagedResult<TResult>.Empty;

            return results;
        }

        public async Task<Maybe<PagedResult<TResult>>> GetFilteredCollectionUsingStorageAsync<TResult, TQuery>(TQuery query,
            string url, string endpoint, Func<Task<Maybe<PagedResult<TResult>>>> storageFetch,
            Func<PagedResult<TResult>, Task> storageSave) where TResult : class where TQuery : class, IPagedQuery
        {
            if (storageFetch != null)
            {
                var data = await storageFetch();
                if (data.HasValue && data.Value.IsNotEmpty)
                    return data;
            }

            var response = await GetFilteredCollectionAsync<TResult,TQuery>(query, url, endpoint);
            if (response.HasNoValue)
                return response;

            if (storageSave != null && response.Value.IsNotEmpty)
                await storageSave(response.Value);

            return response;
        }
    }
}