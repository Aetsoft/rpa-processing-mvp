using Business.Abstraction;
using Business.Implementation;
using LightInject;

namespace IoCRegistration
{
    public static class IoCRegistration
    {
        public static void RegisterAppDependencies(this ServiceContainer container)
        {
            //register other services
            container.RegisterScoped<ITestClass, TestClass>();
            container.RegisterSingleton<IOcrEngine, TesseractInstance>();
        }
    }
}
