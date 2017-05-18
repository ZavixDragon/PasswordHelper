using PasswordHelper;
using PasswordHelper.Common;
using System;
using System.Linq;
using Xunit;

namespace PasswordHelperTests
{
    public class MemoryStoreTests
    {
        private readonly MemoryStore<string> _store;

        public MemoryStoreTests()
        {
            _store = new MemoryStore<string>();
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void RetrieveNonExistentInfo_InvalidOperationException()
        {
            Assert.Throws<InvalidOperationException>(() => _store.Retrieve("non-existent id"));
        }

        [Theory]
        [Trait("Category", "Unit")]
        [InlineData("something")]
        [InlineData("banana")]
        public void RetrieveStoredInfo_SameAsWhatWasStored(string item)
        {
            _store.Store("id", item);

            var retrievedItem = _store.Retrieve("id");

            Assert.Equal(retrievedItem, item);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void StoreTwoThings_RetrievesCorrectItem()
        {
            _store.Store("id1", "right");
            _store.Store("id2", "wrong");

            var item = _store.Retrieve("id1");

            Assert.Equal("right", item);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void OverwriteStore_RetrieveOverwriteValue()
        {
            _store.Store("id", "wrong");
            _store.Store("id", "right");

            var item = _store.Retrieve("id");

            Assert.Equal("right", item);
        }

        [Theory]
        [Trait("Category", "Unit")]
        [InlineData("sdfsd", "iwejre")]
        [InlineData("sdfjweifjaj", "jre", "sadih", "ssiofiwea")]
        public void RetrieveWhereConditionIsMet_CorrectlyReturnsList(params string[] input)
        {
            Predicate<string> condition = x => x.Length > 5;
            input.ForEachIndex((x, i) => _store.Store(i.ToString(), x));

            var filteredList = _store.RetrieveWhere(condition);

            Assert.Equal(input.Count(x => condition(x)), filteredList.Count);
            filteredList.ForEach(x => Assert.True(input.Contains(x)));
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void AskIfNonStoredIDExists_ReturnsFalse()
        {
            var doesExist = _store.Exists("non used id");

            Assert.False(doesExist);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void AskIfStoredIDExists_ReturnsTrue()
        {
            _store.Store("id", "something");

            var doesExist = _store.Exists("id");

            Assert.True(doesExist);
        }
    }
}
