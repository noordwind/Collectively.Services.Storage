using System;
using System.Threading.Tasks;
using Collectively.Common.Types;
using Collectively.Services.Storage.Models.Operations;

namespace Collectively.Services.Storage.Services
{
    public interface IOperationCache
    {
        Task<Maybe<Operation>> GetAsync(Guid requestId);
        Task AddAsync(Operation operation);     
    }
}