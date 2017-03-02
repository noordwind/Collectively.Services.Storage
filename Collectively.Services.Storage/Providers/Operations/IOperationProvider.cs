using System;
using System.Threading.Tasks;
using Collectively.Common.Types;
using Collectively.Services.Storage.Models.Operations;

namespace Collectively.Services.Storage.Providers.Operations
{
    public interface IOperationProvider
    {
        Task<Maybe<Operation>> GetAsync(Guid requestId);
    }
}