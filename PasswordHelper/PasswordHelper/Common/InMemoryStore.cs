using System;
using System.Collections.Generic;
using System.Linq;

namespace PasswordHelper.Common
{
    public class InMemoryStore<T> : IStore<T>
    {
        public Dictionary<string, T> _values = new Dictionary<string, T>();

        public void Store(string id, T obj)
        {
            _values[id] = obj;
        }

        public bool Exists(string id)
        {
            return _values.ContainsKey(id);
        }

        public T Retrieve(string id)
        {
            if (_values.ContainsKey(id))
                return _values[id];
            throw new InvalidOperationException($"No item with that the id of {id} has been stored.");
        }

        public List<T> RetrieveWhere(Predicate<T> condition)
        {
            return _values.Values.Where(x => condition(x)).ToList();
        }
    }
}
