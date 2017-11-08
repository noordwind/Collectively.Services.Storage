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
    public class MemberAddedToGroupHandler : IEventHandler<MemberAddedToGroup>
    {
        private readonly IHandler _handler;
        private readonly IGroupRepository _groupRepository;
        private readonly IUserRepository _userRepository;
        private readonly IGroupCache _groupCache;
        private readonly IUserCache _userCache;

        public MemberAddedToGroupHandler(IHandler handler, 
            IGroupRepository groupRepository,
            IUserRepository userRepository,
            IGroupCache groupCache,
            IUserCache userCache)
        {
            _handler = handler;
            _groupRepository = groupRepository;
            _userRepository = userRepository;
            _groupCache = groupCache;
            _userCache = userCache;
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
                    await _groupCache.AddAsync(group.Value);
                    var member = group.Value.Members.First(x => x.UserId == @event.MemberId);
                    if (user.Value.Groups == null)
                    {
                        user.Value.Groups = new HashSet<UserGroup>();
                    }
                    user.Value.Groups.Add(new UserGroup
                    {
                        Id = group.Value.Id,
                        Name = group.Value.Name,
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