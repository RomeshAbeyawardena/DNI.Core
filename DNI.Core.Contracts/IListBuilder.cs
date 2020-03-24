namespace DNI.Core.Contracts
{
    using System;
    using System.Collections.Generic;

    public interface IListBuilder<T>
    {
        IListBuilder<T> Add(T item);

        IListBuilder<T> AddRangle(params T[] items);

        IListBuilder<T> AddRange<TItem>(IEnumerable<TItem> item, Func<TItem, T> getItem);

        IList<T> ToList();

        IEnumerable<T> ToEnumerable();
    }
}
