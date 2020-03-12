using System.Collections.Generic;
using System.Reflection;

namespace DNI.Core.Contracts
{
    public interface IAssembliesDescriptor
    {
        IAssembliesDescriptor GetAssembly<T>();
        IEnumerable<Assembly> Assemblies { get; }
    }
}
