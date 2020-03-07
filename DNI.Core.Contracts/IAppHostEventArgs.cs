using System.Collections.Generic;

namespace DNI.Core.Contracts
{
    public interface IAppHostEventArgs
    {
        IEnumerable<object> Arguments { get; set; }
    }
}
