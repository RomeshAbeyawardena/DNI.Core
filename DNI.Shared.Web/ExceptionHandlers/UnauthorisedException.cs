using DNI.Shared.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
