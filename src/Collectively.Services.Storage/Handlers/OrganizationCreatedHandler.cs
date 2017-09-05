using System.Threading.Tasks;
using Collectively.Messages.Events;
using Collectively.Common.Services;
using Collectively.Messages.Events.Groups;
using Collectively.Services.Storage.Models.Groups;
using Collectively.Services.Storage.Repositories;
using Collectively.Services.Storage.ServiceClients;
using Collectively.Services.Storage.Services;

namespace Collectively.Services.Storage.Handlers
{
    public class OrganizationCreatedHandler : IEventHandler<OrganizationCreated>
    {
        private readonly IHandler _handler;
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IGroupServiceClient _groupServiceClient;
        private readonly IOrganizationCache _cache;

        public OrganizationCreatedHandler(IHandler handler, 
            IOrganizationRepository organizationRepository,
            IGroupServiceClient groupServiceClient,
            IOrganizationCache cache)
        {
            _handler = handler;
            _organizationRepository = organizationRepository;
            _groupServiceClient = groupServiceClient;
            _cache = cache;
        }

        public async Task HandleAsync(OrganizationCreated @event)
        {
            await _handler
                .Run(async () =>
                {
                    var organization = await _groupServiceClient.GetOrganizationAsync<Organization>(@event.OrganizationId);
                    organization.Value.GroupsCount = organization.Value.Groups?.Count ?? 0;
                    organization.Value.MembersCount = organization.Value.Members?.Count ?? 0;
                    await _organizationRepository.AddAsync(organization.Value);
                    await _cache.AddAsync(organization.Value);
                })
                .OnError((ex, logger) =>
                {
                    logger.Error(ex, $"Error occured while handling {@event.GetType().Name} event");
                })
                .ExecuteAsync();
        }
    }
}