namespace DNI.Core.Services.Attributes
{
    using System;
    using Microsoft.AspNetCore.Mvc.Filters;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public sealed class HeaderValueAttribute : Attribute, IActionFilter
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
