namespace DNI.Core.Contracts
{
    using System.Collections.Generic;

    public interface IAppHostEventArgs
    {
        IEnumerable<object> Arguments { get; set; }
    }
}
