using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin;
using Serilog.Context;

namespace TestSelfHostedApp.Logger
{
    public class OwinExceptionHandlerMiddleware : OwinMiddleware
    {
        private readonly Serilog.ILogger logger;

        public OwinExceptionHandlerMiddleware(OwinMiddleware next, Serilog.ILogger logger)
            : base(next)
        {
            this.logger = logger;
        }

        public override async Task Invoke(IOwinContext context)
        {
            using (LogContext.PushProperty("RequestId", Guid.NewGuid()))
            {
                try
                {
                    await this.Next.Invoke(context).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    this.logger.Error(ex, $"{nameof(OwinExceptionHandlerMiddleware)} caught exception.");
                    throw;
                }
            }
        }
    }
}
