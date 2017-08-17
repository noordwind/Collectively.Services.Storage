using System;

namespace Collectively.Services.Storage.Models.Users
{
    public class UserInfo
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public string State { get; set; }
        public string AvatarUrl { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}