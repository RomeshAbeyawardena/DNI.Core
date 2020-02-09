using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Shared.Services.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public sealed class HandleErrorAttribute : Attribute, IExceptionFilter
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
            if (!ExceptionTypes.Any(exception => context.Exception.GetType() == exception))
                return;
            
            var service = context.HttpContext.RequestServices.GetService(ServiceType);

            if(service == null)
                return;

            var exceptionHandlingMethod = ServiceType.GetMethod(HandleMethod);

            if(exceptionHandlingMethod == null)
                return;

            var methodParameters = exceptionHandlingMethod.GetParameters();
            if (methodParameters.Length != 1 
                || methodParameters.All(p => p.ParameterType != typeof(ExceptionContext)))
                return;

            exceptionHandlingMethod.Invoke(service, new [] { context });
            context.ExceptionHandled = true;
        }
    }
}
