using System;
using System.Threading.Tasks;
using Coolector.Common.Types;
using Coolector.Dto.Operations;

namespace Coolector.Services.Storage.Providers
{
    public interface IOperationProvider
    {
        Task<Maybe<OperationDto>> GetAsync(Guid requestId);
    }
}