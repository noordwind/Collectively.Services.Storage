using System.Threading.Tasks;
using Collectively.Common.Types;
using Collectively.Services.Storage.Models.Remarks;
using Collectively.Services.Storage.ServiceClients.Queries;

namespace Collectively.Services.Storage.Repositories
{
    public interface IReportRepository
    {
         Task<Maybe<PagedResult<Report>>> BrowseAsync(BrowseReports query);
         Task AddAsync(Report report);         
    }
}