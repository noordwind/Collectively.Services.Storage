using System;
using System.Threading.Tasks;
using Collectively.Common.Types;
using Collectively.Services.Storage.Models.Users;
using Collectively.Services.Storage.Queries;


namespace Collectively.Services.Storage.Providers.Users
{
    public interface IUserProvider
    {
        Task<Maybe<AvailableResource>> IsAvailableAsync(string name);
        Task<Maybe<PagedResult<User>>> BrowseAsync(BrowseUsers query);
        Task<Maybe<User>> GetAsync(string userId);
        Task<Maybe<User>> GetByNameAsync(string name);
        Task<Maybe<UserSession>> GetSessionAsync(Guid id);
    }
}