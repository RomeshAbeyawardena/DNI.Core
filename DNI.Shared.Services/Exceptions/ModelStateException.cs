using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Shared.Services.Exceptions
{
    public class ModelStateException : Exception
    {
        private string BuildErrorMessage()
        {
            var errorStringBuilder = new StringBuilder();
            return errorStringBuilder.ToString();
        }

        public ModelStateException(ModelStateDictionary modelState)
        {
            ModelState = modelState;
        }

        public override string Message => base.Message;

        public ModelStateDictionary ModelState { get; }
    }
}
