using System;
using System.Collections.Generic;

namespace Collectively.Services.Storage.Models.Groups
{
    public class GroupRemark
    {
        public Guid GroupId { get; set; }
        public ISet<GroupRemarkState> Remarks { get; set; }
    }
}