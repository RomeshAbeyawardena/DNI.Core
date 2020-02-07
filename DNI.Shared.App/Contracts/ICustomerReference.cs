using DNI.Shared.Shared.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Shared.App.Contracts
{
    public interface ICustomerReference
    {
        [Claim("Reference")]
        string CustomerReference { get; set; }
    }
}
