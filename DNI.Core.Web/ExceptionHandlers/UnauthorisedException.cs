using DNI.Core.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace DNI.Core.Web.ExceptionHandlers
{
    public class UnauthorisedException : IExceptionHandler<UnauthorizedAccessException>
    {
        public UnauthorizedAccessException Exception => throw new NotImplementedException();

        public bool HandleException(ExceptionContext exception)
        {
            exception.Result = new UnauthorizedResult();
            return true;
        }
    }
}
