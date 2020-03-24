namespace DNI.Core.Services
{
    using System.Collections.Generic;
    using DNI.Core.Contracts;

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
