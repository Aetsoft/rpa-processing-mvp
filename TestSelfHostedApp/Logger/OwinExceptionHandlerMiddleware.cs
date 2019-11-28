using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Results;
using Microsoft.Owin;
using Newtonsoft.Json;
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
                catch (Exception exception)
                {
                    this.logger.Error(exception, $"{nameof(OwinExceptionHandlerMiddleware)} caught exception.");

                    var errorDataModel = new 
                    {
                        Message = "Internal server error occurred, error has been reported!",
                        Details = exception.Message,
                        ErrorReference = exception.Data["ErrorReference"] != null 
                            ? exception.Data["ErrorReference"].ToString() 
                            : string.Empty,
                        DateTime = DateTime.UtcNow
                    };

                    context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                    context.Response.ReasonPhrase = "Internal Server Error";
                    context.Response.ContentType = "application/json";
                    context.Response.Write(JsonConvert.SerializeObject(errorDataModel));
                }
            }
        }

        public class PassthroughExceptionHandler : ExceptionHandler, IExceptionHandler
        {
            public override void Handle(ExceptionHandlerContext context)
            {
                var info = ExceptionDispatchInfo.Capture(context.Exception);
                info.Throw();
            }
        }
    }
}
