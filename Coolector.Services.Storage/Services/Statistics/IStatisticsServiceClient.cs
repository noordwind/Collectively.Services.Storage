using System.Threading.Tasks;
using Coolector.Common.Types;
using Coolector.Services.Statistics.Shared.Dto;
using Coolector.Services.Storage.Queries;

namespace Coolector.Services.Storage.Services.Statistics
{
    public interface IStatisticsServiceClient
    {
        Task<Maybe<PagedResult<UserStatisticsDto>>> BrowseUserStatisticsAsync(BrowseUserStatistics query);
        Task<Maybe<UserStatisticsDto>> GetUserStatisticsAsync(GetUserStatistics query);
        Task<Maybe<PagedResult<RemarkStatisticsDto>>> BrowseRemarkStatisticsAsync(BrowseRemarkStatistics query);
        Task<Maybe<RemarkStatisticsDto>> GetRemarkStatisticsAsync(GetRemarkStatistics query);
    }
}