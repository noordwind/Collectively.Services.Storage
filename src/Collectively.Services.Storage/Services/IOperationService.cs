using System;
using System.Threading.Tasks;
using Collectively.Services.Storage.Models.Operations;

namespace Collectively.Services.Storage.Services
{
    public interface IOperationService
    {
        Task SetAsync(Operation operation);     
    }
}