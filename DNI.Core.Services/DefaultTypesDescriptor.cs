using DNI.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Services
{
    public static class TypesDescriptor
    {
        public static ITypesDescriptor Describe<T>()
        {
            return new DefaultTypesDescriptor()
                .Describe<T>();
        }
    }

    internal class DefaultTypesDescriptor : ITypesDescriptor
    {
        public IEnumerable<ITypeDescriptor> DescribedTypes => describedTypes.ToArray();
        private readonly IList<ITypeDescriptor> describedTypes;
        public ITypesDescriptor Describe<T>()
        {
            describedTypes.Add(new DefaultTypeDescritor<T>());
            return this;
        }

        public IEnumerable<Type> ToTypeArray()
        {
            return describedTypes
                .Select(describedType => describedType.Type);
        }

        public DefaultTypesDescriptor()
        {
            describedTypes = new List<ITypeDescriptor>();
        }
    }

    internal class DefaultTypeDescritor<T> : ITypeDescriptor<T>
    {
        public Type Type => typeof(T);
    }
}
