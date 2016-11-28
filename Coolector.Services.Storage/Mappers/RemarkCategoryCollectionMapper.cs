using Coolector.Common.Dto.Remarks;

namespace Coolector.Services.Storage.Mappers
{
    public class RemarkCategoryCollectionMapper : CollectionMapper<RemarkCategoryDto>
    {
        public RemarkCategoryCollectionMapper(IMapper<RemarkCategoryDto> mapper) : base(mapper)
        {
        }
    }
}