using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Collectively.Common.Types;
using Collectively.Services.Storage.Models.Groups;

namespace Collectively.Services.Storage.Repositories
{
    public interface IGroupRemarkRepository
    {
        Task<Maybe<GroupRemark>> GetAsync(Guid groupId, Guid remarkId);
        Task<IEnumerable<GroupRemark>> GetAllForGroupAsync(Guid groupId);
        Task AddAsync(GroupRemark groupRemark);
        Task AddRemarksAsync(Guid remarkId, IEnumerable<Guid> groupIds);
        Task DeleteAsync(Guid groupId, Guid remarkId);
        Task DeleteAllForRemarkAsync(Guid remarkId);
    }
}