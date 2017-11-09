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
using System.Linq;
using Collectively.Services.Storage.Models.Users;

namespace Collectively.Services.Storage.Handlers
{
    public class GroupCreatedHandler : IEventHandler<GroupCreated>
    {
        private readonly IHandler _handler;
        private readonly IGroupRepository _groupRepository;
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IUserRepository _userRepository;
        private readonly IGroupServiceClient _groupServiceClient;
        private readonly IGroupCache _groupCache;
        private readonly IOrganizationCache _organizationCache;
        private readonly IUserCache _userCache;

        public GroupCreatedHandler(IHandler handler, 
            IGroupRepository groupRepository,
            IOrganizationRepository organizationRepository,
            IUserRepository userRepository,
            IGroupServiceClient groupServiceClient,
            IGroupCache groupCache,
            IOrganizationCache organizationCache,
            IUserCache userCache)
        {
            _handler = handler;
            _groupRepository = groupRepository;
            _organizationRepository = organizationRepository;
            _userRepository = userRepository;
            _groupServiceClient = groupServiceClient;
            _groupCache = groupCache;
            _organizationCache = organizationCache;
            _userCache = userCache;
        }

        public async Task HandleAsync(GroupCreated @event)
        {
            await _handler
                .Run(async () =>
                {
                    var group = await _groupServiceClient.GetAsync<Group>(@event.GroupId);
                    group.Value.MembersCount = group.Value.Members.Count;
                    await _groupRepository.AddAsync(group.Value);
                    await _groupCache.AddAsync(group.Value);
                    var owner = group.Value.Members.First(x => x.Role == "owner");
                    var user = await _userRepository.GetByIdAsync(owner.UserId);
                    if (user.Value.Groups == null)
                    {
                        user.Value.Groups = new HashSet<UserGroup>();
                    }
                    user.Value.Groups.Add(new UserGroup
                    {
                        Id = group.Value.Id,
                        Name = group.Value.Name,
                        Role = owner.Role,
                        IsActive = owner.IsActive
                    });
                    await _userRepository.EditAsync(user.Value);
                    await _userCache.AddAsync(user.Value);
                    if (!group.Value.OrganizationId.HasValue)
                    {
                        return;
                    }
                    var organization = await _organizationRepository.GetAsync(group.Value.OrganizationId.Value);
                    if (organization.Value.Groups == null)
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