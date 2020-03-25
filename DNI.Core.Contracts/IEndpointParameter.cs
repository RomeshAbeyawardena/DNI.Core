namespace DNI.Core.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IEndpointParameter
    {
        string Description { get; set; }

        bool Required { get; set; }

        IParameterSchema Schema { get; set; }
    }
}
