using System;
using System.Threading.Tasks;
using Coolector.Common.Dto.General;
using Coolector.Common.Dto.Users;
using Coolector.Common.Types;
using Coolector.Services.Storage.Queries;

namespace Coolector.Services.Storage.Services.Users
{
    public interface IUserServiceClient
    {
        Task<Maybe<AvailableResourceDto>> IsAvailableAsync(string name); 
        Task<Maybe<PagedResult<UserDto>>> BrowseAsync(BrowseUsers query); 
        Task<Maybe<UserDto>> GetAsync(string userId);
        Task<Maybe<UserDto>> GetByNameAsync(string name);
        Task<Maybe<UserSessionDto>> GetSessionAsync(Guid id);
    }
}