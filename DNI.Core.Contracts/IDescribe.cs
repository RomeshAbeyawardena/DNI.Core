using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Contracts
{
    public interface IDescribe
    {
        Action<ITypesDescriptor> Types { get; set; }
        Action<IAssembliesDescriptor> Assemblies { get; set; }
        IEnumerable<Type> DescribedTypes { get; }
        IEnumerable<Assembly> DescribedAssemblies { get; }
    }
}
