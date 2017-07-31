using System;

namespace Collectively.Services.Storage.Models.Groups
{
    public class Group
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Codename { get; set; }
        public bool IsPublic { get; set; }        
    }
}