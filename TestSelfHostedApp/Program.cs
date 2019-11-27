using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.Hosting;
using Serilog;

namespace TestSelfHostedApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            string baseAddress = "http://localhost:9000/";

            Start<Startup>(baseAddress);
        }

        protected static void Start<T>(string baseAddress) where T : Startup
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .CreateLogger();

            Log.Logger.Warning($"Run application on {baseAddress}");

            try
            {
                // Start OWIN host 
                using (WebApp.Start<Startup>(url: baseAddress))
                {
                    Log.Information("Start browser...");

                    System.Diagnostics.Process.Start(baseAddress);
                    if (Debugger.IsAttached)
                    {
                        System.Diagnostics.Process.Start(baseAddress + "swagger/");
                    }

                    Log.Information("Press any button to stop...");
                    Console.ReadLine();
                }
            }
            catch (Exception e)
            {
                Log.Fatal(e, "Error on start");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}
