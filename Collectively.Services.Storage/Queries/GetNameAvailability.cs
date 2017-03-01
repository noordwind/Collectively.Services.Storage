using Collectively.Common.Queries;

namespace Collectively.Services.Storage.Queries
{
    public class GetNameAvailability : IQuery
    {
        public string Name { get; set; }
    }
}