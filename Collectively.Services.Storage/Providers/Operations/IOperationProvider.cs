using System;
using System.Threading.Tasks;
using Collectively.Common.Types;


namespace Collectively.Services.Storage.Providers.Operations
{
    public interface IOperationProvider
    {
        Task<Maybe<OperationDto>> GetAsync(Guid requestId);
    }
}