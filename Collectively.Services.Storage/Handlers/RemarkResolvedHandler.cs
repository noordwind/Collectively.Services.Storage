using System.Threading.Tasks;
using Collectively.Messages.Events;
using Collectively.Services.Storage.Repositories;
using System.Linq;
using Collectively.Common.Services;

using Collectively.Messages.Events.Remarks;
using System.Collections.Generic;

namespace Collectively.Services.Storage.Handlers
{
    public class RemarkResolvedHandler : IEventHandler<RemarkResolved>
    {
        private readonly IHandler _handler;
        private readonly IRemarkRepository _remarkRepository;
        private readonly IUserRepository _userRepository;

        public RemarkResolvedHandler(IHandler handler, 
            IRemarkRepository remarkRepository,
            IUserRepository userRepository)
        {
            _handler = handler;
            _remarkRepository = remarkRepository;
            _userRepository = userRepository;
        }

        public async Task HandleAsync(RemarkResolved @event)
        {
            await _handler
                .Run(async () =>
                {
                    var remark = await _remarkRepository.GetByIdAsync(@event.RemarkId);
                    if (remark.HasNoValue)
                        return;

                    var user = await _userRepository.GetByIdAsync(@event.UserId);
                    if (user.HasNoValue)
                        return;
                    
                    remark.Value.Photos = @event.Photos.Select(x => new FileDto
                    {
                        GroupId = x.GroupId,
                        Name = x.Name,
                        Size = x.Size,
                        Url = x.Url,
                        Metadata = x.Metadata
                    }).ToList();
                    
                    var state = new RemarkStateDto
                    {
                        State = @event.State.State,
                        User = new RemarkUserDto
                        {
                            UserId = @event.State.UserId,
                            Name = @event.State.Username
                        },
                        Description = @event.State.Description,
                        CreatedAt = @event.State.CreatedAt
                    };
                    if (@event.State.Location != null)
                    {
                        state.Location = new LocationDto
                        {
                            Address = @event.State.Location.Address,
                            Coordinates = new[] {@event.State.Location.Longitude, @event.State.Location.Latitude},
                            Type = "Point"
                        };
                    }

                    remark.Value.State = state;
                    if(remark.Value.States == null)
                    {
                        remark.Value.States = new List<RemarkStateDto>();
                    }
                    remark.Value.States.Add(state);
                    await _remarkRepository.UpdateAsync(remark.Value);
                })
                .OnError((ex, logger) =>
                {
                    logger.Error(ex, $"Error occured while handling {@event.GetType().Name} event");
                })
                .ExecuteAsync();
        }
    }
}