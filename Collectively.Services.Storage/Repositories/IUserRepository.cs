using System.Collections.Generic;
using System.Threading.Tasks;
using Collectively.Services.Storage.ServiceClients.Queries;
using Collectively.Common.Types;
using Collectively.Services.Storage.Models.Users;

namespace Collectively.Services.Storage.Repositories
{
    public interface IUserRepository
    {
        Task<bool> ExistsAsync(string id);
        Task<Maybe<AvailableResource>> IsNameAvailableAsync(string name);
        Task<Maybe<PagedResult<User>>> BrowseAsync(BrowseUsers query);
        Task<Maybe<User>> GetByIdAsync(string id);
        Task<Maybe<User>> GetByNameAsync(string name);
        Task EditAsync(User user);
        Task AddAsync(User user);
        Task AddManyAsync(IEnumerable<User> users);
    }
}