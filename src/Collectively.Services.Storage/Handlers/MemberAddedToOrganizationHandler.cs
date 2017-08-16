using System;
using System.Threading.Tasks;
using Collectively.Messages.Events;
using Collectively.Common.Services;
using Collectively.Messages.Events.Groups;
using Collectively.Services.Storage.Models.Groups;
using Collectively.Services.Storage.Repositories;
using System.Collections.Generic;

namespace Collectively.Services.Storage.Handlers
{
    public class MemberAddedToOrganizationHandler : IEventHandler<MemberAddedToOrganization>
    {
        private readonly IHandler _handler;
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IUserRepository _userRepository;

        public MemberAddedToOrganizationHandler(IHandler handler, 
            IOrganizationRepository organizationRepository,
            IUserRepository userRepository)
        {
            _handler = handler;
            _organizationRepository = organizationRepository;
            _userRepository = userRepository;
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
                })
                .OnError((ex, logger) =>
                {
                    logger.Error(ex, $"Error occured while handling {@event.GetType().Name} event");
                })
                .ExecuteAsync();
        }
    }
}