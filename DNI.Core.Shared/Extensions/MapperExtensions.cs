using AutoMapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Shared.Extensions
{
    public static class MapperExtensions
    {
        public static IEnumerable<TDestination> Map<TDestination>(this IMapper mapper, object source)
        {
            return mapper.Map<IEnumerable<TDestination>>(source);
        }

        public static IEnumerable<TDestination> Map<TSource, TDestination>(this IMapper mapper, IEnumerable<TSource> source)
        {
            return mapper.Map<IEnumerable<TDestination>>(source);
        }
    }
}
