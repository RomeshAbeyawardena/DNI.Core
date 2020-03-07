using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Linq;
using System.Text;

namespace DNI.Core.Services.Exceptions
{
    public class ModelStateException : Exception
    {
        private readonly string NewLine = Environment.NewLine;

        private string BuildErrorMessage()
        {
            var errorStringBuilder = new StringBuilder();

            errorStringBuilder.AppendLine("Validation Errors:");
            foreach (var (key, value) in ModelState.Where(model => model.Value.Errors.Any()))
            {
                errorStringBuilder.AppendLine(BuildKeyErrors(key, value.Errors));
            }

            return errorStringBuilder.ToString();
        }

        private string BuildKeyErrors(string key, ModelErrorCollection errors)
        {
            var errorStringBuilder = new StringBuilder();
            errorStringBuilder.AppendFormat("{0}:{1}", key, NewLine);
            foreach (var error in errors)
            {
                errorStringBuilder.AppendFormat("\t{0}{1}", error.ErrorMessage, NewLine);
            }

            return errorStringBuilder.ToString();
        }

        public ModelStateException(ModelStateDictionary modelState)
        {
            ModelState = modelState;
        }

        public override string Message => BuildErrorMessage();

        public ModelStateDictionary ModelState { get; }
    }
}
