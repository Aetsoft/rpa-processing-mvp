using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RpaSolution
{
    class Program: TestSelfHostedApp.Program
    {
        static void Main(string[] args)
        {
            string baseAddress = "http://localhost:9000/";

            Start<RpaSolution.Startup>(baseAddress);
        }
    }
}
