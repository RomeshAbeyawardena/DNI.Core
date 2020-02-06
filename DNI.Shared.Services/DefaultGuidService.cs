using DNI.Shared.Contracts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Shared.Services
{
    internal sealed class DefaultGuidService : IGuidService
    {
        public Guid Generate()
        {
            return Guid.NewGuid();
        }

        public Guid Parse(string guid)
        {
            if(Guid.TryParse(guid, out var parsedGuid))
                return parsedGuid;

            return default;
        }
    }
}
