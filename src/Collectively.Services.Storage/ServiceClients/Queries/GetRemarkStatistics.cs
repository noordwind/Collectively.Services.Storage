using System;
using Collectively.Common.Queries;

namespace Collectively.Services.Storage.ServiceClients.Queries
{
    public class GetRemarkStatistics : IQuery
    {
        public Guid Id { get; set; }
    }
}