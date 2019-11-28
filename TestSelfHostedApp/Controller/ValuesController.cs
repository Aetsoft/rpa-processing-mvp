using System;
using System.Collections.Generic;
using System.Web.Http;
using Business.Abstraction;

namespace RpaSelfHostedApp.Controller
{
    public class ValuesController : ApiController
    {
        public ValuesController(ITestClass test, Serilog.ILogger logger,IMessageBus messageBus) :base()
        {
            logger.Debug(test.TestValue);
            messageBus.Run(() => Console.WriteLine("-+"));
            messageBus.Run<ITestClass>((log) => log.Run());
           
        }
        // GET api/values 
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5 
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values 
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5 
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5 
        public void Delete(int id)
        {
        }
    }
}
