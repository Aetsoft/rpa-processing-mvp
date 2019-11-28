using System;
using DomainModels.Enums;
using Newtonsoft.Json;

namespace RpaSelfHostedApp.Models
{
    public class PostBase64DataOcrRequestModel
    {
        [JsonProperty(PropertyName = "base64")]
        public string Base64String { get; set; }
        
        [JsonProperty(PropertyName = "lang")]
        public SupportedLangs Language { get; set; }
        
    }
}
