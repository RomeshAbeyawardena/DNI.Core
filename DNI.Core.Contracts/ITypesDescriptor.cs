using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Contracts
{
    public interface ITypesDescriptor
    {
        ITypesDescriptor Describe<T>();
        ITypesDescriptor Describe(Type type);
        IEnumerable<ITypeDescriptor> DescribedTypes { get; }
        IEnumerable<Type> ToTypeArray();
    }
    public interface ITypeDescriptor
    {
        Type Type { get; }
    }

    public interface ITypeDescriptor<T> : ITypeDescriptor
    {
        
    }
}
