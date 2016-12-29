using Coolector.Services.Remarks.Shared.Dto;
using Coolector.Services.Storage.Providers.Remarks;
using Coolector.Services.Storage.Queries;

namespace Coolector.Services.Storage.Modules
{
    public class RemarkModule : ModuleBase
    {
        public RemarkModule(IRemarkProvider remarkProvider) : base("remarks")
        {
            Get("", async args => await FetchCollection<BrowseRemarks, RemarkDto>
                (async x => await remarkProvider.BrowseAsync(x)).HandleAsync());

            Get("{id}", async args => await Fetch<GetRemark, RemarkDto>
                (async x => await remarkProvider.GetAsync(x.Id)).HandleAsync());

            Get("categories", async args => await FetchCollection<BrowseRemarkCategories, RemarkCategoryDto>
                (async x => await remarkProvider.BrowseCategoriesAsync(x)).HandleAsync());

            Get("tags", async args => await FetchCollection<BrowseRemarkTags, TagDto>
                (async x => await remarkProvider.BrowseTagsAsync(x)).HandleAsync());
        }
    }
}