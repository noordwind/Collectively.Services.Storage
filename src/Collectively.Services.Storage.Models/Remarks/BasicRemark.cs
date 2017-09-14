using System;

namespace Collectively.Services.Storage.Models.Remarks
{
    public class BasicRemark
    {
        public Guid Id { get; set; }
        public RemarkGroup Group { get; set; }
        public RemarkUser Author { get; set; }
        public RemarkCategory Category { get; set; }
        public Location Location { get; set; }
        public RemarkState State { get; set; }
        public string SmallPhotoUrl { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Rating { get; set; }
        public bool Resolved { get; set; }
        public int CommentsCount { get; set; }
        public int ParticipantsCount { get; set; }
        public int OfferingProposalsCount { get; set; }
        public int PositiveVotesCount { get; set; }
        public int NegativeVotesCount { get; set; }
        public int ReportsCount { get; set; }
        public string Status { get; set; }
        public double? Distance { get; set; }
        public Offering Offering { get; set; }
    }
}