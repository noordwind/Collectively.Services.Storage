using System;
using System.Collections.Generic;

namespace Collectively.Services.Storage.Dto.Statistics
{
    public class RemarkStatisticsDto
    {
        public Guid RemarkId { get; set; }
        public UserDto Author { get; set; }
        public string Category { get; set; }
        public LocationDto Location { get; set; }
        public string Description { get; set; }
        public RemarkStateDto State { get; set; }
        public DateTime CreatedAt { get; set; }
        public IList<RemarkStateDto> States { get; set; }
        public IList<string> Tags { get; set; }
        public IList<VoteDto> Votes { get; set; }
    }
}