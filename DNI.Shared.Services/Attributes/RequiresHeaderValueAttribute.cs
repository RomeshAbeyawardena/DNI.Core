using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;

namespace DNI.Shared.Services.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public sealed class RequiresHeaderValueAttribute : Attribute, IActionFilter
    {
        public string HeaderKey { get; }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if(!context.HttpContext.Request.Headers.TryGetValue(HeaderKey, out var headerValue))
                context.Result = new UnauthorizedResult();

            context.HttpContext.Items.TryAdd(HeaderKey, headerValue);
        }

        public RequiresHeaderValueAttribute(string headerKey)
        {
            HeaderKey = headerKey;
        }
    }
}
