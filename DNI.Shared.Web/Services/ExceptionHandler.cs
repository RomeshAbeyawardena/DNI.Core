using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Shared.Web.Services
{
    public class ExceptionHandler
    {
        public void HandleException(ExceptionContext ex)
        {
            Console.WriteLine(ex.Exception.Message);
            ex.Result = new UnauthorizedResult();
        }
    }
}
