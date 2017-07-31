using System;
using System.Threading.Tasks;
using Collectively.Common.Types;
using Collectively.Services.Storage.ServiceClients.Queries;

namespace Collectively.Services.Storage.ServiceClients
{
    public interface IGroupServiceClient
    {
        Task<Maybe<T>> GetAsync<T>(Guid id) where T : class;
        Task<Maybe<dynamic>> GetAsync(Guid id);
        Task<Maybe<T>> GetOrganizationAsync<T>(Guid id) where T : class;
        Task<Maybe<dynamic>> GetOrganizationAsync(Guid id);
    }
}