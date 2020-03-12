using System;
using System.Collections.Generic;

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
