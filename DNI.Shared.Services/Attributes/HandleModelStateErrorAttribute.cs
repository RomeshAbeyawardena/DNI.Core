using DNI.Shared.Services.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace DNI.Shared.Services.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class HandleModelStateErrorAttribute : Attribute, IActionFilter
    {
        private readonly bool _throwModelStateException;
        private ILogger<HandleModelStateErrorAttribute> GetLogger(IServiceProvider serviceProvider) => serviceProvider
            .GetRequiredService<ILogger<HandleModelStateErrorAttribute>>();

        public void OnActionExecuted(ActionExecutedContext context)
        {
            var logger = GetLogger(context.HttpContext.RequestServices);

            logger.LogInformation("{0} executed.", nameof(HandleModelStateErrorAttribute));

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var logger = GetLogger(context.HttpContext.RequestServices);

            logger.LogInformation("{0} executing.", nameof(HandleModelStateErrorAttribute));

            if(context.ModelState.IsValid)
                return;

            var modelStateError = new ModelStateException(context.ModelState);
            
            logger.LogError(modelStateError, "Validation errors occurred.");

            if(_throwModelStateException)
                throw modelStateError;

            context.Result = new BadRequestObjectResult(context.ModelState);
        }

        public HandleModelStateErrorAttribute(bool throwModelStateException = false)
        {
            _throwModelStateException = throwModelStateException;
        }

    }
}
