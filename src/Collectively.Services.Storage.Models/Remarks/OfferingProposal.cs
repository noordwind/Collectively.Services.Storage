using System;

namespace Collectively.Services.Storage.Models.Remarks
{
    public class OfferingProposal
    {
        public Guid Id { get; set; }
        public Guid RemarkId { get; set; }
        public string UserId { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }            
    }
}