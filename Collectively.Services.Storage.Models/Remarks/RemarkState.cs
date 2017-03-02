using System;

namespace Collectively.Services.Storage.Models.Remarks
{
    public class RemarkState
    {
        public string State { get; set; }
        public RemarkUser User { get; set; }
        public string Description { get; set; }
        public Location Location { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}