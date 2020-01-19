using DNI.Shared.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
