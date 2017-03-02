using System.Threading.Tasks;
using Collectively.Common.ServiceClients.Queries;
using Collectively.Common.ServiceClients.Statistics;
using Collectively.Common.Types;
using Collectively.Services.Storage.Models.Statistics;

namespace Collectively.Services.Storage.Providers.Statistics
{
    public class StatisticsProvider : IStatisticsProvider
    {
        private readonly IProviderClient _providerClient;
        private readonly IStatisticsServiceClient _statisticsServiceClient;

        public StatisticsProvider(IProviderClient providerClient,
            IStatisticsServiceClient statisticsServiceClient)
        {
            _providerClient = providerClient;
            _statisticsServiceClient = statisticsServiceClient;
        }

        public async Task<Maybe<PagedResult<RemarkStatistics>>> BrowseRemarkStatisticsAsync(BrowseRemarkStatistics query)
            => await _providerClient.GetCollectionAsync(
                async () => await _statisticsServiceClient.BrowseRemarkStatisticsAsync<RemarkStatistics>(query));

        public async Task<Maybe<PagedResult<UserStatistics>>> BrowseUserStatisticsAsync(BrowseUserStatistics query)
            => await _providerClient.GetCollectionAsync(
                async () => await _statisticsServiceClient.BrowseUserStatisticsAsync<UserStatistics>(query));

        public async Task<Maybe<RemarkStatistics>> GetRemarkStatisticsAsync(GetRemarkStatistics query)
            => await _providerClient.GetAsync(
                async () => await _statisticsServiceClient.GetRemarkStatisticsAsync<RemarkStatistics>(query));

        public async Task<Maybe<RemarksCountStatistics>> GetRemarksCountStatisticsAsync(GetRemarksCountStatistics query)
            => await _providerClient.GetAsync(
                async () => await _statisticsServiceClient.GetRemarksCountStatisticsAsync<RemarksCountStatistics>(query));

        public async Task<Maybe<PagedResult<CategoryStatistics>>> BrowseCategoryStatisticsAsync(BrowseCategoryStatistics query)
            => await _providerClient.GetCollectionAsync(
                async () => await _statisticsServiceClient.BrowseCategoryStatisticsAsync<CategoryStatistics>(query));

        public async Task<Maybe<PagedResult<TagStatistics>>> BrowseTagStatisticsAsync(BrowseTagStatistics query)
            => await _providerClient.GetCollectionAsync(
                async () => await _statisticsServiceClient.BrowseTagStatisticsAsync<TagStatistics>(query));

        public async Task<Maybe<UserStatistics>> GetUserStatisticsAsync(GetUserStatistics query)
            => await _providerClient.GetAsync(
                async () => await _statisticsServiceClient.GetUserStatisticsAsync<UserStatistics>(query));
    }
}