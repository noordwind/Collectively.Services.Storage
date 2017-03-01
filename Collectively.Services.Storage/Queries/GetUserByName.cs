using Collectively.Common.Queries;

namespace Collectively.Services.Storage.Queries
{
    public class GetUserByName : IQuery
    {
        public string Name { get; set; }
    }
}