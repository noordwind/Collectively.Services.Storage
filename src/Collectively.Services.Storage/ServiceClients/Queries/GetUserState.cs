using Collectively.Common.Queries;

namespace Collectively.Services.Storage.ServiceClients.Queries
{
    public class GetUserState : IQuery
    {
        public string Id { get; set; }
    }
}