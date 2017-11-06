using System;

namespace Collectively.Services.Storage.Models.Remarks
{
    public class RemarkTag
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid DefaultId { get; set; }
        public string Default { get; set; }        
    }
}