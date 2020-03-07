using DNI.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Services
{
    public class AssembliesDescriptor
    {
        public static IAssembliesDescriptor GetAssembly<T>()
        {
            return new DefaultAssembliesDescriptor()
                .GetAssembly<T>();
        }
    }

    internal class DefaultAssembliesDescriptor : IAssembliesDescriptor
    {
        private readonly IList<Assembly> assemblies;

        public IAssembliesDescriptor GetAssembly<T>()
        {
            assemblies.Add(Assembly.GetAssembly(typeof(T)));
            return this;
        }

        public IEnumerable<Assembly> Assemblies => assemblies.ToArray();

        public DefaultAssembliesDescriptor()
        {
            assemblies = new List<Assembly>();
        }
    }
}
