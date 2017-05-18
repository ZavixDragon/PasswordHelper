using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using LiteIoCContainer;
using PasswordHelper;
using PasswordHelper.Common;
using PasswordHelperWebApi.DependencyInversion;
using PasswordHelperWebApi.PasswordHandling;

namespace PasswordHelperWebApi
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            var container = new Container();
            var provider = new LiteServiceProvider(container, services.BuildServiceProvider());
            container.RegisterInstance<IServiceScopeFactory>(new LiteServiceScopeFactory(provider));
            container.Register<IPasswordGenerationFactory>(typeof(PasswordGenerationFactory));
            container.RegisterInstance<IStore<Password>>(new InMemoryStore<Password>());
            return provider;
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseMvc();
        }
    }
}
