using Coolector.Common.Nancy;
using Coolector.Services.Statistics.Shared.Dto;
using Coolector.Services.Statistics.Shared.Queries;
using Coolector.Services.Storage.Providers.Statistics;

namespace Coolector.Services.Storage.Modules
{
    public class StatisticsModule : ApiModuleBase
    {
        public StatisticsModule(IStatisticsProvider statisticsProvider)
            :base("statistics")
        {
            Get("reporters", async args => await FetchCollection<BrowseReporters, ReporterDto>
                (async x => await statisticsProvider.BrowseReportersAsync(x))
                .HandleAsync());

            Get("resolvers", async args => await FetchCollection<BrowseResolvers, ResolverDto>
                (async x => await statisticsProvider.BrowseResolversAsync(x))
                .HandleAsync());
        }
    }
}