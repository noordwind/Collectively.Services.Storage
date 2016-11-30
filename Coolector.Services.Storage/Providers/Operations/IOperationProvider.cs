using System;
using System.Threading.Tasks;
using Coolector.Common.Dto.Operations;
using Coolector.Common.Types;

namespace Coolector.Services.Storage.Providers.Operations
{
    public interface IOperationProvider
    {
        Task<Maybe<OperationDto>> GetAsync(Guid requestId);
    }
}