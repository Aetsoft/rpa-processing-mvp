using System.Diagnostics;
using IoCRegistration;
using LightInject;

namespace RpaSolution
{
    public class Startup: TestSelfHostedApp.Startup
    {
      
        public override void RegisterDependencies(ServiceContainer container)
        {
            container.RegisterAppDependencies();
        }
    }
}
