using DNI.Core.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace DNI.Core.Services.Abstraction
{
    public abstract class DefaultViewComponentBase : ViewComponent
    {
        public virtual TDestination Map<TSource, TDestination>(TSource source)
        {
            return MapperProvider.Map<TSource, TDestination>(source);
        }

        public virtual IEnumerable<TDestination> Map<TSource, TDestination>(IEnumerable<TSource> source)
        {
            return MapperProvider.Map<TSource, TDestination>(source);
        }

        protected IMapperProvider MapperProvider => GetService<IMapperProvider>();
        protected IMediatorService MediatorService => GetService<IMediatorService>();

        protected TService GetService<TService>()
        {
            if(HttpContext == null)
                throw new NullReferenceException("HttpContext unavailable");

            return HttpContext
                .RequestServices
                .GetRequiredService<TService>();
        }
    }
}
