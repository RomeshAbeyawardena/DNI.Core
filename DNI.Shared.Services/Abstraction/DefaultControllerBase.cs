using DNI.Core.Contracts;
using DNI.Core.Domains;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;


namespace DNI.Core.Services.Abstraction
{
    [Route("{controller}/{action}")]
    public abstract class DefaultControllerBase : Controller
    {
        public virtual TDestination Map<TSource, TDestination>(TSource source)
        {
            return MapperProvider.Map<TSource, TDestination>(source);
        }

        public virtual IEnumerable<TDestination> Map<TSource, TDestination>(IEnumerable<TSource> source)
        {
            return MapperProvider.Map<TSource, TDestination>(source);
        }

        protected void AddErrorsToModelState(ResponseBase response)
        {
            foreach(var error in response.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
        }

        protected IMediatorService MediatorService => GetService<IMediatorService>();

        protected IMapperProvider MapperProvider => GetService<IMapperProvider>();

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
