using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Coolector.Common.Events;
using Coolector.Common.Events.Remarks;
using Coolector.Dto.Common;
using Coolector.Dto.Remarks;
using Coolector.Dto.Users;
using Coolector.Services.Storage.Repositories;
using Coolector.Services.Storage.Settings;

namespace Coolector.Services.Storage.Handlers
{
    public class RemarkCreatedHandler : IEventHandler<RemarkCreated>
    {
        private readonly IUserRepository _userRepository;
        private readonly IRemarkRepository _remarkRepository;
        private readonly GeneralSettings _generalSettings;

        public RemarkCreatedHandler(IUserRepository userRepository,
            IRemarkRepository remarkRepository,
            GeneralSettings generalSettings)
        {
            _userRepository = userRepository;
            _remarkRepository = remarkRepository;
            _generalSettings = generalSettings;
        }

        public async Task HandleAsync(RemarkCreated @event)
        {
            var user = await _userRepository.GetByIdAsync(@event.UserId);
            var photos = @event.Photos.Select(x => new FileDto
            {
                Name = x.Name,
                Size = x.Size,
                Url = x.Url,
                Metadata = x.Metadata
            });

            var remark = MapToDto(@event, photos, user.Value);
            await _remarkRepository.AddAsync(remark);
        }

        private static RemarkDto MapToDto(RemarkCreated @event, IEnumerable<FileDto> photos,
                UserDto user)
            => new RemarkDto
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
                Author = new RemarkAuthorDto
                {
                    UserId = @event.UserId,
                    Name = user.Name
                },
                Photos = photos.ToList()
            };
    }
}