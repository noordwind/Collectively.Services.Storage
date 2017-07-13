using Collectively.Services.Storage.ServiceClients.Queries;
using Collectively.Services.Storage.Models.Remarks;
using Collectively.Services.Storage.Providers;

namespace Collectively.Services.Storage.Modules
{
    public class RemarkModule : ModuleBase
    {
        public RemarkModule(IRemarkProvider remarkProvider) : base("remarks")
        {
            Get("", async args => await FetchCollection<BrowseRemarks, Remark>
                (async x => await remarkProvider.BrowseAsync(x)).HandleAsync());

            Get("{id}", async args => await Fetch<GetRemark, Remark>
                (async x => await remarkProvider.GetAsync(x.Id)).HandleAsync());

            Get("categories", async args => await FetchCollection<BrowseRemarkCategories, RemarkCategory>
                (async x => await remarkProvider.BrowseCategoriesAsync(x)).HandleAsync());

            Get("tags", async args => await FetchCollection<BrowseRemarkTags, Tag>
                (async x => await remarkProvider.BrowseTagsAsync(x)).HandleAsync());
        }
    }
}