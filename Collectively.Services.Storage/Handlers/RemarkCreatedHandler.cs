using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Collectively.Messages.Events;
using Collectively.Common.Services;

using Collectively.Messages.Events.Remarks;
using Collectively.Services.Storage.Models.Remarks;
using Collectively.Services.Storage.Models.Users;
using Collectively.Services.Storage.Repositories;


namespace Collectively.Services.Storage.Handlers
{
    public class RemarkCreatedHandler : IEventHandler<RemarkCreated>
    {
        private readonly IHandler _handler;
        private readonly IUserRepository _userRepository;
        private readonly IRemarkRepository _remarkRepository;

        public RemarkCreatedHandler(IHandler handler, 
            IUserRepository userRepository,
            IRemarkRepository remarkRepository)
        {
            _handler = handler;
            _userRepository = userRepository;
            _remarkRepository = remarkRepository;
        }

        public async Task HandleAsync(RemarkCreated @event)
        {
            await _handler
                .Run(async () =>
                {
                    var user = await _userRepository.GetByIdAsync(@event.UserId);
                    var remark = MapToDto(@event, user.Value);
                    await _remarkRepository.AddAsync(remark);
                })
                .OnError((ex, logger) =>
                {
                    logger.Error(ex, $"Error occured while handling {@event.GetType().Name} event");
                })
                .ExecuteAsync();
        }

        private static Remark MapToDto(RemarkCreated @event, User user)
        {   
            var remarkDto  = new Remark
            {
                Id = @event.RemarkId,
                Description = @event.Description,
                Category = new RemarkCategory
                {
                    Id = @event.Category.CategoryId,
                    Name = @event.Category.Name
                },
                Location = new Location
                {
                    Address = @event.Location.Address,
                    Coordinates = new[] {@event.Location.Longitude, @event.Location.Latitude},
                    Type = "Point"
                },
                CreatedAt = DateTime.UtcNow,
                Author = new RemarkUser
                {
                    UserId = @event.UserId,
                    Name = user.Name
                },
                States = new List<RemarkState>()
                {
                    new RemarkState
                    {
                        State = @event.State.State,
                        User = new RemarkUser
                        {
                            UserId = @event.State.UserId,
                            Name = @event.State.Username
                        },
                        Description = @event.State.Description,
                        Location = new Location
                        {
                            Address = @event.State.Location.Address,
                            Coordinates = new[] {@event.State.Location.Longitude, @event.State.Location.Latitude},
                            Type = "Point"
                        },
                        CreatedAt = @event.State.CreatedAt
                    }
                },
                Tags = @event.Tags.ToList(),
                Photos = new List<File>(),
                Votes = new List<Vote>()
            };
            remarkDto.State = remarkDto.States.First();

            return remarkDto;
        }
    }
}