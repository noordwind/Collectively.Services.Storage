using System;
using System.Threading.Tasks;
using Collectively.Messages.Events;
using Collectively.Common.Services;
using Collectively.Messages.Events.Groups;
using Collectively.Services.Storage.Models.Groups;
using Collectively.Services.Storage.Repositories;
using System.Collections.Generic;
using Collectively.Messages.Events.Remarks;
using System.Linq;
using Collectively.Services.Storage.Models.Remarks;

namespace Collectively.Services.Storage.Handlers
{
    public class RemarkReportedHandler : IEventHandler<RemarkReported>
    {
        private readonly IHandler _handler;
        private readonly IReportRepository _reportRepository;
        private readonly IRemarkRepository _remarkRepository;

        public RemarkReportedHandler(IHandler handler, 
            IReportRepository reportRepository,
            IRemarkRepository remarkRepository)
        {
            _handler = handler;
            _reportRepository = reportRepository;
            _remarkRepository = remarkRepository;
        }

        public async Task HandleAsync(RemarkReported @event)
        {
            await _handler
                .Run(async () =>
                {
                    var report = new Report
                    {
                        Id = Guid.NewGuid(),
                        RemarkId = @event.RemarkId,
                        ResourceId = @event.ResourceId,
                        UserId = @event.UserId,
                        Type = @event.Type,
                        CreatedAt = DateTime.UtcNow
                    };
                    await _reportRepository.AddAsync(report);
                    var remark = await _remarkRepository.GetByIdAsync(@event.RemarkId);
                    switch(@event.Type)
                    {
                        case "activity": remark.Value.States
                            .Single(x => x.Id == @event.ResourceId.Value).ReportsCount++; break;
                        case "comment": remark.Value.Comments
                            .Single(x => x.Id == @event.ResourceId.Value).ReportsCount++; break;
                        case "remark": remark.Value.ReportsCount++; break;
                    }
                    await _remarkRepository.UpdateAsync(remark.Value);
                })
                .OnError((ex, logger) =>
                {
                    logger.Error(ex, $"Error occured while handling {@event.GetType().Name} event");
                })
                .ExecuteAsync();
        }
    }
}