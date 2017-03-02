using System;
using System.Threading.Tasks;
using Collectively.Common.Types;
using Collectively.Services.Storage.Models.Operations;

namespace Collectively.Services.Storage.Services.Operations
{
    public interface IOperationServiceClient
    {
        Task<Maybe<Operation>> GetAsync(Guid requestId);
    }
}