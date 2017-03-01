using System;
using Collectively.Common.Queries;

namespace Collectively.Services.Storage.Queries
{
    public class GetRemarksCountStatistics : IQuery
    {
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }
}