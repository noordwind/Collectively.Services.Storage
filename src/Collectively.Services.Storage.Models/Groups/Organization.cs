using System;
using System.Collections.Generic;

namespace Collectively.Services.Storage.Models.Groups
{
    public class Organization
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Codename { get; set; }
        public bool IsPublic { get; set; }   
        public string State { get; set; } 
        public SubjectDetails Details { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public IList<Member> Members { get; set; }
        public IDictionary<string,string> Criteria { get; set; } 
        public IList<Guid> Groups { get; set; }
    }
}