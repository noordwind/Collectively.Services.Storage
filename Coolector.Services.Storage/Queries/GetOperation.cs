using System;
using Coolector.Common.Queries;

namespace Coolector.Services.Storage.Queries
{
    public class GetOperation : IQuery
    {
        public Guid RequestId { get; set; }
    }
}