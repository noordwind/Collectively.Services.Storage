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

        private const string UserStatisticsEndpoint = "statistics/users";

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
    }
}