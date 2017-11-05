using Collectively.Services.Storage.ServiceClients.Queries;
using Collectively.Services.Storage.Models.Remarks;
using Collectively.Services.Storage.Providers;

namespace Collectively.Services.Storage.Modules
{
    public class TagModule : ModuleBase
    {
        public TagModule(IRemarkProvider remarkProvider) : base("tags")
        {
            Get("", async args => await FetchCollection<BrowseTags, Tag>
                (async x => await remarkProvider.BrowseTagsAsync(x)).HandleAsync());
        }
    }
}