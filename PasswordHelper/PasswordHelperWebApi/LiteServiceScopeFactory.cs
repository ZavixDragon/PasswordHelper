using System;
using Microsoft.Extensions.DependencyInjection;

namespace PasswordHelperWebApi
{
    public class LiteServiceScopeFactory : IServiceScopeFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public LiteServiceScopeFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IServiceScope CreateScope()
        {
            return new LiteServiceScope(_serviceProvider);
        }
    }
}
