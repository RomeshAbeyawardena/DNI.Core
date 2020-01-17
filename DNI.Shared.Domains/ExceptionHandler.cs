using System;

namespace DNI.Shared.Domains
{
    public class ExceptionHandler
    {
        public static ExceptionHandler Create(Action<Exception> exceptionAction, bool continueOnExceptionThrow = false)
        {
            return new ExceptionHandler(exceptionAction, continueOnExceptionThrow);
        }

        private ExceptionHandler(Action<Exception> exceptionAction, bool continueOnExceptionThrow = false)
        {
            ExceptionAction = exceptionAction;
            ContinueOnExceptionThrow = continueOnExceptionThrow;
        }

        public Action<Exception> ExceptionAction { get; }
        public bool ContinueOnExceptionThrow { get; }
    }
}
