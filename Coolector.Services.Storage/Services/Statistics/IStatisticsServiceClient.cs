using System.Threading.Tasks;
using Coolector.Common.Types;
using Coolector.Services.Statistics.Shared.Dto;
using Coolector.Services.Statistics.Shared.Queries;

namespace Coolector.Services.Storage.Services.Statistics
{
    public interface IStatisticsServiceClient
    {
        Task<Maybe<PagedResult<ReporterDto>>> BrowseReportersAsync(BrowseReporters query);
        Task<Maybe<PagedResult<ResolverDto>>> BrowseResolversAsync(BrowseResolvers query);
    }
}