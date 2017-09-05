using System;
using System.Threading.Tasks;
using Collectively.Services.Storage.Models.Groups;

namespace Collectively.Services.Storage.Services
{
    public interface IOrganizationCache
    {
        Task AddAsync(Organization organization);
        Task DeleteAsync(Guid organizationId);             
    }
}