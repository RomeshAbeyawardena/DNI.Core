namespace DNI.Core.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IEndPointPath
    {
        string Method { get; set; }

        string Summary { get; set; }

        string OperationId { get; set; }

        IEnumerable<string> Tags { get; set; }

        IDictionary<string, IEndpointParameter> Parameters { get; set; }
    }
}
