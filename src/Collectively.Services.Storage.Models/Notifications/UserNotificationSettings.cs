using System;

namespace Collectively.Services.Storage.Models.Notifications
{
    public class UserNotificationSettings
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Culture { get; set; }
        public NotificationSettings EmailSettings { get; set; }
        public NotificationSettings PushSettings { get; set; }
    }
}