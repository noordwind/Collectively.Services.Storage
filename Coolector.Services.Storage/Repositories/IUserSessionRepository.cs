using System;
using System.Threading.Tasks;
using Coolector.Common.Dto.Users;
using Coolector.Common.Types;

namespace Coolector.Services.Storage.Repositories
{
    public interface IUserSessionRepository
    {
        Task<Maybe<UserSessionDto>> GetByIdAsync(Guid id);
        Task AddAsync(UserSessionDto session);
    }
}