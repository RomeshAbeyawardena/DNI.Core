using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace DNI.Shared.Services.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public sealed class HeaderValueAttribute  : Attribute, IActionFilter
    {
        public HeaderValueAttribute(string name, string value = default)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; }
        public string Value { get; }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            context.HttpContext
                .Response.Headers
                .Add(Name, Value);
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            
        }
    }
}
