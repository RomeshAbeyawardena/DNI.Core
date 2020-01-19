using System.Collections.Generic;

namespace DNI.Shared.Contracts
{
    public interface IAppHostEventArgs
    {
        IEnumerable<object> Arguments { get; set; }
    }
}
