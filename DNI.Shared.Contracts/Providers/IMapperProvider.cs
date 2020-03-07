using System.Collections.Generic;

namespace DNI.Core.Contracts
{
    public interface IMapperProvider
    {
        TDestination Map<TSource, TDestination>(TSource source);
        IEnumerable<TDestination> Map<TSource, TDestination>(IEnumerable<TSource> source);
    }
}
