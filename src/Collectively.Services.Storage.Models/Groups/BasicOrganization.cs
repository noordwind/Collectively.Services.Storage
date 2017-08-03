using System;

namespace Collectively.Services.Storage.Models.Groups
{
    public class BasicOrganization
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Codename { get; set; }
        public bool IsPublic { get; set; }   
        public string State { get; set; } 
        public DateTime CreatedAt { get; set; }
        public int MembersCount { get; set; }
        public int GroupsCount { get; set; }       
    }
}