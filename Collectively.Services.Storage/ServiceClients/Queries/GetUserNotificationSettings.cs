using Collectively.Common.Queries;

namespace Collectively.Services.Storage.ServiceClients.Queries
{
    public class GetUserNotificationSettings : IQuery
    {
        public string UserId { get; set; }
    }
}