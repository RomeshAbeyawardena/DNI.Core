﻿using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Shared.Services.Attributes
{
    public class HandleErrorAttribute : Attribute, IExceptionFilter
    {
        public HandleErrorAttribute(Type serviceType, string handleMethod, params Type[] exceptionTypes)
        {
            ServiceType = serviceType;
            HandleMethod = handleMethod;
            ExceptionTypes = exceptionTypes;
        }

        public Type ServiceType { get; }
        public string HandleMethod { get; }
        public Type[] ExceptionTypes { get; }

        public void OnException(ExceptionContext context)
        {
            if (!ExceptionTypes.Any(exception => context.Exception.GetType() == exception)) { 
                context.ExceptionHandled = false;
                return;
            }
            
            var service = context.HttpContext.RequestServices.GetService(ServiceType);

            var exceptionHandlingMethod = ServiceType.GetMethod(HandleMethod);

            exceptionHandlingMethod.Invoke(service, new [] { context });
            context.ExceptionHandled = true;
        }
    }
}
