using System;
using System.Threading.Tasks;
using Collectively.Messages.Events;
using Collectively.Common.Services;
using Collectively.Messages.Events.Groups;
using Collectively.Services.Storage.Models.Groups;
using Collectively.Services.Storage.Repositories;
using System.Collections.Generic;
using Collectively.Services.Storage.Services;

namespace Collectively.Services.Storage.Handlers
{
    public class MemberAddedToOrganizationHandler : IEventHandler<MemberAddedToOrganization>
    {
        private readonly IHandler _handler;
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IUserRepository _userRepository;
        private readonly IOrganizationCache _cache;

        public MemberAddedToOrganizationHandler(IHandler handler, 
            IOrganizationRepository organizationRepository,
            IUserRepository userRepository,
            IOrganizationCache cache)
        {
            _handler = handler;
            _organizationRepository = organizationRepository;
            _userRepository = userRepository;
            _cache = cache;
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