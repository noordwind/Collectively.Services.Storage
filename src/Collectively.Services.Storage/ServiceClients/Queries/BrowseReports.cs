using Collectively.Common.Types;

namespace Collectively.Services.Storage.ServiceClients.Queries
{
    public class BrowseReports : PagedQueryBase
    {
        public string Type { get; set; }
    }
}