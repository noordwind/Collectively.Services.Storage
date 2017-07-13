using System.Collections.Generic;

namespace Collectively.Services.Storage.Models.Statistics
{
    public class UserStatistics
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public RemarksCountStatistics Remarks { get; set; }
        public IList<Vote> Votes { get; set; }
    }
}