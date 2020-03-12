using System;
using System.Reflection;
using System.Threading;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using Hangfire;
using Hangfire.LiteDB;
using Microsoft.Owin.BuilderProperties;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.StaticFiles;
using Owin;
using RpaSelfHostedApp.App_config;
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
        bool _wasDisposed = false;

        public virtual void Configuration(IAppBuilder appBuilder)
        {
            appBuilder.Use<OwinExceptionHandlerMiddleware>(Log.Logger);


            // Configure Web API for self-host. 
            HttpConfiguration config = new HttpConfiguration();
            config.Services.Replace(typeof(IExceptionHandler), new OwinExceptionHandlerMiddleware.PassthroughExceptionHandler());

            config.MapHttpAttributeRoutes();

            config.EnableSwagger(c =>
                {
                    c.SingleApiVersion("v1", "RPA web API");
                    c.DescribeAllEnumsAsStrings();
                    c.IncludeXmlComments("RpaSelfHostedApp.xml");
                    c.OperationFilter<SwaggerParameterOperationFilter>();
                })
                .EnableSwaggerUi();

            appBuilder.UseWebApi(config);
            appBuilder.MapSignalR();

            var container =  appBuilder.RegisterDependencies(config);

            Hangfire.GlobalConfiguration.Configuration
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

            Action disposeObjects = () =>
            {
                if (!_wasDisposed)
                {
                    _wasDisposed = true;
                    var fullNameOfDll = Assembly.GetExecutingAssembly().FullName;
                    Log.Logger.Warning($"Close {this.GetType().FullName} application \n  {fullNameOfDll}");
                    client.Dispose();
                    container.Dispose();
                }
            };
            
            if (token != CancellationToken.None)
            {
                client.WaitForShutdown(TimeSpan.FromSeconds(10));
                token.Register(disposeObjects);
            }
            else
            {
                Log.Fatal("Error with application start");
            }
            AppDomain.CurrentDomain.ProcessExit += (sender, args) => { disposeObjects(); };
        }
        
    }
}
