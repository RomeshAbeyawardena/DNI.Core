using DNI.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace DNI.Core.Services
{
    public static class ReflectionDescriber
    {
        public static IDescribe Describe()
        {
            return new DefaultDescribe();
        }
    }

    internal class DefaultDescribe : IDescribe
    {
        private readonly ITypesDescriptor _typesDescriptor;
        private readonly IAssembliesDescriptor _assembliesDescriptor;
        public Action<ITypesDescriptor> Types { get; set; }
        public Action<IAssembliesDescriptor> Assemblies { get; set; }

        public IEnumerable<Type> DescribedTypes
        {
            get
            {
                Types?.Invoke(_typesDescriptor);
                return _typesDescriptor.ToTypeArray();
            }
        }

        public IEnumerable<Assembly> DescribedAssemblies
        {
            get
            {
                Assemblies?.Invoke(_assembliesDescriptor);
                return _assembliesDescriptor.Assemblies;
            }
        }

        public DefaultDescribe()
        {
            _typesDescriptor = new DefaultTypesDescriptor();
            _assembliesDescriptor = new DefaultAssembliesDescriptor();
        }
    }
}
