using System.Threading.Tasks;
using Coolector.Common.Extensions;
using Coolector.Common.Security;
using Coolector.Common.Types;
using Coolector.Services.Statistics.Shared.Dto;
using Coolector.Services.Storage.Queries;
using NLog;

namespace Coolector.Services.Storage.Services.Statistics
{
    public class StatisticsServiceClient : IStatisticsServiceClient
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IServiceClient _serviceClient;
        private readonly ServiceSettings _settings;
        private readonly string UserStatisticsEndpoint = "statistics/users";
        private readonly string RemarkStatisticsEndpoint = "statistics/remarks";
        private readonly string CategoryStatisticsEndpoint = "statistics/categories";
        private readonly string TagStatisticsEndpoint = "statistics/tags";

        public StatisticsServiceClient(IServiceClient serviceClient, ServiceSettings settings)
        {
            _serviceClient = serviceClient;
            _settings = settings;
            _serviceClient.SetSettings(settings);
        }

        public async Task<Maybe<PagedResult<UserStatisticsDto>>> BrowseUserStatisticsAsync(BrowseUserStatistics query)
        {
            Logger.Debug($"Requesting BrowseReportersAsync, page:{query.Page}, results:{query.Results}");
            var queryString = UserStatisticsEndpoint.ToQueryString(query);
            return await _serviceClient
                .GetCollectionAsync<UserStatisticsDto>(_settings.Url, queryString);
        }

        public async Task<Maybe<UserStatisticsDto>> GetUserStatisticsAsync(GetUserStatistics query)
        {
            Logger.Debug($"Requesting GetUserStatisticsAsync, userId:{query.Id}");
            var endpoint = $"{UserStatisticsEndpoint}/{query.Id}";
            return await _serviceClient
                .GetAsync<UserStatisticsDto>(_settings.Url, endpoint);
        }

        public async Task<Maybe<PagedResult<RemarkStatisticsDto>>> BrowseRemarkStatisticsAsync(BrowseRemarkStatistics query)
        {
            Logger.Debug($"Requesting BrowseRemarkStatisticsAsync, page:{query.Page}, results:{query.Results}");
            var queryString = RemarkStatisticsEndpoint.ToQueryString(query);
            return await _serviceClient
                .GetCollectionAsync<RemarkStatisticsDto>(_settings.Url, queryString);
        }

        public async Task<Maybe<RemarkStatisticsDto>> GetRemarkStatisticsAsync(GetRemarkStatistics query)
        {
            Logger.Debug($"Requesting GetRemarkStatisticsAsync, remarkId:{query.Id}");
            var endpoint = $"{RemarkStatisticsEndpoint}/{query.Id}";
            return await _serviceClient
                .GetAsync<RemarkStatisticsDto>(_settings.Url, endpoint);
        }

        public async Task<Maybe<RemarkGeneralStatisticsDto>> GetRemarkGeneralStatisticsAsync(GetRemarkGeneralStatistics query)
        {
            Logger.Debug($"Requesting GetRemarkGeneralStatisticsAsync, from:{query.From}, to:{query.To}");
            var endpoint = $"{RemarkStatisticsEndpoint}/general".ToQueryString(query);
            return await _serviceClient
                .GetAsync<RemarkGeneralStatisticsDto>(_settings.Url, endpoint);
        }

        public async Task<Maybe<PagedResult<CategoryStatisticsDto>>> BrowseCategoryStatisticsAsync(BrowseCategoryStatistics query)
        {
            Logger.Debug($"Requesting BrowseCategoryStatisticsAsync, page:{query.Page}, results:{query.Results}");
            var queryString = CategoryStatisticsEndpoint.ToQueryString(query);
            return await _serviceClient
                .GetCollectionAsync<CategoryStatisticsDto>(_settings.Url, queryString);
        }

        public async Task<Maybe<PagedResult<TagStatisticsDto>>> BrowseTagStatisticsAsync(BrowseTagStatistics query)
        {
            Logger.Debug($"Requesting BrowseTagStatisticsAsync, page:{query.Page}, results:{query.Results}");
            var queryString = TagStatisticsEndpoint.ToQueryString(query);
            return await _serviceClient
                .GetCollectionAsync<TagStatisticsDto>(_settings.Url, queryString);
        }
    }
}