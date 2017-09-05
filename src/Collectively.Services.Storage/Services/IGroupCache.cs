using System;
using System.Threading.Tasks;
using Collectively.Services.Storage.Models.Groups;

namespace Collectively.Services.Storage.Services
{
    public interface IGroupCache
    {
        Task AddAsync(Group group);
        Task DeleteAsync(Guid groupId);         
    }
}