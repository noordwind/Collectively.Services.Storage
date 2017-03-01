using System;
using System.Threading.Tasks;
using Collectively.Common.Types;


namespace Collectively.Services.Storage.Services.Operations
{
    public interface IOperationServiceClient
    {
        Task<Maybe<OperationDto>> GetAsync(Guid requestId);
    }
}