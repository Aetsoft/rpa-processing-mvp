using DomainModels.Enums;
using Newtonsoft.Json;

namespace RpaSelfHostedApp.Models
{
    public class DocumentBoxModel
    {
        [JsonProperty(PropertyName = "x")]
        public int X { get; set; }
        [JsonProperty(PropertyName = "y")]
        public int Y { get; set; }
        [JsonProperty(PropertyName = "w")]
        public int Width { get; set; }
        [JsonProperty(PropertyName = "h")]
        public int Height { get; set; }
        [JsonProperty(PropertyName = "lang")]
        public SupportedLangs Language { get; set; }
        [JsonProperty(PropertyName = "content")]
        public string Content { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
    }
}
