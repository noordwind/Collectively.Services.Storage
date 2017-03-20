using System;

namespace Collectively.Services.Storage.Models.Users
{
    public class User
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Provider { get; set; }
        public string AvatarUrl { get; set; }
        public string Role { get; set; }
        public string State { get; set; }
        public string ExternalUserId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}