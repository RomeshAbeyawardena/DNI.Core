using DNI.Shared.Contracts.Factories;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Shared.Services.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public sealed class HandleExceptionAttribute : Attribute, IExceptionFilter
    {
        public HandleExceptionAttribute()
        {
            
        }

        public void OnException(ExceptionContext context)
        {
            
            var exceptionHandlerFactory = context.HttpContext.RequestServices.GetRequiredService<IExceptionHandlerFactory>();

            if(exceptionHandlerFactory == null || !exceptionHandlerFactory
                .TryGetExceptionHandler(context.Exception.GetType(), out var exceptionHandler))
                return;

            context.ExceptionHandled = exceptionHandler.HandleException(context);
        }
    }
}
