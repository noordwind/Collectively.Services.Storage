using System;

namespace Collectively.Services.Storage.Models.Statistics
{
    public class RemarkState
    {
        public string State { get; set; }
        public string UserId { get; set; }
        public string Description { get; set; }
        public Location Location { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}