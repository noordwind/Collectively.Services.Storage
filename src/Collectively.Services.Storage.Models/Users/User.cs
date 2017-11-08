using System;
using System.Collections.Generic;

namespace Collectively.Services.Storage.Models.Users
{
    public class User : UserInfo
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Provider { get; set; }
        public string ExternalUserId { get; set; }
        public string Culture { get; set; }
        public ISet<Guid> FavoriteRemarks { get; set; }
        public ISet<UserGroup> Groups { get; set; }
        public ISet<UserOrganization> Organizations { get; set; }
    }
}