using System;
using System.Threading.Tasks;
using Collectively.Common.Mongo;
using Collectively.Common.Types;
using Collectively.Services.Storage.ServiceClients.Queries;
using Collectively.Services.Storage.Repositories.Queries;
using MongoDB.Driver;
using Collectively.Services.Storage.Models.Remarks;

namespace Collectively.Services.Storage.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly IMongoDatabase _database;

        public ReportRepository(IMongoDatabase database)
        {
            _database = database;
        }

        public async Task<Maybe<PagedResult<Report>>> BrowseAsync(BrowseReports query)
            => await _database.Reports()
                    .Query(query)
                    .PaginateAsync();

        public async Task AddAsync(Report report)
            => await _database.Reports().InsertOneAsync(report);
    }
}