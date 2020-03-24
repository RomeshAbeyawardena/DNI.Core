namespace DNI.Core.Domains
{
    using System;

    public class ExceptionHandler
    {
        private ExceptionHandler(Action<Exception> exceptionAction, bool continueOnExceptionThrow = false)
        {
            ExceptionAction = exceptionAction;
            ContinueOnExceptionThrow = continueOnExceptionThrow;
        }

        public Action<Exception> ExceptionAction { get; }

        public bool ContinueOnExceptionThrow { get; }

        public static ExceptionHandler Create(Action<Exception> exceptionAction, bool continueOnExceptionThrow = false)
        {
            return new ExceptionHandler(exceptionAction, continueOnExceptionThrow);
        }
    }
}
