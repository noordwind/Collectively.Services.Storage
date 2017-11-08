using System;
using System.Threading.Tasks;
using Collectively.Messages.Events;
using Collectively.Common.Services;
using Collectively.Messages.Events.Groups;
using Collectively.Services.Storage.Models.Groups;
using Collectively.Services.Storage.Repositories;
using System.Collections.Generic;
using Collectively.Services.Storage.Services;
using Collectively.Services.Storage.Models.Users;
using System.Linq;

namespace Collectively.Services.Storage.Handlers
{
    public class MemberAddedToOrganizationHandler : IEventHandler<MemberAddedToOrganization>
    {
        private readonly IHandler _handler;
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IUserRepository _userRepository;
        private readonly IOrganizationCache _organizationCache;
        private readonly IUserCache _userCache;

        public MemberAddedToOrganizationHandler(IHandler handler, 
            IOrganizationRepository organizationRepository,
            IUserRepository userRepository,
            IOrganizationCache organizationCache,
            IUserCache userCache)
        {
            _handler = handler;
            _organizationRepository = organizationRepository;
            _userRepository = userRepository;
            _organizationCache = organizationCache;
            _userCache = userCache;
        }

        public async Task HandleAsync(MemberAddedToOrganization @event)
        {
            await _handler
                .Run(async () =>
                {
                    var organization = await _organizationRepository.GetAsync(@event.OrganizationId);
                    var user = await _userRepository.GetByIdAsync(@event.MemberId);
                    organization.Value.Members.Add(new Member
                    {
                        UserId = user.Value.UserId,
                        Name = user.Value.Name,
                        Role = @event.Role,
                        IsActive = true
                    });
                    organization.Value.MembersCount++;
                    await _organizationRepository.UpdateAsync(organization.Value);
                    await _organizationCache.AddAsync(organization.Value);
                    var member = organization.Value.Members.First(x => x.UserId == @event.MemberId);
                    if (user.Value.Organizations == null)
                    {
                        user.Value.Organizations = new HashSet<UserOrganization>();
                    }
                    user.Value.Organizations.Add(new UserOrganization
                    {
                        Id = organization.Value.Id,
                        Name = organization.Value.Name,
                        Role = member.Role,
                        IsActive = member.IsActive
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