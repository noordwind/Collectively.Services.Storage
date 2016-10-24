using System.Collections.Generic;
using System.Linq;
using Coolector.Dto.Common;
using Coolector.Dto.Remarks;

namespace Coolector.Services.Storage.Mappers
{
    public class RemarkMapper : IMapper<RemarkDto>
    {
        public RemarkDto Map(dynamic source)
        {
            return new RemarkDto
            {
                Id = source.id,
                Author = new RemarkAuthorDto
                {
                    UserId = source.author.userId,
                    Name = source.author.name
                },
                Category = new RemarkCategoryDto
                {
                    Id = source.category.id,
                    Name = source.category.name
                },
                Location = new LocationDto
                {
                    Address = source.location.address,
                    Coordinates = new[]
                    {
                        (double) source.location.coordinates[0],
                        (double) source.location.coordinates[1]
                    },
                    Type = source.location.type
                },
                Photos = ((IEnumerable<dynamic>)source.photos).Select(x => new FileDto
                {
                    Name = x.name,
                    Metadata = x.metadata,
                    Size = x.size,
                    Url = x.url
                }).ToList(),
                Description = source.description,
                Resolved = source.resolved,
                ResolvedAt = source.resolvedAt,
                CreatedAt = source.createdAt
            };
        }
    }
}