using System;

namespace PasswordHelper.Common
{
    public class Value<T>
    {
        private T _obj; 
        private readonly Func<T> _getObj;

        public Value(Func<T> getObj)
        {
            _getObj = getObj;
        }

        public T Get()
        {
            if (_obj == null)
                _obj = _getObj();
            return _obj;
        }
    }
}
