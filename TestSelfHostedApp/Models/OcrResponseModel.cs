using Newtonsoft.Json;

namespace RpaSelfHostedApp.Models
{
    public class OcrResponseModel
    {
        [JsonProperty(PropertyName = "response")]
        public string OcrTextResponse { get; set; }

        [JsonProperty(PropertyName = "code")]
        public string OcrResponseCode { get; set; }
    }
}
