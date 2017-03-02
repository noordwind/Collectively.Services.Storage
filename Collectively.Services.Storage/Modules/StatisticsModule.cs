using Collectively.Common.ServiceClients.Queries;
using Collectively.Services.Storage.Models.Statistics;
using Collectively.Services.Storage.Providers.Statistics;

namespace Collectively.Services.Storage.Modules
{
    public class StatisticsModule : ModuleBase
    {
        public StatisticsModule(IStatisticsProvider statisticsProvider)
            :base("statistics")
        {
            Get("remarks", async args => await FetchCollection<BrowseRemarkStatistics, RemarkStatistics>
                (async x => await statisticsProvider.BrowseRemarkStatisticsAsync(x))
                .HandleAsync());

            Get("remarks/{id}", async args => await Fetch<GetRemarkStatistics, RemarkStatistics>
                (async x => await statisticsProvider.GetRemarkStatisticsAsync(x))
                .HandleAsync());

            Get("remarks/general", async args => await Fetch<GetRemarksCountStatistics, RemarksCountStatistics>
                (async x => await statisticsProvider.GetRemarksCountStatisticsAsync(x))
                .HandleAsync());

            Get("categories", async args => await FetchCollection<BrowseCategoryStatistics, CategoryStatistics>
                (async x => await statisticsProvider.BrowseCategoryStatisticsAsync(x))
                .HandleAsync());

            Get("tags", async args => await FetchCollection<BrowseTagStatistics, TagStatistics>
                (async x => await statisticsProvider.BrowseTagStatisticsAsync(x))
                .HandleAsync());

            Get("users", async args => await FetchCollection<BrowseUserStatistics, UserStatistics>
                (async x => await statisticsProvider.BrowseUserStatisticsAsync(x))
                .HandleAsync());

            Get("users/{id}", async args => await Fetch<GetUserStatistics, UserStatistics>
                (async x => await statisticsProvider.GetUserStatisticsAsync(x))
                .HandleAsync());
        }
    }
}