using System;
using System.Threading.Tasks;
using Collectively.Services.Storage.ServiceClients.Queries;
using Collectively.Common.Types;
using NLog;
using Collectively.Common.ServiceClients;

namespace Collectively.Services.Storage.ServiceClients
{
    public class RemarkServiceClient : IRemarkServiceClient
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IServiceClient _serviceClient;
        private readonly string _name;

        public RemarkServiceClient(IServiceClient serviceClient, string name)
        {
            _serviceClient = serviceClient;
            _name = name;
        }

        public async Task<Maybe<T>> GetAsync<T>(Guid id) where T : class 
        {
            Logger.Debug($"Requesting GetAsync, id:{id}");
            return await _serviceClient
                .GetAsync<T>(_name, $"remarks/{id}");
        }

        public async Task<Maybe<dynamic>> GetAsync(Guid id)
            => await GetAsync<dynamic>(id);

        public async Task<Maybe<PagedResult<T>>> BrowseCategoriesAsync<T>(BrowseRemarkCategories query)
            where T : class 
        {
            Logger.Debug("Requesting BrowseCategoriesAsync");
            return await _serviceClient
                .GetCollectionAsync<T>(_name, "remarks/categories");
        }

        public async Task<Maybe<PagedResult<dynamic>>> BrowseCategoriesAsync(BrowseRemarkCategories query)
            => await BrowseCategoriesAsync<dynamic>(query);

        public async Task<Maybe<PagedResult<T>>> BrowseTagsAsync<T>(BrowseRemarkTags query)
            where T : class 
        {
            Logger.Debug("Requesting BrowseTagsAsync");
            return await _serviceClient
                .GetCollectionAsync<T>(_name, "remarks/tags");
        }

        public async Task<Maybe<PagedResult<dynamic>>> BrowseTagsAsync(BrowseRemarkTags query)
            => await BrowseTagsAsync<dynamic>(query);
    }
}