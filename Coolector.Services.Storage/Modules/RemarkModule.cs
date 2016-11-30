using Coolector.Common.Dto.Remarks;
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

            Get("categories", async args => await FetchCollection<BrowseRemarkCategories, RemarkCategoryDto>
                (async x => await remarkProvider.BrowseCategoriesAsync(x)).HandleAsync());

            Get("{id}", async args => await Fetch<GetRemark, RemarkDto>
                (async x => await remarkProvider.GetAsync(x.Id)).HandleAsync());
        }
    }
}