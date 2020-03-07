using DNI.Shared.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace DNI.Shared.Web.ExceptionHandlers
{
    public class UnauthorisedException : IExceptionHandler<UnauthorizedAccessException>
    {
        public bool HandleException(ExceptionContext exception)
        {
            exception.Result = new UnauthorizedResult();
            return true;
        }
    }
}
