namespace DNI.Core.Services.Attributes
{
    using System;
    using DNI.Core.Contracts.Factories;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.Extensions.DependencyInjection;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public sealed class HandleExceptionAttribute : Attribute, IExceptionFilter
    {
        public HandleExceptionAttribute()
        {
        }

        public void OnException(ExceptionContext context)
        {
            var exceptionHandlerFactory = context.HttpContext.RequestServices.GetRequiredService<IExceptionHandlerFactory>();

            if (exceptionHandlerFactory == null || !exceptionHandlerFactory
                .TryGetExceptionHandler(context.Exception.GetType(), out var exceptionHandler))
            {
                return;
            }

            context.ExceptionHandled = exceptionHandler.HandleException(context);
        }
    }
}
