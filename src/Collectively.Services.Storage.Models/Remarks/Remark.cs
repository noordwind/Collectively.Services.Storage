using System;
using System.Collections.Generic;

namespace Collectively.Services.Storage.Models.Remarks
{
    public class Remark : BasicRemark
    {
        public IList<File> Photos { get; set; }
        public IList<RemarkState> States { get; set; }
        public IList<Vote> Votes { get; set; }
        public IList<Comment> Comments { get; set; }
        public ISet<string> UserFavorites { get; set; }
        public ISet<Participant> Participants { get; set; }
        public ISet<Guid> AvailableGroups { get; set; }
    }
}