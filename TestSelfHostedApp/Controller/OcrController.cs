using System;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Business.Abstraction;
using DomainModels.Enums;
using RpaSelfHostedApp.Models;
using RpaSelfHostedApp.Services.Util;
using Swashbuckle.Swagger.Annotations;

namespace RpaSelfHostedApp.Controller
{
    [RoutePrefix("api/ocr")]
    public class OcrController : ApiController
    {
        private readonly Serilog.ILogger _logger;
        private readonly IOcrEngineManager _ocrEngine;
        public OcrController(Serilog.ILogger logger, IOcrEngineManager ocrEngine) : base()
        {
            this._logger = logger;
            this._ocrEngine = ocrEngine;
        }

        /// <summary>
        /// GET api/ocr
        /// </summary>
        /// <returns>Message on OCR services package status</returns>
        [HttpGet]
        [Route("")]
        [SwaggerResponse(HttpStatusCode.OK, "Ocr service up", typeof(string))]
        public IHttpActionResult Get()
        {
            return Ok("ocr service included in package");
        }

        /// <summary>
        /// GET api/ocr/info
        /// </summary>
        /// <returns>Supported language codes</returns>
        [HttpGet]
        [Route("info")]
        [SwaggerResponse(HttpStatusCode.OK, "Ocr Information", typeof(string[]))]
        public IHttpActionResult GetInfo()
        {
            return Ok(Enum.GetNames(typeof(SupportedLangs)));
        }

        /// <summary>
        /// POST api/ocr/base64
        /// </summary>
        /// <param name="json"></param>
        /// <returns>Ocr-ed text for whole document</returns>
        [HttpPost]
        [Route("base64")]
        [SwaggerResponse(HttpStatusCode.OK, "Image was OCR-ed successfully", typeof(OcrResponseModel))]
        public IHttpActionResult Post([FromBody] PostBase64DataOcrRequestModel value)
        {
            OcrResponseModel response = new OcrResponseModel
            {
                OcrResponseCode = "200",
                OcrTextResponse = this._ocrEngine.GetEngineForLang(value.Language).ReadText(value.Base64String)
            };
            return Ok(response);
        }

        /// <summary>
        /// POST api/ocr/bulk
        /// </summary>
        /// <param name="json"></param>
        /// <returns>Ocr-ed text at specified coordinates for the document</returns>
        [HttpPost]
        [Route("bulk")]
        [SwaggerResponse(HttpStatusCode.OK, "Document was OCR-ed successfully", typeof(MultipleFieldsOcrResponseModel))]
        public IHttpActionResult PostAll([FromBody] MultipleFieldsOcrRequestModel json)
        {
            using (Bitmap wholeImage = ImgUtils.toBitmap(json.Base64String))
            {

                var convertedSections = json.Fields.Select(field =>
                {
                    try
                    {
                        Rectangle box = new Rectangle(field.X, field.Y, field.Width, field.Height);
                        return new { field, sector = wholeImage.Clone(box, wholeImage.PixelFormat) };
                    }
                    catch (Exception e)
                    {
                        this._logger.Error(e, e.Message);
                        return null;
                    }
                   

                }).ToArray().Where(n => n.sector != null).ToArray();

                try
                {
                    Parallel.ForEach(convertedSections, field =>
                    {
                        field.field.Content = this._ocrEngine.GetEngineForLang(field.field.Language).ReadText(field.sector);
                    });

                }
                catch (Exception e)
                {
                    foreach (var section in convertedSections)
                    {
                        section.sector?.Dispose();
                    }

                    throw;
                }


                MultipleFieldsOcrResponseModel response = new MultipleFieldsOcrResponseModel()
                {
                    Fields = json.Fields,
                    OcrResponseCode = "200"
                };

                return Ok(response);
            }
        }
    }
}
