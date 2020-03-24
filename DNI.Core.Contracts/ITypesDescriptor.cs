namespace DNI.Core.Contracts
{
    using System;
    using System.Collections.Generic;

    public interface ITypesDescriptor
    {
        IEnumerable<ITypeDescriptor> DescribedTypes { get; }

        ITypesDescriptor Describe<T>();

        IEnumerable<Type> ToTypeArray();

        ITypesDescriptor Describe(Type type);
    }

    public interface ITypeDescriptor
    {
        Type Type { get; }
    }

    public interface ITypeDescriptor<T> : ITypeDescriptor
    {
        T Value { get; }
    }
}
