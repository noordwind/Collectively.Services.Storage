
using Coolector.Common.Dto.General;

namespace Coolector.Services.Storage.Mappers
{
    public class AvailabilityMapper : IMapper<AvailableResourceDto>
    {
        public AvailableResourceDto Map(dynamic source)
        {
            var available = source as bool? ?? default(bool);

            return new AvailableResourceDto {IsAvailable = available};
        }
    }
}