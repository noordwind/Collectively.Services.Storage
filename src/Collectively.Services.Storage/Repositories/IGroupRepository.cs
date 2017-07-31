using System;
using System.Threading.Tasks;
using Collectively.Common.Types;
using Collectively.Services.Storage.Models.Groups;
using Collectively.Services.Storage.ServiceClients.Queries;

namespace Collectively.Services.Storage.Repositories
{
    public interface IGroupRepository
    {
        Task<bool> ExistsAsync(string name); 
        Task<Maybe<Group>> GetAsync(Guid id);
        Task<Maybe<PagedResult<Group>>> BrowseAsync(BrowseGroups query);
        Task AddAsync(Group group);
        Task UpdateAsync(Group group);
        Task DeleteAsync(Guid id);         
    }
}