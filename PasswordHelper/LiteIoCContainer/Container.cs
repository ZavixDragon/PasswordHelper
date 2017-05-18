using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace LiteIoCContainer
{
    public class Container
    {
        private readonly Dictionary<Type, Func<object>> _types = new Dictionary<Type, Func<object>>();

        public void Register<T>(Type type)
        {
            Register(typeof(T), type);
        }

        public void Register(Type target, Type type)
        {
            ValidateRegistration(target, type);
            _types.Add(target, () => Construct(type));
        }

        public void RegisterInstance<T>(T instance)
        {
            RegisterInstance(typeof(T), instance);
        }

        public void RegisterInstance(Type target, object instance)
        {
            ValidateRegistration(target, instance.GetType());
            _types.Add(target, () => instance);
        }

        public bool IsRegistered<T>()
        {
            return IsRegistered(typeof(T));
        }

        public bool IsRegistered(Type type)
        {
            return _types.ContainsKey(type);
        }

        public T Resolve<T>()
        {
            return (T)Resolve(typeof(T));
        }

        public object Resolve(Type type)
        {
            if (!_types.ContainsKey(type))
                throw new InvalidOperationException($"Can't resolve type {type.Name}, because it has not been registered yet.");
            return _types[type]();
        }

        private void ValidateRegistration(Type target, Type type)
        {
            if (type.GetTypeInfo().IsAbstract)
                throw new ArgumentException($"Can't register type {type.Name} as {target.Name}, because {type.Name} is abstract and cannot be constructed.");
            if (!target.GetTypeInfo().IsAssignableFrom(type.GetTypeInfo()))
                throw new ArgumentException($"Can't register type {type.Name} as {target.Name}, because {type.Name} is not assignable from {target.Name}.");
        }

        private object Construct(Type type)
        {
            return Activator.CreateInstance(type, GetParameters(type).ToArray());
        }

        private IEnumerable<object> GetParameters(Type type)
        {
            return GetParameterTypes(type).Select(x => Resolve(x));
        }

        private IEnumerable<Type> GetParameterTypes(Type type)
        {
            return GetConstructor(type).GetParameters().Select(x => x.ParameterType);
        }

        private ConstructorInfo GetConstructor(Type type)
        {
            var viableConstructors = type.GetTypeInfo().DeclaredConstructors.Where(x => IsViableConstructor(x)).ToList();
            if (!viableConstructors.Any())
                throw new InvalidOperationException($"Can't resolve type {type.Name}, because there was no constructor that had all it's argument types registered.");
            return viableConstructors
                .OrderByDescending(x => x.GetParameters().Count())
                .First();
        }

        private bool IsViableConstructor(ConstructorInfo info)
        {
            return info.GetParameters()
                .Select(x => x.ParameterType)
                .All(x => _types.ContainsKey(x));
        }
    }
}
