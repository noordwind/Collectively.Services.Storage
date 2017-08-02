using System.Collections.Generic;

namespace Collectively.Services.Storage.Models.Groups
{
    public class SubjectDetails
    {
        public string Description { get; set; }
        public string Location { get; set; }
        public string Website { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string PictureUrl { get; set; }  
        private IList<string> Media { get; set; }
    }
}