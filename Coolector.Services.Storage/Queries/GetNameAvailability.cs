using Coolector.Common.Queries;

namespace Coolector.Services.Storage.Queries
{
    public class GetNameAvailability : IQuery
    {
        public string Name { get; set; }
    }
}