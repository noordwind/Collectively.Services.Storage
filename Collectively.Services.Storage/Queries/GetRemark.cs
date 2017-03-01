using System;
using Collectively.Common.Queries;

namespace Collectively.Services.Storage.Queries
{
    public class GetRemark : IQuery
    {
        public Guid Id { get; set; }
    }
}