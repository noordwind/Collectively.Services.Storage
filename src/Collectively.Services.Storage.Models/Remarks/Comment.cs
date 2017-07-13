using System;
using System.Collections.Generic;

namespace Collectively.Services.Storage.Models.Remarks
{
    public class Comment
    {
        public Guid Id { get; set; }
        public RemarkUser User { get; set; }
        public string Text { get; set; }
        public int Rating { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool Removed { get; set; }
        public IList<CommentHistory> History { get; set; }
        public IList<Vote> Votes { get; set; }   
    }
}