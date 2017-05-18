using System;
using System.Collections.Generic;

namespace PasswordHelper.Common
{
    public interface IStore<T>
    {
        void Store(string id, T obj);
        bool Exists(string id);
        T Retrieve(string id);
        List<T> RetrieveWhere(Predicate<T> condition);
    }
}
