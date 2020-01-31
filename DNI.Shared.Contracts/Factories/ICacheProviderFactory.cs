using DNI.Shared.Contracts.Enumerations;
using DNI.Shared.Contracts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Shared.Contracts.Factories
{
    public interface ICacheProviderFactory
    {
        ICacheService GetCache(CacheType cacheType);
    }
}
