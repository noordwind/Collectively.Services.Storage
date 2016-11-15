using System;
using System.Threading.Tasks;
using Coolector.Common.Types;
using Coolector.Dto.Users;

namespace Coolector.Services.Storage.Repositories
{
    public interface IUserSessionRepository
    {
        Task<Maybe<UserSessionDto>> GetByIdAsync(Guid id);
        Task AddAsync(UserSessionDto session);
    }
}