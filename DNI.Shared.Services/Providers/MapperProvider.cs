using AutoMapper;
using DNI.Core.Contracts;
using System.Collections.Generic;

namespace DNI.Core.Services.Providers
{
    internal sealed class MapperProvider : IMapperProvider
    {
        private readonly IMapper _mapper;

        public TDestination Map<TSource, TDestination>(TSource source)
        {
            return _mapper.Map<TSource, TDestination>(source);
        }

        public IEnumerable<TDestination> Map<TSource, TDestination>(IEnumerable<TSource> source)
        {
            return Map<IEnumerable<TSource>, IEnumerable<TDestination>>(source);
        }

        public MapperProvider(IMapper mapper)
        {
            _mapper = mapper;
        }
    }
}
