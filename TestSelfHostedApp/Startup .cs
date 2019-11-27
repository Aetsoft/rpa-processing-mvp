using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Hangfire;
using Hangfire.LiteDB;
using IoCRegistration;
using LightInject;
using Microsoft.Owin;
using Microsoft.Owin.BuilderProperties;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.Logging;
using Microsoft.Owin.StaticFiles;
using Owin;
using Serilog;
using Swashbuckle.Application;
using TestSelfHostedApp.App_config;
using TestSelfHostedApp.Logger;

namespace TestSelfHostedApp
{
    public class Startup
    {
        // This code configures Web API. The Startup class is specified as a type
        // parameter in the WebApp.Start method.
        public virtual void Configuration(IAppBuilder appBuilder)
        {
            appBuilder.Use<OwinExceptionHandlerMiddleware>(Log.Logger);

            // Configure Web API for self-host. 
            HttpConfiguration config = new HttpConfiguration();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            config.EnableSwagger(c =>
                {
                    c.SingleApiVersion("v1", "RPA web API");
                })
                .EnableSwaggerUi();

            appBuilder.UseWebApi(config);
            var container =  appBuilder.RegisterDependencies(config);
            this.RegisterDependencies(container);

            GlobalConfiguration.Configuration
                .UseLiteDbStorage()
                .UseLightInjectActivator(container)
                .UseSerilogLogProvider();

            

            var physicalFileSystem = new PhysicalFileSystem(@".\Web"); //. = root, Web = your physical directory that contains all other static content, see prev step
            var options = new FileServerOptions
            {
                EnableDefaultFiles = true,
                FileSystem = physicalFileSystem,
                StaticFileOptions = {FileSystem = physicalFileSystem, ServeUnknownFileTypes = false},
                DefaultFilesOptions = {DefaultFileNames = new[] {"index.html"}},
            };

            //put whatever default pages you like here
            appBuilder.UseFileServer(options);

            var properties = new AppProperties(appBuilder.Properties);
            CancellationToken token = properties.OnAppDisposing;

            var client = new BackgroundJobServer(new BackgroundJobServerOptions()
            {
                SchedulePollingInterval = TimeSpan.FromSeconds(1),
                ServerCheckInterval = TimeSpan.FromSeconds(1)
            }, JobStorage.Current);

            if (token != CancellationToken.None)
            {
                var fullNameOfDll = Assembly.GetExecutingAssembly().FullName;
                token.Register(() =>
                {
                    Log.Logger.Warning($"Close {this.GetType().FullName} application \n  {fullNameOfDll}");
                    client.Dispose();
                    container.Dispose();
                });
            }
            else
            {
                Log.Fatal("Error with application start");
            }

        }

        public virtual void RegisterDependencies(ServiceContainer container)
        {
            container.RegisterAppDependencies();
        }
    }
}
