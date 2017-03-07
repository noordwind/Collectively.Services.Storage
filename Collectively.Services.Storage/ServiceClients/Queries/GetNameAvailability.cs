using Collectively.Common.Queries;

namespace Collectively.Services.Storage.ServiceClients.Queries
{
    public class GetNameAvailability : IQuery
    {
        public string Name { get; set; }
    }
}