namespace DNI.Core.Services
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using DNI.Core.Contracts;

    public static class ReflectionDescriber
    {
        public static IDescribe Describe()
        {
            return new DefaultDescribe();
        }
    }

    internal class DefaultDescribe : IDescribe
    {
        private readonly ITypesDescriptor typesDescriptor;
        private readonly IAssembliesDescriptor assembliesDescriptor;

        public Action<ITypesDescriptor> Types { get; set; }

        public Action<IAssembliesDescriptor> Assemblies { get; set; }

        public IEnumerable<Type> DescribedTypes
        {
            get
            {
                Types?.Invoke(typesDescriptor);
                return typesDescriptor.ToTypeArray();
            }
        }

        public IEnumerable<Assembly> DescribedAssemblies
        {
            get
            {
                Assemblies?.Invoke(assembliesDescriptor);
                return assembliesDescriptor.Assemblies;
            }
        }

        public DefaultDescribe()
        {
            typesDescriptor = new DefaultTypesDescriptor();
            assembliesDescriptor = new DefaultAssembliesDescriptor();
        }
    }
}
