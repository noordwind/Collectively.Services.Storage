using System.Threading.Tasks;
using Coolector.Common.Types;
using Coolector.Services.Statistics.Shared.Dto;
using Coolector.Services.Statistics.Shared.Queries;
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

        public async Task<Maybe<PagedResult<ReporterDto>>> BrowseReportersAsync(BrowseReporters query)
            => await _providerClient.GetCollectionAsync(
                async () => await _statisticsServiceClient.BrowseReportersAsync(query));

        public async Task<Maybe<PagedResult<ResolverDto>>> BrowseResolversAsync(BrowseResolvers query)
            => await _providerClient.GetCollectionAsync(
                async () => await _statisticsServiceClient.BrowseResolversAsync(query));
    }
}