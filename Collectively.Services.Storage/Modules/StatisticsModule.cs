
using Collectively.Services.Storage.Providers.Statistics;
using Collectively.Services.Storage.Queries;

namespace Collectively.Services.Storage.Modules
{
    public class StatisticsModule : ModuleBase
    {
        public StatisticsModule(IStatisticsProvider statisticsProvider)
            :base("statistics")
        {
            Get("remarks", async args => await FetchCollection<BrowseRemarkStatistics, RemarkStatisticsDto>
                (async x => await statisticsProvider.BrowseRemarkStatisticsAsync(x))
                .HandleAsync());

            Get("remarks/{id}", async args => await Fetch<GetRemarkStatistics, RemarkStatisticsDto>
                (async x => await statisticsProvider.GetRemarkStatisticsAsync(x))
                .HandleAsync());

            Get("remarks/general", async args => await Fetch<GetRemarksCountStatistics, RemarksCountStatisticsDto>
                (async x => await statisticsProvider.GetRemarksCountStatisticsAsync(x))
                .HandleAsync());

            Get("categories", async args => await FetchCollection<BrowseCategoryStatistics, CategoryStatisticsDto>
                (async x => await statisticsProvider.BrowseCategoryStatisticsAsync(x))
                .HandleAsync());

            Get("tags", async args => await FetchCollection<BrowseTagStatistics, TagStatisticsDto>
                (async x => await statisticsProvider.BrowseTagStatisticsAsync(x))
                .HandleAsync());

            Get("users", async args => await FetchCollection<BrowseUserStatistics, UserStatisticsDto>
                (async x => await statisticsProvider.BrowseUserStatisticsAsync(x))
                .HandleAsync());

            Get("users/{id}", async args => await Fetch<GetUserStatistics, UserStatisticsDto>
                (async x => await statisticsProvider.GetUserStatisticsAsync(x))
                .HandleAsync());
        }
    }
}