using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class OcrResponseModel
    {
        [JsonProperty(PropertyName = "response")]
        private string OcrTextResponse;

        [JsonProperty(PropertyName = "code")]
        private string OcrResponseCode;

        public OcrResponseModel() { }

        public void SetOcrTextResponse(string text)
        {
            this.OcrTextResponse = text;
        }

        public void SetOcrResponseCode(string text)
        {
            this.OcrResponseCode = text;
        }
    }
}
