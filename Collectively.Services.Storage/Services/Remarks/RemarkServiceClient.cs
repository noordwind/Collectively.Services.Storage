using System;
using System.Threading.Tasks;
using Collectively.Common.Security;
using Collectively.Common.Types;
using Collectively.Services.Storage.Models.Remarks;
using Collectively.Services.Storage.Queries;
using NLog;

namespace Collectively.Services.Storage.Services.Remarks
{
    public class RemarkServiceClient : IRemarkServiceClient
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IServiceClient _serviceClient;
        private readonly ServiceSettings _settings;

        public RemarkServiceClient(IServiceClient serviceClient, ServiceSettings settings)
        {
            _serviceClient = serviceClient;
            _settings = settings;
            _serviceClient.SetSettings(settings);
        }

        public async Task<Maybe<Remark>> GetAsync(Guid id)
        {
            Logger.Debug($"Requesting GetAsync, id:{id}");
            return await _serviceClient
                .GetAsync<Remark>(_settings.Url, $"remarks/{id}");
        }

        public async Task<Maybe<PagedResult<RemarkCategory>>> BrowseCategoriesAsync(BrowseRemarkCategories query)
        {
            Logger.Debug("Requesting BrowseCategoriesAsync");
            return await _serviceClient
                .GetCollectionAsync<RemarkCategory>(_settings.Url, "remarks/categories");
        }

        public async Task<Maybe<PagedResult<Tag>>> BrowseTagsAsync(BrowseRemarkTags query)
        {
            Logger.Debug("Requesting BrowseTagsAsync");
            return await _serviceClient
                .GetCollectionAsync<Tag>(_settings.Url, "remarks/tags");
        }
    }
}