using Coolector.Services.Storage.Mappers;
using System.Dynamic;

namespace Coolector.Services.Storage.Tests.Specs.Mappers
{
    public abstract class Mapper_specs<T>
    {
        protected static IMapper<T> Mapper;
        protected static T Result;
        protected static dynamic Source;

        protected static void Initialize(IMapper<T> mapper)
        {
            Mapper = mapper;
            Source = new ExpandoObject();
        }

        protected static void Map()
        {
            Result = Mapper.Map(Source);
        }
    }
}