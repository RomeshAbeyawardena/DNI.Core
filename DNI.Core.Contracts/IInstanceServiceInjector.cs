using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Contracts
{
    public interface IInstanceServiceInjector
    {
        object CreateInstance(Type serviceType);
        TService CreateInstance<TService>();
    }
}
