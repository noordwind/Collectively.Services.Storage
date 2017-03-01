using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Collectively.Common.Extensions;
using Collectively.Common.Queries;
using Collectively.Common.Security;
using Collectively.Common.Types;
using Newtonsoft.Json;
using NLog;

namespace Collectively.Services.Storage.Services
{
    public class ServiceClient : IServiceClient
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private bool _isAuthenticated = false;
        private ServiceSettings _serviceSettings;
        private readonly IHttpClient _httpClient;
        private readonly IServiceAuthenticatorClient _serviceAuthenticatorClient;

        public ServiceClient(IHttpClient httpClient, IServiceAuthenticatorClient serviceAuthenticatorClient)
        {
            _httpClient = httpClient;
            _serviceAuthenticatorClient = serviceAuthenticatorClient;
        }

        public void SetSettings(ServiceSettings serviceSettings)
        {
            _serviceSettings = serviceSettings;
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
            if (!_isAuthenticated && _serviceSettings != null)
            {
                var token = await _serviceAuthenticatorClient.AuthenticateAsync(_serviceSettings.Url, new Credentials
                {
                    Username = _serviceSettings.Username,
                    Password = _serviceSettings.Password
                });
                if (token.HasNoValue)
                {
                    Logger.Error($"Could not get authentication token for service: '{_serviceSettings.Name}'.");

                    return null;
                }

                _httpClient.SetAuthorizationHeader(token.Value);
                _isAuthenticated = true;
            }

            var response = await _httpClient.GetAsync(url, endpoint);
            if (response.HasNoValue)
                return new Maybe<T>();

            var content = await response.Value.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<T>(content);

            return data;
        }
    }
}