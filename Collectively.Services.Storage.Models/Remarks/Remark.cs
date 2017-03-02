using System.Collections.Generic;

namespace Collectively.Services.Storage.Models.Remarks
{
    public class Remark : BasicRemark
    {
        public IList<File> Photos { get; set; }
        public IList<RemarkState> States { get; set; }
        public IList<string> Tags { get; set; }
        public IList<Vote> Votes { get; set; }
    }
}