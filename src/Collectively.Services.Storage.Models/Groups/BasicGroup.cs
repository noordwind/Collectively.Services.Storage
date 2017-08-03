using System;

namespace Collectively.Services.Storage.Models.Groups
{
    public class BasicGroup
    {
        public Guid Id { get; set; }
        public Guid? OrganizationId { get; set; }
        public string Name { get; set; }  
        public string Codename { get; set; }
        public bool IsPublic { get; set; }
        public string State { get; set; }  
        public DateTime CreatedAt { get; set; }
        public int MembersCount { get; set; }
    }
}