namespace DNI.Core.Services.Attributes
{
    using System;
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public sealed class RequiresHeaderValueAttribute : Attribute, IActionFilter
    {
        public string HeaderKey { get; }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue(HeaderKey, out var headerValue))
            {
                context.Result = new UnauthorizedResult();
            }

            context.HttpContext.Items.TryAdd(HeaderKey, headerValue);
        }

        public RequiresHeaderValueAttribute(string headerKey)
        {
            HeaderKey = headerKey;
        }
    }
}
