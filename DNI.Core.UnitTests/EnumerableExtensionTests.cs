using NUnit.Framework;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DNI.Core.Shared.Extensions;
using System.Collections.Generic;

namespace DNI.Core.UnitTests
{
    public class EnumerableExtensionTests
    {
        private readonly Random _random;

        public EnumerableExtensionTests()
        {
            _random = new Random();
        }

        [Test]
        public void ForEach_returns_leaving_original_list_unchanged()
        {
            var myList = new System.Collections.Generic.List<int>(new [] { 1, 2, 3, 5, 6, 7, 8, 9, 10 });

            var newList = EnumerableExtensions.ForEach(myList, item => item * 2 );

            CollectionAssert.AreNotEqual(myList, newList);
        }

        [TestCase(2,new [] {1, 2, 3 })]
        [TestCase(4,new [] {1, 2, 3, 4, 5 })]
        [TestCase(9,new [] {1, 2, 3, 4, 5,6,7,8,9,10 })]
        public void ForEach_runs_on_items_that_meet_condition(int expectedCollectionLength, int[] originalCollection)
        {
            var actualCollection = EnumerableExtensions.ForEach(originalCollection, 
                item => item * 2, item=> item > 1);

            Assert.AreEqual(expectedCollectionLength, actualCollection.Count());
        }

        [Test]
        public async Task ForEachAsync_returns()
        {
            var myList = new System.Collections.Generic.List<int>(new [] { 1, 2, 3, 5, 6, 7, 8, 9, 10 });

            var newList = EnumerableExtensions.ForEachAsync(myList, async (t) => await DoWorkAsync(t, 500, 3500, t1 => t1 * 2));

            CollectionAssert.AreNotEqual(myList, await newList);
        }

        
        [Test]
        public async Task ForEachAsync()
        {
            var myList = new List<int>(new [] { 1, 2, 3, 5, 6, 7, 8, 9, 10 });
            var newList = new List<int>();
            await EnumerableExtensions.ForEachAsync(myList, async (t) => await DoWorkAsync(t, 500, 3500, t1 => newList.Add(t1 * 2)));

            CollectionAssert.AreNotEqual(myList, newList);
        }

        private async Task<T> DoWorkAsync<T>(T item, int from, int upto, Func<T, T> work)
        {
            var delayInterval = _random.Next(from, upto);
            await Task.Delay(delayInterval);
            return await Task.FromResult(work(item));
        }

        private async Task DoWorkAsync<T>(T item, int from, int upto, Action<T> work)
        {
            var delayInterval = _random.Next(from, upto);
            await Task.Delay(delayInterval);
            work(item);
            return;
        }
    }
}
