using System.Threading.Tasks;

namespace Collectively.Services.Storage.Services
{
    public interface IAccountStateService
    {
         Task SetAsync(string userId, string state);
    }
}