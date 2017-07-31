using System.Threading.Tasks;
using Collectively.Messages.Events;
using Collectively.Common.Services;
using Collectively.Messages.Events.Groups;
using Collectively.Services.Storage.Models.Groups;
using Collectively.Services.Storage.Repositories;
using Collectively.Services.Storage.ServiceClients;

namespace Collectively.Services.Storage.Handlers
{
    public class GroupCreatedHandler : IEventHandler<GroupCreated>
    {
        private readonly IHandler _handler;
        private readonly IGroupRepository _groupRepository;
        private readonly IGroupServiceClient _groupServiceClient;

        public GroupCreatedHandler(IHandler handler, 
            IGroupRepository groupRepository,
            IGroupServiceClient groupServiceClient)
        {
            _handler = handler;
            _groupRepository = groupRepository;
            _groupServiceClient = groupServiceClient;
        }

        public async Task HandleAsync(GroupCreated @event)
        {
            await _handler
                .Run(async () =>
                {
                    var group = await _groupServiceClient.GetAsync<Group>(@event.GroupId);
                    await _groupRepository.AddAsync(group.Value);
                })
                .OnError((ex, logger) =>
                {
                    logger.Error(ex, $"Error occured while handling {@event.GetType().Name} event");
                })
                .ExecuteAsync();
        }
    }
}