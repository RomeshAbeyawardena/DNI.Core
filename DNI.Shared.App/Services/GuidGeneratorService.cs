using DNI.Shared.App.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Shared.App.Services
{
    public class GuidGeneratorService : IGuidGeneratorService
    {
        public Guid Generate()
        {
            return Guid.NewGuid();
        }
    }
}
