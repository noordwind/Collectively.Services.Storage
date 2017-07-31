using System;
using System.Threading.Tasks;
using Collectively.Common.Types;
using Collectively.Services.Storage.Models.Groups;
using Collectively.Services.Storage.Repositories;
using Collectively.Services.Storage.Repositories;
using Collectively.Services.Storage.ServiceClients;
using Collectively.Services.Storage.ServiceClients.Queries;

namespace Collectively.Services.Storage.Providers
{
    public class GroupProvider : IGroupProvider
    {
        private readonly IProviderClient _provider;
        private readonly IGroupRepository _groupRepository;
        private readonly IGroupServiceClient _serviceClient;

        public GroupProvider(IProviderClient provider,
            IGroupRepository groupRepository,
            IGroupServiceClient serviceClient)
        {
            _provider = provider;
            _groupRepository = groupRepository;
            _serviceClient = serviceClient;
        }

        public async Task<Maybe<Group>> GetAsync(Guid id)
            => await _provider.GetAsync(
                async () => await _groupRepository.GetAsync(id),
                async () => await _serviceClient.GetAsync<Group>(id));

        public async Task<Maybe<PagedResult<Group>>> BrowseAsync(BrowseGroups query)
            => await _provider.GetCollectionAsync(async () => await _groupRepository.BrowseAsync(query));

    }
}