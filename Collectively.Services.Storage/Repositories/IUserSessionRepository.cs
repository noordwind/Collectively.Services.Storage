using System;
using System.Threading.Tasks;
using Collectively.Common.Types;
using Collectively.Services.Storage.Dto.Users;

namespace Collectively.Services.Storage.Repositories
{
    public interface IUserSessionRepository
    {
        Task<Maybe<UserSessionDto>> GetByIdAsync(Guid id);
        Task AddAsync(UserSessionDto session);
    }
}