using System.Web.Http;
using Business.Abstraction;
using IoCRegistration;
using LightInject;
using LightInject.WebApi;
using Owin;
using Serilog;
using TestSelfHostedApp.Services;

namespace TestSelfHostedApp.App_config
{
    public static class IoCRegistration
    {
        public static ServiceContainer RegisterDependencies(this IAppBuilder appBuilder, HttpConfiguration configuration)
        {
            var container = new ServiceContainer();
            container.RegisterApiControllers();
            container.SetDefaultLifetime<PerRequestLifeTime>();

           

            //register other services
            container.RegisterScoped<ILogger>(factory => Serilog.Log.Logger);
            container.RegisterSingleton<IMessageBus, MessageBus>();

            container.RegisterAppDependencies();

            configuration.DependencyResolver = new LightInjectWebApiDependencyResolver(container);


            return container;
        }
    }
}
