using System.Threading.Tasks;
using Coolector.Common.Types;
using Coolector.Services.Statistics.Shared.Dto;
using Coolector.Services.Statistics.Shared.Queries;
using Coolector.Services.Storage.Settings;
using NLog;

namespace Coolector.Services.Storage.Services.Statistics
{
    public class StatisticsServiceClient : IStatisticsServiceClient
    {
        private readonly IServiceClient _serviceClient;
        private readonly ProviderSettings _settings;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public StatisticsServiceClient(IServiceClient serviceClient, ProviderSettings settings)
        {
            _serviceClient = serviceClient;
            _settings = settings;
        }

        public async Task<Maybe<PagedResult<ReporterDto>>> BrowseReportersAsync(BrowseReporters query)
        {
            Logger.Debug($"Requesting BrowseReportersAsync, page:{query.Page}, results:{query.Results}");
            return await _serviceClient
                .GetCollectionAsync<ReporterDto>(_settings.StatisticsApiUrl, "statistics/reporters");
        }

        public async Task<Maybe<PagedResult<ResolverDto>>> BrowseResolversAsync(BrowseResolvers query)
        {
            Logger.Debug($"Requesting BrowseResolversAsync, page:{query.Page}, results:{query.Results}");
            return await _serviceClient
                .GetCollectionAsync<ResolverDto>(_settings.StatisticsApiUrl, "statistics/resolvers");
        }
    }
}