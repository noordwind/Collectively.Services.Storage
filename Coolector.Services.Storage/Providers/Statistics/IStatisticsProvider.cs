using System.Threading.Tasks;
using Coolector.Common.Types;
using Coolector.Services.Statistics.Shared.Dto;
using Coolector.Services.Statistics.Shared.Queries;

namespace Coolector.Services.Storage.Providers.Statistics
{
    public interface IStatisticsProvider
    {
        Task<Maybe<PagedResult<UserStatisticsDto>>> BrowseUserStatisticsAsync(BrowseUserStatistics query);
        Task<Maybe<UserStatisticsDto>> GetUserStatisticsAsync(GetUserStatistics query);
    }
}