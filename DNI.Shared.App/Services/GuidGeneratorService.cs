using DNI.Shared.App.Contracts;
using System;

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
