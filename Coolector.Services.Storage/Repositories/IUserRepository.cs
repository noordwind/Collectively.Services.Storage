using System.Collections.Generic;
using System.Threading.Tasks;
using Coolector.Common.Dto.General;
using Coolector.Common.Dto.Users;
using Coolector.Common.Types;
using Coolector.Services.Storage.Queries;

namespace Coolector.Services.Storage.Repositories
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