using Collectively.Services.Storage.ServiceClients.Queries;
using Collectively.Services.Storage.Models.Groups;
using Collectively.Services.Storage.Providers;

namespace Collectively.Services.Storage.Modules
{
    public class GroupModule : ModuleBase
    {
        public GroupModule(IGroupProvider groupProvider) 
            : base("groups")
        {
            Get("", async args => await FetchCollection<BrowseGroups, Group>
                (async x => await groupProvider.BrowseAsync(x)).HandleAsync());

            Get("{id}", async args => await Fetch<GetGroup, Group>
                (async x => await groupProvider.GetAsync(x.Id)).HandleAsync());
        }
    }
}