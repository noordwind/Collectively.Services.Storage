using System.Threading.Tasks;
using Collectively.Common.Types;

using Collectively.Services.Storage.Queries;

namespace Collectively.Services.Storage.Services.Statistics
{
    public interface IStatisticsServiceClient
    {
        Task<Maybe<PagedResult<UserStatisticsDto>>> BrowseUserStatisticsAsync(BrowseUserStatistics query);
        Task<Maybe<UserStatisticsDto>> GetUserStatisticsAsync(GetUserStatistics query);
        Task<Maybe<PagedResult<RemarkStatisticsDto>>> BrowseRemarkStatisticsAsync(BrowseRemarkStatistics query);
        Task<Maybe<RemarkStatisticsDto>> GetRemarkStatisticsAsync(GetRemarkStatistics query);
        Task<Maybe<RemarksCountStatisticsDto>> GetRemarksCountStatisticsAsync(GetRemarksCountStatistics query);
        Task<Maybe<PagedResult<CategoryStatisticsDto>>> BrowseCategoryStatisticsAsync(BrowseCategoryStatistics query);
        Task<Maybe<PagedResult<TagStatisticsDto>>> BrowseTagStatisticsAsync(BrowseTagStatistics query);
    }
}