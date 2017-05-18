using LiteIoCContainer;
using System;

namespace PasswordHelperWebApi
{
    public class LiteServiceProvider : IServiceProvider
    {
        private readonly Container _container;
        private readonly IServiceProvider _fallback;

        public LiteServiceProvider(Container container, IServiceProvider fallback)
        {
            _container = container;
            _fallback = fallback;
        }

        public object GetService(Type serviceType)
        {
            if (_container.IsRegistered(serviceType))
                return _container.Resolve(serviceType);
            return _fallback.GetService(serviceType);
        }
    }
}
