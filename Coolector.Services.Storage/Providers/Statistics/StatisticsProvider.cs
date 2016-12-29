using System;
using System.Threading.Tasks;
using Coolector.Common.Types;
using Coolector.Services.Statistics.Shared.Dto;
using Coolector.Services.Storage.Queries;
using Coolector.Services.Storage.Services.Statistics;

namespace Coolector.Services.Storage.Providers.Statistics
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

        public async Task<Maybe<PagedResult<RemarkStatisticsDto>>> BrowseRemarkStatisticsAsync(BrowseRemarkStatistics query)
            => await _providerClient.GetCollectionAsync(
                async () => await _statisticsServiceClient.BrowseRemarkStatisticsAsync(query));

        public async Task<Maybe<PagedResult<UserStatisticsDto>>> BrowseUserStatisticsAsync(BrowseUserStatistics query)
            => await _providerClient.GetCollectionAsync(
                async () => await _statisticsServiceClient.BrowseUserStatisticsAsync(query));

        public async Task<Maybe<RemarkStatisticsDto>> GetRemarkStatisticsAsync(GetRemarkStatistics query)
            => await _providerClient.GetAsync(
                async () => await _statisticsServiceClient.GetRemarkStatisticsAsync(query));

        public async Task<Maybe<UserStatisticsDto>> GetUserStatisticsAsync(GetUserStatistics query)
            => await _providerClient.GetAsync(
                async () => await _statisticsServiceClient.GetUserStatisticsAsync(query));
    }
}