using System.Net.Http;
using System.Threading.Tasks;
using Coolector.Common.Types;

namespace Coolector.Services.Storage.Services
{
    public interface IHttpClient
    {
        void SetAuthorizationHeader(string token);
        Task<Maybe<HttpResponseMessage>> GetAsync(string url, string endpoint);
    }
}