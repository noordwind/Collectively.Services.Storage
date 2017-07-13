using System;

namespace Collectively.Services.Storage.Models.Remarks
{
    public class Participant
    {
        public RemarkUser User { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }            
    }
}