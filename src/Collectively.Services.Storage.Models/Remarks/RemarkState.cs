using System;

namespace Collectively.Services.Storage.Models.Remarks
{
    public class RemarkState
    {
        public Guid Id { get; set; }
        public string State { get; set; }
        public RemarkUser User { get; set; }
        public string Description { get; set; }
        public Location Location { get; set; }
        public File Photo { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool Removed { get; set; }
        public int ReportsCount { get; set; }
    }
}