using System;
using System.Threading.Tasks;
using Collectively.Messages.Events;
using Collectively.Common.Services;
using Collectively.Messages.Events.Groups;
using Collectively.Services.Storage.Models.Groups;
using Collectively.Services.Storage.Repositories;
using Collectively.Services.Storage.ServiceClients;
using System.Collections.Generic;
using Collectively.Services.Storage.Services;

namespace Collectively.Services.Storage.Handlers
{
    public class GroupCreatedHandler : IEventHandler<GroupCreated>
    {
        private readonly IHandler _handler;
        private readonly IGroupRepository _groupRepository;
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IGroupServiceClient _groupServiceClient;
        private readonly IGroupCache _groupCache;
        private readonly IOrganizationCache _organizationCache;

        public GroupCreatedHandler(IHandler handler, 
            IGroupRepository groupRepository,
            IOrganizationRepository organizationRepository,
            IGroupServiceClient groupServiceClient,
            IGroupCache groupCache,
            IOrganizationCache organizationCache)
        {
            _handler = handler;
            _groupRepository = groupRepository;
            _organizationRepository = organizationRepository;
            _groupServiceClient = groupServiceClient;
            _groupCache = groupCache;
            _organizationCache = organizationCache;
        }

        public async Task HandleAsync(GroupCreated @event)
        {
            await _handler
                .Run(async () =>
                {
                    var group = await _groupServiceClient.GetAsync<Group>(@event.GroupId);
                    group.Value.MembersCount = group.Value.Members?.Count ?? 0;
                    await _groupRepository.AddAsync(group.Value);
                    await _groupCache.AddAsync(group.Value);
                    if(!group.Value.OrganizationId.HasValue)
                    {
                        return;
                    }
                    var organization = await _organizationRepository.GetAsync(group.Value.OrganizationId.Value);
                    if(organization.Value.Groups == null)
                    {
                        organization.Value.Groups = new List<Guid>();
                    }
                    organization.Value.Groups.Add(@event.GroupId);
                    organization.Value.GroupsCount = organization.Value.Groups.Count;
                    await _organizationRepository.UpdateAsync(organization.Value);
                    await _organizationCache.AddAsync(organization.Value);
                })
                .OnError((ex, logger) =>
                {
                    logger.Error(ex, $"Error occured while handling {@event.GetType().Name} event");
                })
                .ExecuteAsync();
        }
    }
}