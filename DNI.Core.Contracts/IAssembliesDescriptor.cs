using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Contracts
{
    public interface IAssembliesDescriptor
    {
        IAssembliesDescriptor GetAssembly<T>();
        IEnumerable<Assembly> Assemblies { get; }
    }
}
