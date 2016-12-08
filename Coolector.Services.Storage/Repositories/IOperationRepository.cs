using System;
using System.Threading.Tasks;
using Coolector.Common.Types;

namespace Coolector.Services.Storage.Repositories
{
    public interface IOperationRepository
    {
        Task<Maybe<OperationDto>> GetAsync(Guid requestId);
        Task AddAsync(OperationDto operation);
        Task UpdateAsync(OperationDto operation);
    }
}