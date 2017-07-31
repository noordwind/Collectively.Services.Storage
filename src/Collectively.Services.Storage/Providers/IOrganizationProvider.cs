using System;
using System.Threading.Tasks;
using Collectively.Common.Types;
using Collectively.Services.Storage.Models.Groups;
using Collectively.Services.Storage.ServiceClients.Queries;

namespace Collectively.Services.Storage.Providers
{
    public interface IOrganizationProvider
    {
        Task<Maybe<Organization>> GetAsync(Guid id);
        Task<Maybe<PagedResult<Organization>>> BrowseAsync(BrowseOrganizations query);         
    }
}