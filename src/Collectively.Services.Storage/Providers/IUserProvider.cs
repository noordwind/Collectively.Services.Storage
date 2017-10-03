﻿using System;
using System.Threading.Tasks;
using Collectively.Services.Storage.ServiceClients.Queries;
using Collectively.Common.Types;
using Collectively.Services.Storage.Models.Users;


namespace Collectively.Services.Storage.Providers
{
    public interface IUserProvider
    {
        Task<Maybe<AvailableResource>> IsAvailableAsync(string name);
        Task<Maybe<PagedResult<User>>> BrowseAsync(BrowseUsers query);
        Task<Maybe<User>> GetAsync(string userId);
        Task<Maybe<string>> GetStateAsync(string userId);
        Task<Maybe<User>> GetByNameAsync(string name);
        Task<Maybe<UserSession>> GetSessionAsync(Guid id);
    }
}