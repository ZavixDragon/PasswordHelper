using System;
using Microsoft.Extensions.DependencyInjection;

namespace PasswordHelperWebApi
{
    public class LiteServiceScope : IServiceScope
    {
        public IServiceProvider ServiceProvider { get; }

        public LiteServiceScope(IServiceProvider provider)
        {
            ServiceProvider = provider;
        }

        public void Dispose()
        {
        }
    }
}
