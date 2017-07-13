using Collectively.Common.Queries;

namespace Collectively.Services.Storage.ServiceClients.Queries
{
    public class GetUser : IQuery
    {
        public string Id { get; set; }
    }
}