using System;
using System.Collections.Generic;
using DomainModels.Enums;
using Newtonsoft.Json;

namespace RpaSelfHostedApp.Models
{
    public class BaseOcrRequestModel 
    {
        [JsonProperty(PropertyName = "base64")]
        public string Base64String { get; set; }
    }

    public class PostBase64DataOcrRequestModel : BaseOcrRequestModel
    {
        [JsonProperty(PropertyName = "lang")]
        public SupportedLangs Language { get; set; }
    }

    public class MultipleFieldsOcrRequestModel : BaseOcrRequestModel
    {
        [JsonProperty(PropertyName = "fields")]
        public List<DocumentBoxModel> Fields { get; set; }
    }
}
