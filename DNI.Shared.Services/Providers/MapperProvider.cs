using AutoMapper;
using DNI.Shared.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Shared.Services.Providers
{
    public class MapperProvider : IMapperProvider
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
