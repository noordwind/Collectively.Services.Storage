using System.Threading.Tasks;
using Collectively.Common.Extensions;
using Collectively.Common.Security;
using Collectively.Common.Types;
using Collectively.Services.Storage.Models.Statistics;
using Collectively.Services.Storage.Queries;
using NLog;

namespace Collectively.Services.Storage.Services.Statistics
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

        public async Task<Maybe<PagedResult<UserStatistics>>> BrowseUserStatisticsAsync(BrowseUserStatistics query)
        {
            Logger.Debug($"Requesting BrowseReportersAsync, page:{query.Page}, results:{query.Results}");
            var queryString = UserStatisticsEndpoint.ToQueryString(query);
            return await _serviceClient
                .GetCollectionAsync<UserStatistics>(_settings.Url, queryString);
        }

        public async Task<Maybe<UserStatistics>> GetUserStatisticsAsync(GetUserStatistics query)
        {
            Logger.Debug($"Requesting GetUserStatisticsAsync, userId:{query.Id}");
            var endpoint = $"{UserStatisticsEndpoint}/{query.Id}";
            return await _serviceClient
                .GetAsync<UserStatistics>(_settings.Url, endpoint);
        }

        public async Task<Maybe<PagedResult<RemarkStatistics>>> BrowseRemarkStatisticsAsync(BrowseRemarkStatistics query)
        {
            Logger.Debug($"Requesting BrowseRemarkStatisticsAsync, page:{query.Page}, results:{query.Results}");
            var queryString = RemarkStatisticsEndpoint.ToQueryString(query);
            return await _serviceClient
                .GetCollectionAsync<RemarkStatistics>(_settings.Url, queryString);
        }

        public async Task<Maybe<RemarkStatistics>> GetRemarkStatisticsAsync(GetRemarkStatistics query)
        {
            Logger.Debug($"Requesting GetRemarkStatisticsAsync, remarkId:{query.Id}");
            var endpoint = $"{RemarkStatisticsEndpoint}/{query.Id}";
            return await _serviceClient
                .GetAsync<RemarkStatistics>(_settings.Url, endpoint);
        }

        public async Task<Maybe<RemarksCountStatistics>> GetRemarksCountStatisticsAsync(GetRemarksCountStatistics query)
        {
            Logger.Debug($"Requesting GetRemarksCountStatisticsAsync, from:{query.From}, to:{query.To}");
            var endpoint = $"{RemarkStatisticsEndpoint}/general".ToQueryString(query);
            return await _serviceClient
                .GetAsync<RemarksCountStatistics>(_settings.Url, endpoint);
        }

        public async Task<Maybe<PagedResult<CategoryStatistics>>> BrowseCategoryStatisticsAsync(BrowseCategoryStatistics query)
        {
            Logger.Debug($"Requesting BrowseCategoryStatisticsAsync, page:{query.Page}, results:{query.Results}");
            var queryString = CategoryStatisticsEndpoint.ToQueryString(query);
            return await _serviceClient
                .GetCollectionAsync<CategoryStatistics>(_settings.Url, queryString);
        }

        public async Task<Maybe<PagedResult<TagStatistics>>> BrowseTagStatisticsAsync(BrowseTagStatistics query)
        {
            Logger.Debug($"Requesting BrowseTagStatisticsAsync, page:{query.Page}, results:{query.Results}");
            var queryString = TagStatisticsEndpoint.ToQueryString(query);
            return await _serviceClient
                .GetCollectionAsync<TagStatistics>(_settings.Url, queryString);
        }
    }
}