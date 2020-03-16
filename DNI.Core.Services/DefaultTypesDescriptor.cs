using DNI.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DNI.Core.Services
{
    public static class TypesDescriptor
    {
        public static ITypesDescriptor Describe<T>()
        {
            return Create()
                .Describe<T>();
        }

        public static ITypesDescriptor Describe(Type type = default)
        {
            var typeDescriptor = Create();

            if(type != null)
               return typeDescriptor.Describe(type);

            return typeDescriptor;
        }

        public static ITypesDescriptor Create()
        {
            return new DefaultTypesDescriptor();

        }
    }

    internal class DefaultTypesDescriptor : ITypesDescriptor
    {
        public IEnumerable<ITypeDescriptor> DescribedTypes => describedTypes.ToArray();
        private readonly IList<ITypeDescriptor> describedTypes;
        public ITypesDescriptor Describe<T>()
        {
            describedTypes.Add(new DefaultTypeDescriptor<T>());
            return this;
        }

        public IEnumerable<Type> ToTypeArray()
        {
            return describedTypes
                .Select(describedType => describedType.Type);
        }

        public ITypesDescriptor Describe(Type type)
        {
            describedTypes.Add(new DefaultTypeDescriptor(type));
            return this;
        }

        public DefaultTypesDescriptor()
        {
            describedTypes = new List<ITypeDescriptor>();
        }
    }

    internal class DefaultTypeDescriptor : ITypeDescriptor
    {
        public DefaultTypeDescriptor(Type type)
        {
            Type = type;
        }

        public Type Type { get; }
    }

    internal class DefaultTypeDescriptor<T> : DefaultTypeDescriptor, ITypeDescriptor<T>
    {
        public DefaultTypeDescriptor()
            : base(typeof(T))
        {

        }
    }
}
