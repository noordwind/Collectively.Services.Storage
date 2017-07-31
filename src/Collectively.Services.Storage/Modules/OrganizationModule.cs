using Collectively.Services.Storage.ServiceClients.Queries;
using Collectively.Services.Storage.Models.Groups;
using Collectively.Services.Storage.Providers;

namespace Collectively.Services.Storage.Modules
{
    public class OrganizationModule : ModuleBase
    {
        public OrganizationModule(IOrganizationProvider organizationProvider) 
            : base("organizations")
        {
            Get("", async args => await FetchCollection<BrowseOrganizations, Organization>
                (async x => await organizationProvider.BrowseAsync(x)).HandleAsync());

            Get("{id}", async args => await Fetch<GetOrganization, Organization>
                (async x => await organizationProvider.GetAsync(x.Id)).HandleAsync());
        }
    }
}