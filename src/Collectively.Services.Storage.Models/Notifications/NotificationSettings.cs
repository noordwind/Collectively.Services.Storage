namespace Collectively.Services.Storage.Models.Notifications
{
    public class NotificationSettings
    {
        public bool Enabled { get; set; }
        public bool RemarkCreated { get; set; }
        public bool RemarkCanceled { get; set; }
        public bool RemarkDeleted { get; set; }
        public bool RemarkProcessed { get; set; }
        public bool RemarkRenewed { get; set; }
        public bool RemarkResolved { get; set; }
        public bool PhotosToRemarkAdded { get; set; }
        public bool CommentAdded { get; set; }
    }
}