using System.Threading.Tasks;
using Collectively.Messages.Events;
using Collectively.Common.Services;
using Collectively.Messages.Events.Groups;
using Collectively.Services.Storage.Models.Groups;
using Collectively.Services.Storage.Repositories;
using Collectively.Services.Storage.ServiceClients;
using Collectively.Services.Storage.Services;
using System.Linq;
using System.Collections.Generic;
using Collectively.Services.Storage.Models.Users;

namespace Collectively.Services.Storage.Handlers
{
    public class OrganizationCreatedHandler : IEventHandler<OrganizationCreated>
    {
        private readonly IHandler _handler;
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IUserRepository _userRepository;
        private readonly IGroupServiceClient _groupServiceClient;
        private readonly IOrganizationCache _organizationCache;
        private readonly IUserCache _userCache;

        public OrganizationCreatedHandler(IHandler handler, 
            IOrganizationRepository organizationRepository,
            IUserRepository userRepository,
            IGroupServiceClient groupServiceClient,
            IOrganizationCache organizationCache,
            IUserCache userCache)
        {
            _handler = handler;
            _organizationRepository = organizationRepository;
            _userRepository = userRepository;
            _groupServiceClient = groupServiceClient;
            _organizationCache = organizationCache;
            _userCache = userCache;
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
                    await _organizationCache.AddAsync(organization.Value);
                    var owner = organization.Value.Members.First(x => x.Role == "owner");
                    var user = await _userRepository.GetByIdAsync(owner.UserId);
                    if (user.Value.Organizations == null)
                    {
                        user.Value.Organizations = new HashSet<UserOrganization>();
                    }
                    user.Value.Organizations.Add(new UserOrganization
                    {
                        Id = organization.Value.Id,
                        Name = organization.Value.Name,
                        Role = owner.Role,
                        IsActive = owner.IsActive
                    });
                    await _userRepository.EditAsync(user.Value);
                    await _userCache.AddAsync(user.Value);
                })
                .OnError((ex, logger) =>
                {
                    logger.Error(ex, $"Error occured while handling {@event.GetType().Name} event");
                })
                .ExecuteAsync();
        }
    }
}