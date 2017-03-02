using System;

namespace Collectively.Services.Storage.Dto.Remarks
{
    public class VoteDto
    {
        public string UserId { get; set; }
        public bool Positive { get; set; }
        public DateTime CreatedAt { get; set; }        
    }
}