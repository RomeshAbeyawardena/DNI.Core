using DNI.Core.Contracts;
using System.Collections.Generic;

namespace DNI.Core.Services
{
    public static class ListBuilder
    {
        public static IListBuilder<T> Create<T>()
        {
            return DefaultListBuilder<T>.Create();
        }

        public static IListBuilder<T> Create<T>(IList<T> list)
        {
            return DefaultListBuilder<T>.Create(list);
        }
    }
}
