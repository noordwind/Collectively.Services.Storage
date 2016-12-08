﻿using System;
using System.Threading.Tasks;
using Coolector.Common.Mongo;
using Coolector.Services.Remarks.Shared.Dto;
using Coolector.Services.Storage.Queries;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Coolector.Services.Storage.Repositories.Queries
{
    public static class RemarkCategoryQueries
    {
        public static IMongoCollection<RemarkCategoryDto> RemarkCategories(this IMongoDatabase database)
            => database.GetCollection<RemarkCategoryDto>();

        public static async Task<RemarkCategoryDto> GetByIdAsync(this IMongoCollection<RemarkCategoryDto> categories, Guid id)
        {
            if (id == Guid.Empty)
                return null;

            return await categories.AsQueryable().FirstOrDefaultAsync(x => x.Id == id);
        }

        public static IMongoQueryable<RemarkCategoryDto> Query(this IMongoCollection<RemarkCategoryDto> categories,
            BrowseRemarkCategories query)
        {
            var values = categories.AsQueryable();

            return values;
        }
    }
}