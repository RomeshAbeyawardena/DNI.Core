namespace DNI.Core.Contracts
{
    using System.Collections.Generic;
    using System.Reflection;

    public interface IAssembliesDescriptor
    {
        IEnumerable<Assembly> Assemblies { get; }

        IAssembliesDescriptor GetAssembly<T>();
    }
}
