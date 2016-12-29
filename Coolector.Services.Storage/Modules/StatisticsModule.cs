using Coolector.Common.Nancy;
using Coolector.Services.Statistics.Shared.Dto;
using Coolector.Services.Storage.Providers.Statistics;
using Coolector.Services.Storage.Queries;

namespace Coolector.Services.Storage.Modules
{
    public class StatisticsModule : ApiModuleBase
    {
        public StatisticsModule(IStatisticsProvider statisticsProvider)
            :base("statistics")
        {
            Get("users", async args => await FetchCollection<BrowseUserStatistics, UserStatisticsDto>
                (async x => await statisticsProvider.BrowseUserStatisticsAsync(x))
                .HandleAsync());

            Get("users/{id}", async args => await Fetch<GetUserStatistics, UserStatisticsDto>
                (async x => await statisticsProvider.GetUserStatisticsAsync(x))
                .HandleAsync());
        }
    }
}