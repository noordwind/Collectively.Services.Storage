using System;
using System.Threading.Tasks;
using Collectively.Common.Types;
using Collectively.Services.Storage.Models.Groups;
using Collectively.Services.Storage.Repositories;
using Collectively.Services.Storage.ServiceClients;
using Collectively.Services.Storage.ServiceClients.Queries;

namespace Collectively.Services.Storage.Providers
{
    public class OrganizationProvider : IOrganizationProvider
    {
        private readonly IProviderClient _provider;
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IGroupServiceClient _serviceClient;

        public OrganizationProvider(IProviderClient provider,
            IOrganizationRepository organizationRepository,
            IGroupServiceClient serviceClient)
        {
            _provider = provider;
            _organizationRepository = organizationRepository;
            _serviceClient = serviceClient;
        }

        public async Task<Maybe<Organization>> GetAsync(Guid id)
            => await _provider.GetAsync(
                async () => await _organizationRepository.GetAsync(id),
                async () => await _serviceClient.GetAsync<Organization>(id));

        public async Task<Maybe<PagedResult<Organization>>> BrowseAsync(BrowseOrganizations query)
            => await _provider.GetCollectionAsync(async () => await _organizationRepository.BrowseAsync(query));

    }
}