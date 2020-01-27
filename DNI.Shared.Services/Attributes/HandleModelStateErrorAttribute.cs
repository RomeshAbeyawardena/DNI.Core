using DNI.Shared.Services.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Shared.Services.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class HandleModelStateErrorAttribute : Attribute, IActionFilter
    {
        private readonly bool _throwModelStateException;

        public void OnActionExecuted(ActionExecutedContext context)
        {
            
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if(context.ModelState.IsValid)
                return;

            if(_throwModelStateException)
                throw new ModelStateException(context.ModelState);

            context.Result = new BadRequestObjectResult(context.ModelState);
        }

        public HandleModelStateErrorAttribute(bool throwModelStateException = true)
        {
            _throwModelStateException = throwModelStateException;
        }
    }
}
