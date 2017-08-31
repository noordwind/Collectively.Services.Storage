using System;
using System.Threading.Tasks;
using Collectively.Services.Storage.Models.Remarks;

namespace Collectively.Services.Storage.Services
{
    public interface IRemarkCache
    {
        Task AddAsync(Remark remark, bool addGeo = false, bool addLatest = false);
        Task DeleteAsync(Guid remarkId, bool deleteGeo = false, bool deleteLatest = false);
    }
}