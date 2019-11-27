using Business.Abstraction;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Implementation
{
    public class TestClass: ITestClass, IDisposable
    {
        public string TestValue
        {
            get
            {
                
                return "Test Class";
            }
        }

        public async Task Run()
        {
            long tics = DateTime.Now.Ticks;
            
            var finalMessage = $"Get #{tics} data  - {Thread.CurrentThread.ManagedThreadId}";
            await Task.Delay(TimeSpan.FromSeconds(5)).ConfigureAwait(false);
            Console.WriteLine($"{finalMessage} -> {Thread.CurrentThread.ManagedThreadId}");
        }

        public void Dispose()
        {
            Console.WriteLine("Dispose");
        }
    }
}
