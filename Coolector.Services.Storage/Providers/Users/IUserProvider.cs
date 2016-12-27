using System;
using System.Threading.Tasks;
using Coolector.Common.Types;
using Coolector.Services.Storage.Queries;
using Coolector.Services.Users.Shared.Dto;

namespace Coolector.Services.Storage.Providers.Users
{
    public interface IUserProvider
    {
        Task<Maybe<AvailableResourceDto>> IsAvailableAsync(string name);
        Task<Maybe<PagedResult<UserDto>>> BrowseAsync(BrowseUsers query);
        Task<Maybe<UserDto>> GetAsync(string userId);
        Task<Maybe<UserDto>> GetByNameAsync(string name);
        Task<Maybe<UserSessionDto>> GetSessionAsync(Guid id);
    }
}