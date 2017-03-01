using System;
using Collectively.Common.Queries;

namespace Collectively.Services.Storage.Queries
{
    public class GetUserSession : IQuery
    {
        public Guid Id { get; set; }
    }
}