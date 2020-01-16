using DNI.Shared.Contracts;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Shared.Services
{
    internal class DefaultListBuilder<T> : IListBuilder<T>
    {
        private readonly IList<T> _list;

        public IListBuilder<T> Add(T item)
        {
            _list.Add(item);
            return this;
        }
        
        public IListBuilder<T> AddRangle(params T[] items)
        {
            foreach(var item in items)
                Add(item);

            return this;
        }

        public IListBuilder<T> AddRange<TItem>(IEnumerable<TItem> items, Func<TItem, T> getItem)
        {
            var array = items.Select(item => getItem(item)).ToArray();

            foreach(var item in array)
                Add(item);

            return this;
        }


        public IEnumerable<T> ToEnumerable()
        {
            return _list.ToArray();
        }

        public IList<T> ToList()
        {
            return _list.ToList();
        }

        private DefaultListBuilder(IList<T> list)
        {
            _list = list;
        }

        public static IListBuilder<T> Create()
        {
            return new DefaultListBuilder<T>(new List<T>());
        }

        public static IListBuilder<T> Create(IList<T> list)
        {
            return new DefaultListBuilder<T>(new List<T>(list));
        }
    }
}
