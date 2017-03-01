using System;
using Collectively.Common.Queries;

namespace Collectively.Services.Storage.Queries
{
    public class GetOperation : IQuery
    {
        public Guid RequestId { get; set; }
    }
}