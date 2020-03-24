namespace DNI.Core.Domains
{
    using System.Collections.Generic;
    using DNI.Core.Contracts;

    public struct AppHostEventArgs : IAppHostEventArgs
    {
        public AppHostEventArgs(IEnumerable<object> arguments)
        {
            Arguments = arguments;
        }

        public IEnumerable<object> Arguments { get; set; }
    }
}
