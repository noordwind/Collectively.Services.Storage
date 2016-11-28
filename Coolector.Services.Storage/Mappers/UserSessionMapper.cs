using Coolector.Common.Dto.Users;

namespace Coolector.Services.Storage.Mappers
{
    public class UserSessionMapper : IMapper<UserSessionDto>
    {
        public UserSessionDto Map(dynamic source)
        {
            return new UserSessionDto
            {
                Id = source.id,
                UserId = source.userId,
                Key = source.key,
                ParentId = source.parentId,
                Refreshed = source.refreshed,
                Destroyed = source.destroyed,
                UserAgent = source.userAgent,
                IpAddress = source.ipAddress,
                UpdatedAt = source.updatedAt,
                CreatedAt = source.createdAt
            };
        }
    }
}