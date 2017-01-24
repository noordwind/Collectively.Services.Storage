using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Coolector.Common.Extensions;
using Coolector.Common.Mongo;
using Coolector.Common.Types;
using Coolector.Services.Remarks.Shared;
using Coolector.Services.Remarks.Shared.Dto;
using Coolector.Services.Storage.Queries;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Coolector.Services.Storage.Repositories.Queries
{
    public static class RemarkQueries
    {
        private static readonly int NegativeVotesThreshold = -2;

        public static IMongoCollection<RemarkDto> Remarks(this IMongoDatabase database)
            => database.GetCollection<RemarkDto>();

        public static async Task<RemarkDto> GetByIdAsync(this IMongoCollection<RemarkDto> remarks, Guid id)
        {
            if (id == Guid.Empty)
                return null;

            return await remarks.AsQueryable().FirstOrDefaultAsync(x => x.Id == id);
        }

        public static async Task<PagedResult<RemarkDto>> QueryAsync(this IMongoCollection<RemarkDto> remarks,
            BrowseRemarks query)
        {
            if (!IsLocationProvided(query) && query.AuthorId.Empty() && !query.Latest)
                return PagedResult<RemarkDto>.Empty;

            if (query.Page <= 0)
                query.Page = 1;
            if (query.Results <= 0)
                query.Results = 10;

            var filterBuilder = new FilterDefinitionBuilder<RemarkDto>();
            var filter = FilterDefinition<RemarkDto>.Empty;
            if (IsLocationProvided(query))
            {
                var maxDistance = query.Radius > 0 ? (double?) query.Radius/1000/6378.1 : null;
                filter = filterBuilder.NearSphere(x => x.Location,
                        query.Longitude, query.Latitude, maxDistance);
            }
            if (query.Latest)
                filter = filterBuilder.Where(x => x.Id != Guid.Empty);
            if (query.AuthorId.NotEmpty())
                filter = filter & filterBuilder.Where(x => x.Author.UserId == query.AuthorId);
            if (!query.Description.Empty())
                filter = filter & filterBuilder.Where(x => x.Description.Contains(query.Description));
            if (query.Categories?.Any() == true)
                filter = filter & filterBuilder.Where(x => query.Categories.Contains(x.Category.Name));
            if (query.Tags?.Any() == true)
                filter = filter & filterBuilder.Where(x => x.Tags.Any(y => query.Tags.Contains(y)));
            if (query.State.NotEmpty() && query.State != RemarkStates.All)
            {
                if (query.State == RemarkStates.Resolved)
                    filter = filter & filterBuilder.Where(x => x.Resolved);
                else
                    filter = filter & filterBuilder.Where(x => x.Resolved == false);
            }
            if (!query.Disliked)
            {
                filter = filter & filterBuilder.Where(x => x.Rating > NegativeVotesThreshold);
            }

            var filteredRemarks = remarks.Find(filter);
            var totalCount = await filteredRemarks.CountAsync();
            var totalPages = (int) totalCount / query.Results + 1;
            var findResult = filteredRemarks
                .Skip(query.Results * (query.Page - 1))
                .Limit(query.Results);

            findResult = SortRemarks(query, findResult);

            var result = await findResult.ToListAsync();
            return PagedResult<RemarkDto>.Create(result, query.Page, query.Results, totalPages, totalCount);
        }

        private static IFindFluent<RemarkDto,RemarkDto>  SortRemarks(BrowseRemarks query, 
            IFindFluent<RemarkDto,RemarkDto> findResult)
        {
            if(query.OrderBy.Empty())
            {
                return findResult;
            }
            if(query.SortOrder.Empty())
            {
                query.SortOrder = "ascending";
            }

            query.OrderBy = query.OrderBy.ToLowerInvariant();
            query.SortOrder = query.SortOrder.ToLowerInvariant();

            switch(query.OrderBy)
            {
                case "userid": return SortRemarks(query, findResult, x => x.Author.UserId);
                case "createdat": return SortRemarks(query, findResult, x => x.CreatedAt);
            }

            return findResult;
        }

        private static IFindFluent<RemarkDto,RemarkDto>  SortRemarks(BrowseRemarks query,
            IFindFluent<RemarkDto,RemarkDto> findResult, Expression<Func<RemarkDto, object>> sortBy)
        {
            switch(query.SortOrder)
            {
                case "ascending": return findResult.SortBy(sortBy);
                case "descending": return findResult.SortByDescending(sortBy);
            }

            return findResult;
        }      

        private static bool IsLocationProvided(BrowseRemarks query)
            => (Math.Abs(query.Latitude) <= 0.0000000001 
                || Math.Abs(query.Longitude) <= 0.0000000001) == false;
    }
}