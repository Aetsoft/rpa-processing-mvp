using System;
using System.Net;
using System.Web.Http;
using Business.Abstraction;
using RpaSelfHostedApp.Models;
using Swashbuckle.Swagger.Annotations;

namespace RpaSelfHostedApp.Controller
{
    public class OcrController : ApiController
    {
        private readonly Serilog.ILogger _logger;
        private readonly IOcrEngine _ocrEngine;
        public OcrController(Serilog.ILogger logger, IOcrEngine ocrEngine) : base()
        {
            this._logger = logger;
            this._ocrEngine = ocrEngine;
        }
        // GET api/ocr
        [SwaggerResponse(HttpStatusCode.OK, "Document was OCR-ed successfully", typeof(string))]
        public IHttpActionResult Get()
        {
            return Ok("ocr service included in package");
        }

        /// <summary>
        ///  POST api/ocr/base64
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [SwaggerResponse(HttpStatusCode.OK, "Document was OCR-ed successfully", typeof(OcrResponseModel))]
        public IHttpActionResult Post([FromBody] PostBase64DataOcrRequestModel value)
        {
            OcrResponseModel response = new OcrResponseModel
            {
                OcrResponseCode = "200",
                OcrTextResponse = this._ocrEngine.ReadText(value.Base64String, value.Language)
            };
            return Ok(response);
        }
    }
}
