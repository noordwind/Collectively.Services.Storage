namespace Collectively.Services.Storage.Models.Groups
{
    public class Member
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public bool IsActive { get; set; }     
    }
}