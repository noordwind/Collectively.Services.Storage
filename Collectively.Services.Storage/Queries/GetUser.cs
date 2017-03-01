using Collectively.Common.Queries;

namespace Collectively.Services.Storage.Queries
{
    public class GetUser : IQuery
    {
        public string Id { get; set; }
    }
}