using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Collectively.Common.Events;
using Collectively.Common.Services;

using Collectively.Messages.Events.Remarks;
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

        private static RemarkDto MapToDto(RemarkCreated @event, UserDto user)
        {   
            var remarkDto  = new RemarkDto
            {
                Id = @event.RemarkId,
                Description = @event.Description,
                Category = new RemarkCategoryDto
                {
                    Id = @event.Category.CategoryId,
                    Name = @event.Category.Name
                },
                Location = new LocationDto
                {
                    Address = @event.Location.Address,
                    Coordinates = new[] {@event.Location.Longitude, @event.Location.Latitude},
                    Type = "Point"
                },
                CreatedAt = DateTime.UtcNow,
                Author = new RemarkUserDto
                {
                    UserId = @event.UserId,
                    Name = user.Name
                },
                States = new List<RemarkStateDto>()
                {
                    new RemarkStateDto
                    {
                        State = @event.State.State,
                        User = new RemarkUserDto
                        {
                            UserId = @event.State.UserId,
                            Name = @event.State.Username
                        },
                        Description = @event.State.Description,
                        Location = new LocationDto
                        {
                            Address = @event.State.Location.Address,
                            Coordinates = new[] {@event.State.Location.Longitude, @event.State.Location.Latitude},
                            Type = "Point"
                        },
                        CreatedAt = @event.State.CreatedAt
                    }
                },
                Tags = @event.Tags.ToList(),
                Photos = new List<FileDto>(),
                Votes = new List<VoteDto>()
            };
            remarkDto.State = remarkDto.States.First();

            return remarkDto;
        }
    }
}