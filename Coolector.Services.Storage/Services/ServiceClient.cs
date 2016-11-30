using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Coolector.Common.Extensions;
using Coolector.Common.Queries;
using Coolector.Common.Types;
using Newtonsoft.Json;

namespace Coolector.Services.Storage.Services
{
    public class ServiceClient : IServiceClient
    {
        private readonly IHttpClient _httpClient;

        public ServiceClient(IHttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Maybe<T>> GetAsync<T>(string url, string endpoint) where T : class
        {
            var data = await GetDataAsync<T>(url, endpoint);
            if (data.HasNoValue)
                return new Maybe<T>();

            return data;
        }

        public async Task<Maybe<Stream>> GetStreamAsync(string url, string endpoint)
        {
            var response = await _httpClient.GetAsync(url, endpoint);
            if (response.HasNoValue)
                return new Maybe<Stream>();

            return await response.Value.Content.ReadAsStreamAsync();
        }

        public async Task<Maybe<PagedResult<T>>> GetCollectionAsync<T>(string url, string endpoint) where T : class
        {
            var data = await GetDataAsync<IEnumerable<T>>(url, endpoint);
            if (data.HasNoValue)
                return new Maybe<PagedResult<T>>();

            return data.Value.PaginateWithoutLimit();
        }

        public async Task<Maybe<PagedResult<TResult>>> GetFilteredCollectionAsync<TQuery, TResult>(TQuery query, 
            string url, string endpoint) 
            where TQuery : class, IPagedQuery where TResult : class
        {
            var queryString = endpoint.ToQueryString(query);

            return await GetCollectionAsync<TResult>(url, queryString);
        }

        private async Task<Maybe<T>> GetDataAsync<T>(string url, string endpoint) where T : class
        {
            var response = await _httpClient.GetAsync(url, endpoint);
            if (response.HasNoValue)
                return new Maybe<T>();

            var content = await response.Value.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<T>(content);

            return data;
        }
    }
}