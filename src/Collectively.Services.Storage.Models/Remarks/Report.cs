using System;

namespace Collectively.Services.Storage.Models.Remarks
{
    public class Report
    {
        public Guid Id { get; set; }
        public Guid RemarkId { get; set; }
        public Guid? ResourceId { get; set; }
        public string Type { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedAt { get; set; }        
    }
}