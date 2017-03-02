using System;
using System.Threading.Tasks;
using Collectively.Common.Types;
using Collectively.Services.Storage.Dto.Operations;

namespace Collectively.Services.Storage.Services.Operations
{
    public interface IOperationServiceClient
    {
        Task<Maybe<OperationDto>> GetAsync(Guid requestId);
    }
}