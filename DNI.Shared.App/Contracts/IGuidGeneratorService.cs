using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Shared.App.Contracts
{
    public interface IGuidGeneratorService
    {
        Guid Generate();
    }
}
