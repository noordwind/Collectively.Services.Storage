using System;

namespace Collectively.Services.Storage.Models.Users
{
    public class UserGroup
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public bool IsActive { get; set; }
    }
}