using Collectively.Common.Queries;

namespace Collectively.Services.Storage.ServiceClients.Queries
{
    public class GetUserByName : IQuery
    {
        public string Name { get; set; }
    }
}