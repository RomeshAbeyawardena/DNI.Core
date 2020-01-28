using DNI.Shared.Services.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Shared.Services.Abstraction
{
    [HandleModelStateError]
    public abstract class DefaultApiControllerBase : DefaultControllerBase
    {
        
    }
}
