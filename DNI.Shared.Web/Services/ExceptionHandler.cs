using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System;

namespace DNI.Shared.Web.Services
{
    public class ExceptionHandler
    {
        public void HandleException2(ExceptionContext ex)
        {
            Console.WriteLine(ex.Exception.Message);
            ex.Result = new UnauthorizedResult();
        }
    }
}
