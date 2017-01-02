using System;
using System.Threading.Tasks;
using Coolector.Common.Extensions;
using Coolector.Common.Types;
using Coolector.Services.Statistics.Shared.Dto;
using Coolector.Services.Storage.Queries;
using Coolector.Services.Storage.Settings;
using NLog;

namespace Coolector.Services.Storage.Services.Statistics
{
    public class StatisticsServiceClient : IStatisticsServiceClient
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IServiceClient _serviceClient;
        private readonly ProviderSettings _settings;
        private readonly string UserStatisticsEndpoint = "statistics/users";
        private readonly string RemarkStatisticsEndpoint = "statistics/remarks";

        public StatisticsServiceClient(IServiceClient serviceClient, ProviderSettings settings)
        {
            _serviceClient = serviceClient;
            _settings = settings;
        }

        public async Task<Maybe<PagedResult<UserStatisticsDto>>> BrowseUserStatisticsAsync(BrowseUserStatistics query)
        {
            Logger.Debug($"Requesting BrowseReportersAsync, page:{query.Page}, results:{query.Results}");
            var queryString = UserStatisticsEndpoint.ToQueryString(query);
            return await _serviceClient
                .GetCollectionAsync<UserStatisticsDto>(_settings.StatisticsApiUrl, queryString);
        }

        public async Task<Maybe<UserStatisticsDto>> GetUserStatisticsAsync(GetUserStatistics query)
        {
            Logger.Debug($"Requesting GetUserStatisticsAsync, userId:{query.Id}");
            var endpoint = $"{UserStatisticsEndpoint}/{query.Id}";
            return await _serviceClient
                .GetAsync<UserStatisticsDto>(_settings.StatisticsApiUrl, endpoint);
        }

        public async Task<Maybe<PagedResult<RemarkStatisticsDto>>> BrowseRemarkStatisticsAsync(BrowseRemarkStatistics query)
        {
            Logger.Debug($"Requesting BrowseRemarkStatisticsAsync, page:{query.Page}, results:{query.Results}");
            var queryString = RemarkStatisticsEndpoint.ToQueryString(query);
            return await _serviceClient
                .GetCollectionAsync<RemarkStatisticsDto>(_settings.StatisticsApiUrl, queryString);
        }

        public async Task<Maybe<RemarkStatisticsDto>> GetRemarkStatisticsAsync(GetRemarkStatistics query)
        {
            Logger.Debug($"Requesting GetRemarkStatisticsAsync, remarkId:{query.Id}");
            var endpoint = $"{RemarkStatisticsEndpoint}/{query.Id}";
            return await _serviceClient
                .GetAsync<RemarkStatisticsDto>(_settings.StatisticsApiUrl, endpoint);
        }

        public async Task<Maybe<RemarkGeneralStatisticsDto>> GetRemarkGeneralStatisticsAsync(GetRemarkGeneralStatistics query)
        {
            Logger.Debug($"Requesting GetRemarkGeneralStatisticsAsync, from:{query.From}, to:{query.To}");
            var endpoint = $"{RemarkStatisticsEndpoint}/general".ToQueryString(query);
            return await _serviceClient
                .GetAsync<RemarkGeneralStatisticsDto>(_settings.StatisticsApiUrl, endpoint);
        }
    }
}