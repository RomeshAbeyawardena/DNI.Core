using DNI.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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
                Types(_typesDescriptor);
                return _typesDescriptor.ToTypeArray();
            }
        }

        public IEnumerable<Assembly> DescribedAssemblies
        {
            get
            {
                Assemblies(_assembliesDescriptor);
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
