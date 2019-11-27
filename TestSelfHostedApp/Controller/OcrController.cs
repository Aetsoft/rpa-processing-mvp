using Business.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Domain.Models;
using Business.Implementation;
using System.Drawing;

namespace RpaSolution.Controller
{
    public class OcrController : ApiController
    {

        public OcrController(ITestClass test, Serilog.ILogger logger, IMessageBus messageBus) : base()
        {
            logger.Debug(test.TestValue);
            messageBus.Run(() => Console.WriteLine("-+"));
            messageBus.Run<ITestClass>((log) => log.Run());

        }
        // GET api/ocr
        public string Get()
        {
            return "ocr service included in package";
        }

        // POST api/ocr/base64
        public OcrResponseModel Post([FromBody] PostBase64DataOcrRequestModel value)
        {
            OcrResponseModel response = new OcrResponseModel();
            try
            {
                Bitmap bitImage = new Bitmap(Image.FromStream(new System.IO.MemoryStream(Convert.FromBase64String(value.GetBase64String()))));
                string text = TesseractInstance.getInstance().ReadText(bitImage);

                response.SetOcrResponseCode("200");
                response.SetOcrTextResponse(text);
            } catch
            {
                response.SetOcrTextResponse(null);
                response.SetOcrResponseCode("500");
            }

            return response;
            
        }
    }
}
