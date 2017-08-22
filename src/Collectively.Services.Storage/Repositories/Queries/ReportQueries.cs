using System;
using System.Threading.Tasks;
using Collectively.Common.Extensions;
using Collectively.Common.Mongo;
using Collectively.Services.Storage.Models.Remarks;
using Collectively.Services.Storage.ServiceClients.Queries;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Collectively.Services.Storage.Repositories.Queries
{
   public static class ReportQueries
    {
        public static IMongoCollection<Report> Reports(this IMongoDatabase database)
            => database.GetCollection<Report>();

        public static IMongoQueryable<Report> Query(this IMongoCollection<Report> reports,
            BrowseReports query)
        {
            var values = reports.AsQueryable();
            if(query.Type.NotEmpty())
            {
                values = values.Where(x => x.Type == query.Type);
            }

            return values;
        }
    }
}