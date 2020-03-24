namespace DNI.Core.Services.ExceptionHandlers.BuiltIn
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using DNI.Core.Contracts;
    using DNI.Core.Services.Exceptions;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;

    public class ModelStateExceptionHandler : IExceptionHandler<ModelStateException>
    {
        public ModelStateException Exception { get; private set; }

        public bool HandleException(ExceptionContext exception)
        {
            Exception = exception.Exception as ModelStateException;
            exception.Result = new BadRequestObjectResult(exception.ModelState);
            return true;
        }
    }
}
