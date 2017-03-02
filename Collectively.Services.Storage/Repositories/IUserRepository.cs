using System.Collections.Generic;
using System.Threading.Tasks;
using Collectively.Common.Types;
using Collectively.Services.Storage.Queries;
using Collectively.Services.Storage.Dto.Users;

namespace Collectively.Services.Storage.Repositories
{
    public interface IUserRepository
    {
        Task<bool> ExistsAsync(string id);
        Task<Maybe<AvailableResourceDto>> IsNameAvailableAsync(string name);
        Task<Maybe<PagedResult<UserDto>>> BrowseAsync(BrowseUsers query);
        Task<Maybe<UserDto>> GetByIdAsync(string id);
        Task<Maybe<UserDto>> GetByNameAsync(string name);
        Task EditAsync(UserDto user);
        Task AddAsync(UserDto user);
        Task AddManyAsync(IEnumerable<UserDto> users);
    }
}