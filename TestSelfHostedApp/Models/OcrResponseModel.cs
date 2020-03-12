using Newtonsoft.Json;
using System.Collections.Generic;

namespace RpaSelfHostedApp.Models
{
    public class ResponseModel
    {
        [JsonProperty(PropertyName = "code")]
        public string OcrResponseCode { get; set; }
    }
    public class OcrResponseModel : ResponseModel
    {
        [JsonProperty(PropertyName = "response")]
        public string OcrTextResponse { get; set; }
    }

    public class MultipleFieldsOcrResponseModel : ResponseModel
    {
        [JsonProperty(PropertyName = "fields")]
        public List<DocumentBoxModel> Fields { get; set; }
    }
}
