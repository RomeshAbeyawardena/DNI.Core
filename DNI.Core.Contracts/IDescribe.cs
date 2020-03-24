namespace DNI.Core.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    public interface IDescribe
    {
        Action<ITypesDescriptor> Types { get; set; }

        Action<IAssembliesDescriptor> Assemblies { get; set; }

        IEnumerable<Type> DescribedTypes { get; }

        IEnumerable<Assembly> DescribedAssemblies { get; }
    }
}
