using System;
using System.Collections.Generic;

namespace Collectively.Services.Storage.Models.Statistics
{
    public class RemarkStatistics
    {
        public Guid RemarkId { get; set; }
        public User Author { get; set; }
        public string Category { get; set; }
        public Location Location { get; set; }
        public string Description { get; set; }
        public RemarkState State { get; set; }
        public DateTime CreatedAt { get; set; }
        public IList<RemarkState> States { get; set; }
        public IList<string> Tags { get; set; }
        public IList<Vote> Votes { get; set; }
    }
}