using System;
using System.Threading.Tasks;
using Coolector.Common.Types;

namespace Coolector.Services.Storage.Services.Operations
{
    public interface IOperationServiceClient
    {
        Task<Maybe<OperationDto>> GetAsync(Guid requestId);
    }
}