using System;
using System.Collections.Generic;

namespace Collectively.Services.Storage.Models.Remarks
{
    public class RemarkGroup
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string MemberRole { get; set; }
        public IList<string> MemberCriteria { get; set; }
        public IDictionary<string, string> Members { get; set; }
        public IDictionary<string,ISet<string>> Criteria { get; set; }
    }
}