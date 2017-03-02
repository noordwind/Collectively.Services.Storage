using System;
using System.Threading.Tasks;
using Collectively.Common.Types;
using Collectively.Services.Storage.Dto.Operations;

namespace Collectively.Services.Storage.Repositories
{
    public interface IOperationRepository
    {
        Task<Maybe<OperationDto>> GetAsync(Guid requestId);
        Task AddAsync(OperationDto operation);
        Task UpdateAsync(OperationDto operation);
    }
}