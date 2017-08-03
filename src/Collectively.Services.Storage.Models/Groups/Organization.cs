using System;
using System.Collections.Generic;

namespace Collectively.Services.Storage.Models.Groups
{
    public class Organization : BasicOrganization
    {
        public SubjectDetails Details { get; set; }
        public DateTime UpdatedAt { get; set; }
        public IList<Member> Members { get; set; }
        public IList<Guid> Groups { get; set; }
        public IDictionary<string, ISet<string>> Criteria { get; set; } 
    }
}