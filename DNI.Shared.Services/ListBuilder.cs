using DNI.Shared.Contracts;
using System.Collections.Generic;

namespace DNI.Shared.Services
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
