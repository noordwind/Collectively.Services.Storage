using System;
using System.Threading.Tasks;
using Collectively.Common.Types;
using Collectively.Services.Storage.Models.Operations;

namespace Collectively.Services.Storage.Services
{
    public interface IOperationService
    {
        Task<Maybe<Operation>> GetAsync(Guid requestId);
        Task SetAsync(Operation operation);     
    }
}