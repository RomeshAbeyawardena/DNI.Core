using NUnit.Framework;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DNI.Core.Shared.Extensions;

namespace DNI.Core.UnitTests
{
    public class EnumerableExtensionTests
    {
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
    }
}
