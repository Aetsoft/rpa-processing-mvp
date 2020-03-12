using System.Threading.Tasks;

namespace Business.Abstraction
{
    public interface ITestClass
    {
        string TestValue { get; }
        Task Run();
    }
}
