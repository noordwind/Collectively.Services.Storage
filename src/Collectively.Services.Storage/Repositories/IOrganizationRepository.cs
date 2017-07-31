using System;
using System.Threading.Tasks;
using Collectively.Common.Types;
using Collectively.Services.Storage.Models.Groups;
using Collectively.Services.Storage.ServiceClients.Queries;

namespace Collectively.Services.Storage.Repositories
{
    public interface IOrganizationRepository
    {
        Task<bool> ExistsAsync(string name); 
        Task<Maybe<Organization>> GetAsync(Guid id);
        Task<Maybe<PagedResult<Organization>>> BrowseAsync(BrowseOrganizations query);
        Task AddAsync(Organization organization);
        Task UpdateAsync(Organization organization);
        Task DeleteAsync(Guid id);         
    }
}