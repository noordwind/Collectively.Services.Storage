using System;

using System;

namespace Collectively.Services.Storage.Models.Remarks
{
    public class Offering
    {
        public decimal Price { get; set; }
        public string Currency { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }        
    }
}