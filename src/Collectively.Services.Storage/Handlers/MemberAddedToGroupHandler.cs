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
    public class MemberAddedToGroupHandler : IEventHandler<MemberAddedToGroup>
    {
        private readonly IHandler _handler;
        private readonly IGroupRepository _groupRepository;
        private readonly IUserRepository _userRepository;

        public MemberAddedToGroupHandler(IHandler handler, 
            IGroupRepository groupRepository,
            IUserRepository userRepository)
        {
            _handler = handler;
            _groupRepository = groupRepository;
            _userRepository = userRepository;
        }

        public async Task HandleAsync(MemberAddedToGroup @event)
        {
            await _handler
                .Run(async () =>
                {
                    var group = await _groupRepository.GetAsync(@event.GroupId);
                    var user = await _userRepository.GetByIdAsync(@event.MemberId);
                    group.Value.Members.Add(new Member
                    {
                        UserId = user.Value.UserId,
                        Name = user.Value.Name,
                        Role = @event.Role,
                        IsActive = true
                    });
                    group.Value.MembersCount++;
                    await _groupRepository.UpdateAsync(group.Value);
                })
                .OnError((ex, logger) =>
                {
                    logger.Error(ex, $"Error occured while handling {@event.GetType().Name} event");
                })
                .ExecuteAsync();
        }
    }
}