using System;
using System.Threading.Tasks;
using Coolector.Common.Types;
using Coolector.Services.Remarks.Shared.Dto;
using Coolector.Services.Storage.Queries;
using Coolector.Services.Storage.Settings;
using NLog;

namespace Coolector.Services.Storage.Services.Remarks
{
    public class RemarkServiceClient : IRemarkServiceClient
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IServiceClient _serviceClient;
        private readonly ProviderSettings _settings;

        public RemarkServiceClient(IServiceClient serviceClient, ProviderSettings settings)
        {
            _serviceClient = serviceClient;
            _settings = settings;
        }

        public async Task<Maybe<RemarkDto>> GetAsync(Guid id)
        {
            Logger.Debug($"Requesting GetAsync, id:{id}");
            return await _serviceClient
                .GetAsync<RemarkDto>(_settings.RemarksApiUrl, $"remarks/{id}");
        }

        public async Task<Maybe<PagedResult<RemarkCategoryDto>>> BrowseCategoriesAsync(BrowseRemarkCategories query)
        {
            Logger.Debug("Requesting BrowseCategoriesAsync");
            return await _serviceClient
                .GetCollectionAsync<RemarkCategoryDto>(_settings.RemarksApiUrl, "remarks/categories");
        }

        public async Task<Maybe<PagedResult<TagDto>>> BrowseTagsAsync(BrowseRemarkTags query)
        {
            Logger.Debug("Requesting BrowseTagsAsync");
            return await _serviceClient
                .GetCollectionAsync<TagDto>(_settings.RemarksApiUrl, "remarks/tags");
        }
    }
}