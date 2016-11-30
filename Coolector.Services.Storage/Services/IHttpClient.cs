using System.Net.Http;
using System.Threading.Tasks;
using Coolector.Common.Types;

namespace Coolector.Services.Storage.Services
{
    public interface IHttpClient
    {
        Task<Maybe<HttpResponseMessage>> GetAsync(string url, string endpoint);
    }
}