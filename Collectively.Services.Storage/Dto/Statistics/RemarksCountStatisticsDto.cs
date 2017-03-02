namespace Collectively.Services.Storage.Dto.Statistics
{
    public class RemarksCountStatisticsDto
    {
        public int NewCount { get; set; }
        public int ReportedCount { get; set; }
        public int ProcessingCount { get; set; }
        public int ResolvedCount { get; set; }
        public int CanceledCount { get; set; }
        public int DeletedCount { get; set; }
        public int RenewedCount { get; set; }
    }
}