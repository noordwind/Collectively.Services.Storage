using Coolector.Dto.Operations;

namespace Coolector.Services.Storage.Mappers
{
    public class OperationMapper : IMapper<OperationDto>
    {
        public OperationDto Map(dynamic source)
            => new OperationDto
            {
                RequestId = source.requestId,
                UserId = source.userId,
                Origin = source.origin,
                Resource = source.resource,
                State = source.state,
                Message = source.message,
                CreatedAt = source.createdAt,
                UpdatedAt = source.updatedAt
            };
    }
}