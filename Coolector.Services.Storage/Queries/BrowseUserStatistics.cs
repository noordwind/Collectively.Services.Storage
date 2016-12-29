using Coolector.Common.Types;

namespace Coolector.Services.Storage.Queries
{
    public class BrowseUserStatistics : PagedQueryBase
    {
        public string OrderBy { get; set; }
    }
}