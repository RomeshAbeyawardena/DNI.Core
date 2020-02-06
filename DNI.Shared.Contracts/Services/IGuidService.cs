using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Shared.Contracts.Services
{
    public interface IGuidService
    {
        Guid Generate();
        Guid Parse(string guid);
    }
}
