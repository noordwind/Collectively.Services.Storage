using System;

namespace Collectively.Services.Storage.Models.Statistics
{
    public class Vote
    {
        public string UserId { get; set; }
        public Guid RemarkId { get; set; }
        public bool Positive { get; set; }
        public bool Deleted { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}