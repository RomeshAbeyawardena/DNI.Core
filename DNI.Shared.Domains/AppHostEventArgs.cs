using DNI.Shared.Contracts;
using System.Collections.Generic;

namespace DNI.Shared.Domains
{
    public struct AppHostEventArgs : IAppHostEventArgs
    {
        public AppHostEventArgs(IEnumerable<object> arguments)
        {
            Arguments = arguments;
        }

        public IEnumerable<object> Arguments { get; set; }
    }
}
